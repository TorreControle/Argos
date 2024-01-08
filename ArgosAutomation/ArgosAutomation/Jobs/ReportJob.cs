using ArgosAutomation.Abstractions;
using ArgosAutomation.Databases;
using Quartz;
using System.Data;
using System.Media;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation.Jobs
{
    /// <summary>
    /// Classe que representa o trabalho report da aplicação, responsável por conter o fluxo de um report
    /// </summary>
    [DisallowConcurrentExecution]
    public class ReportJob : IJob
    {
        /// <summary>
        /// Nome do trabalho sendo executado.
        /// </summary>
        public static string? JobName { get; set; }
        /// <summary>
        /// Grupo na qual o trabalho está associado.
        /// </summary>
        public static string? JobGroup { get; set; }
        /// <summary>
        /// Nome do gatilho do trabalho.
        /// </summary>
        public static string? TriggerName { get; set; }
        /// <summary>
        /// Grupo na qual a trigger do trabalho pertence.
        /// </summary>
        public static string? TriggerGroup { get; set; }
        /// <summary>
        /// Identificador único do trabalho.
        /// </summary>
        public static int Id { get; set; }
        /// <summary>
        /// Nome do painel.
        /// </summary>
        public static string ReportName { get; set; }
        /// <summary>
        /// Nome do grupo no Telegram.
        /// </summary>
        public static List<string> GroupName = new();
        /// <summary>
        /// Identificador único do chat.
        /// </summary>
        public static List<long> ChatIdGroup = new();
        /// <summary>
        /// Operação na qual o report pertence.
        /// </summary>
        public static List<string> Operation = new();
        /// <summary>
        /// Hora em que o trabalho é acionado.
        /// </summary>
        public static string? ReportTime { get; set; }
        /// <summary>
        /// Armazena se o painel está sendo gerado ou não.
        /// </summary>
        public int BeingGenerated { get; set; }
        /// <summary>
        /// Objeto do tipo Report para fluxo de geração e envio de reports.
        /// </summary>
        public static Report? Report { get; set; }

        /// <summary>
        /// Método responsável pelo fluxo de execução dos trabalhos defunidos pela regra de negócio.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // Acessando valores e informações definidas na criação dos trablhos.
                IJobDetail Jobkey = context.JobDetail;
                ITrigger triggerKey = context.Trigger;
                JobDataMap dataMap = context.MergedJobDataMap;

                // Atribuindo valores.
                JobName = Jobkey.Key.Name;
                JobGroup = Jobkey.Key.Group;
                TriggerName = triggerKey.Key.Name;
                TriggerGroup = triggerKey.Key.Group;
                ReportTime = dataMap.GetString("Hora");

                // Saída no console sobre a execução dos trabalhos
                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: {JobName} em execução.");

                // Alerta no telegram para os administradores sobre a execução dos trabalhos.
                await Utilities.botClient.SendTextMessageAsync(
                    chatId: 5495003005,
                    text: $@" *{JobName}* em execução 📊.

Trabalho faz parte do grupo {JobGroup} e sendo executado as *{DateTime.Now}*.",
                    parseMode: ParseMode.Markdown,
                    cancellationToken: Utilities.cts);

                // Obtém os reports do trabalho em questão atráves da hora.
                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                string qry = "qryGetReportsByHour.txt";
                Odbc.dtm.CleanParamters(qry);
                Odbc.dtm.ParamByName(qry, ":HORA_DIVULGACAO", ReportTime);
                DataTable dt = Odbc.dtm.ExecuteQuery(qry);

                // Fluxo de geração e envio dos reports, itera sobre todos os paineis respectivos ao horário, gera e envia um por um.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // Atribui os valores as propriedades 
                    Id = int.Parse((string)dt.Rows[i]["ID"]);
                    ReportName = (string)dt.Rows[i]["NOME"];

                    // Constrói o report em questão através do ID.
                    Report = new(id: Id, reportTime: ReportTime);
                    Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: Nova instância do painel de {ReportName} criada.");

                    //
                    Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                    qry = "qryGetGroupsReport.txt";
                    Odbc.dtm.CleanParamters(qry);
                    Odbc.dtm.ParamByName(qry, ":ID", Id.ToString());
                    var dtx = Odbc.dtm.ExecuteQuery(qry);

                    for (int j = 0; j < dtx.Rows.Count; j++)
                    {
                        GroupName.Add((string)dtx.Rows[j]["NOME_GRUPO_TELEGRAM"]);
                        ChatIdGroup.Add(long.Parse((string)dtx.Rows[j]["CHAT_ID_GRUPO"]));
                        Operation.Add((string)dtx.Rows[j]["OPERACAO"]);
                    }

                    // Verifica se o painel está ativado ou não.
                    if (Report.Enable == 1)
                    {
                        // Obtém os todos os valores da coluna "GERANDO" dentro da tabela t_painel_automation, verifica se há algum valor igual a true e grava o resultado dentro da propriedade BeingGenerated.
                        Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: O painel de {ReportName} está ativo.");
                        Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                        qry = "qryGetReports.txt";
                        var dts = Odbc.dtm.ExecuteQuery(qry);
                        //Odbc.dtm.Disconect();
                        BeingGenerated = Tools.HasTrueValueInColumn(dts, "GERANDO");

                        // Verifica se há algum painel sendo gerado.
                        if (BeingGenerated == 0)
                        {
                            // Caso não tiver, começa a gerar o painel solicitado e faz o envio.
                            await Report.Generate();

                            // Faz um insert na tabela "argos.t_historico_divulgacao_automation" para controle e geração de dados dos envios de report e o envio do print no telegram.
                            for (int j = 0; j < ChatIdGroup.Count; j++)
                            {
                                await Report.ToSend(ChatIdGroup[j]);
                                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                                qry = "qryInsertHistoricDivulgations.txt";
                                Odbc.dtm.CleanParamters(qry);
                                Odbc.dtm.ParamByName(qry, ":ID_HISTORICO", Guid.NewGuid().ToString());
                                Odbc.dtm.ParamByName(qry, ":ID_PAINEL", Id.ToString());
                                Odbc.dtm.ParamByName(qry, ":NOME_PAINEL", ReportName);
                                Odbc.dtm.ParamByName(qry, ":OPERACAO", Operation[j]);
                                Odbc.dtm.ParamByName(qry, ":HORA_AGENDAMENTO", ReportTime);
                                Odbc.dtm.ExecuteNonQuery(qry);
                                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: Insert na tabela argos.t_historico_divulgacao_automation do report de {ReportName} feito as {DateTime.Now:HH:mm:ss} foi realizado com exito.");
                                //Odbc.dtm.Disconect(); 
                            }

                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: Conflito entre de reports, uma pessoa chamado(a) {UpdateHandler.FirstName} {UpdateHandler.LastName} solicitou o painel de {UpdateHandler.MessageText} em paralelo ao trabalho {JobName} durante o report de {ReportName}.");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;

                            // Caso algum painel esteja sendo gerado ele envia um alerta ao usuário.
                            await Utilities.botClient.SendTextMessageAsync(
                                chatId: UpdateHandler.ChatId,
                                text: $"🤖: {UpdateHandler.FirstName}! Sua solicitação do painel de *{UpdateHandler.MessageText}* foi um pouco adiada pois estou gerando o report automático de *{ReportName}* no grupo do *{GroupName}*, você receberá os dados atualizados de *{UpdateHandler.MessageText}* em breve!.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);

                        }
                    }
                    else
                    {

                        for (int j = 0; j < ChatIdGroup.Count; j++)
                        {
                            // Caso o painel em questão esteja desetivado o bot envia um alerta ao usuário.
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] ReportJob: O painel de {ReportName} não está ativo, abortando report.");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            await Utilities.botClient.SendTextMessageAsync(
                                    chatId: ChatIdGroup[j],
                                    text: $"🤖: Pessoal, o painel de *{ReportName}* foi desativado automaticamente devido a manutenção nos dados ou no layout, o time de dados da TI/Torre de Controle já está atuando e assim que normalizar ativarei novamente esse painel!",
                                    parseMode: ParseMode.Markdown,
                                    cancellationToken: Utilities.cts);
                        }
                    }

                    Console.WriteLine(" ");
                    GroupName.Clear();
                    ChatIdGroup.Clear();
                    Operation.Clear();
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erros genéricos.
                await Utilities.botClient.SendTextMessageAsync(
                chatId: 5495003005,
                text: @$"*Manipulador de erros acionado* 🪲 - {DateTime.Now}

*Classe:* ReportJob.cs 📊

Erro no trabalho da *{JobName}* realizado as *{ReportTime}* devido a {ex.Message}",
                parseMode: ParseMode.Markdown,
                cancellationToken: Utilities.cts);
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(@$"Manipulador de erros acionado - {DateTime.Now}

Classe: ReportJob.cs

Erro no trabalho da {JobName} realizado as {ReportTime} devido a {ex.Message} 

{ex}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.sound = new SoundPlayer();
                Program.sound.SoundLocation = @$"{Tools.GetDirectoryProject()}\Resources\AlertError.wav";
                Program.sound.Play();
                Program.sound.Dispose();

            }
        }
    }
}

