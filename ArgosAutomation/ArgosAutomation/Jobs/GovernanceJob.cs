using ArgosAutomation.Databases;
using Quartz;
using System.Data;
using System.Media;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    [DisallowConcurrentExecution]
    public class GovernanceJob : IJob
    {
        // <summary>
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
        /// Horário em que o trabalho é executado.
        /// </summary>
        public static string? Time { get; set; }
        /// <summary>
        /// Id da query em questão.
        /// </summary>
        public int? QueryId { get; set; }
        /// <summary>
        /// Grupo de dados em que o BI está alocado.
        /// </summary>
        public string? GroupData { get; set; }
        /// <summary>
        /// Script da query de governança.
        /// </summary>
        public string? Script { get; set; }
        /// <summary>
        /// Id do painel em questão.
        /// </summary>
        public int? ReportId { get; set; }
        /// <summary>
        /// Nome do painel em questão
        /// </summary>
        public List<string?> ReportName = new();
        /// <summary>
        /// Chat Id do grupo do painel em questão.
        /// </summary>
        public List<long?> ChatIdGroup = new();
        /// <summary>
        /// Nome do grupo do painel em questão.
        /// </summary>
        public List<string?> GroupName = new();
        /// <summary>
        /// Armazena se o painel está ativo ou não.
        /// </summary>
        public int? Enable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // Acessando valores e informações definidas na criação dos trablhos.
            IJobDetail Jobkey = context.JobDetail;
            ITrigger triggerKey = context.Trigger;
            JobDataMap dataMap = context.MergedJobDataMap;

            // Atribuindo valores as propriedades do trabalho e da trigger.
            JobName = Jobkey.Key.Name;
            JobGroup = Jobkey.Key.Group;
            TriggerName = triggerKey.Key.Name;
            TriggerGroup = triggerKey.Key.Group;
            Time = dataMap.GetString("Hora");

            // Saída no console sobre a execução dos trabalhos
            Console.WriteLine(@$"
 [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: {JobName} em execução.");

            // Alerta no telegram para os administradores sobre a execução dos trabalhos.
            await Utilities.botClient.SendTextMessageAsync(
                chatId: 5495003005,
                text: $@"*{JobName}* em execução 👮🏾‍♂️.

🤖: Trabalho que faz parte do grupo {JobGroup} está sendo executado as *{DateTime.Now}*.",
                parseMode: ParseMode.Markdown,
                cancellationToken: Utilities.cts);

            try
            {
                // Obtém as queries respectivas a cada BI mapeado
                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                string qry = "qryGetQueriesGovernance.txt";
                DataTable dt = Odbc.dtm.ExecuteQuery(qry);
                //Odbc.dtm.Disconect();

                // Percorre por todas a queries.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Atribui os valores pertencentes a queries.
                    QueryId = int.Parse((string)dt.Rows[i]["ID"]);
                    GroupData = (string)dt.Rows[i]["GRUPO_DADOS"];
                    Script = (string)dt.Rows[i]["SCRIPT"];

                    // Saída no console sobre a execução dos trabalhos
                    Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Monitoramento de atualização de dados de {GroupData} iniciado.");

                    // Executa a querie de governaça no Datalake e trás se na coluna "ALERTA" de cada query existe algum valor = 1.
                    Odbc.Connect("ArgosAutomation", "DSN=Databricks");
                    qry = "qryGetAlert.txt";
                    Odbc.dtm.CleanParamters(qry);
                    Odbc.dtm.ParamByName(qry, ":SCRIPT", Script);
                    DataTable dts = Odbc.dtm.ExecuteQuery(qry);
                    int outdated = Tools.HasTrueValueInColumn(dts, "ALERTA");
                    //outdated = 0;

                    // Obtém informações dos grupos na qual a query em questão está relacionada.
                    Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                    qry = "qryGetGroupQueriesGovernance.txt";
                    Odbc.dtm.CleanParamters(qry);
                    Odbc.dtm.ParamByName(qry, ":ID_QUERY", QueryId.ToString());
                    DataTable dtx = Odbc.dtm.ExecuteQuery(qry);

                    // Atribui informações dos grupos.
                    for (int j = 0; j < dtx.Rows.Count; j++)
                    {
                        ChatIdGroup.Add(long.Parse((string)dtx.Rows[j]["CHAT_ID_GROUP"]));
                        GroupName.Add((string)dtx.Rows[j]["NOME_GRUPO"]);
                    }

                    // Se o valor for = 1 o grupo de dados dados da query em questão está desatualizado.
                    if (outdated == 1)
                    {
                        // Faz um alerta no grupo da governça sobre ao painel em questão com dados desatualizados.
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Dados de {GroupData} foram encontrados desatualizados.");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;

                        // Busca pelos paineis que estão relacionados a query em questão.
                        Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                        qry = "qryGetReportsByQuery.txt";
                        Odbc.dtm.CleanParamters(qry);
                        Odbc.dtm.ParamByName(qry, ":ID_QUERY", QueryId.ToString());
                        DataTable dtc = Odbc.dtm.ExecuteQuery(qry);
                        int enable = Tools.HasTrueValueInColumn(dtc, "ATIVO");

                        //
                        if (enable == 1)
                        {
                            await Utilities.botClient.SendTextMessageAsync(
                                chatId: /*5495003005*/ -975484125,
                                text: @$"@labtorre e @TorreSul

🤖: Pessoal, os dados de *{GroupData}* estão desatualizados ⚠️.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);
                        }

                        // Percorre por todos paineis encontrados.
                        for (int j = 0; j < dtc.Rows.Count; j++)
                        {
                            ReportId = int.Parse((string)dtc.Rows[j]["ID_PAINEL"]);
                            Enable = int.Parse((string)dtc.Rows[j]["ATIVO"]);
                            ReportName.Add((string)dtc.Rows[j]["NOME_PAINEL"]);

                            // Verifica se ele já está desativado ou não.
                            if (Enable == 1)
                            {
                                // Se estiver ativo, atualiza o valor do painel em questão na coluna "ATIVO".
                                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                                qry = "qryUpdateEnable.txt";
                                Odbc.dtm.CleanParamters(qry);
                                Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                                Odbc.dtm.ParamByName(qry, ":VALUE", "0");
                                Odbc.dtm.ExecuteNonQuery(qry);
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Painel de {ReportName.Last()} foi desativado.");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Gray;
                                //Odbc.dtm.Disconect();

                            }
                        }

                        // Percorre os grupos em que a query está relacionada.
                        for (int k = 0; k < dtx.Rows.Count; k++)
                        {
                            if (Enable == 1)
                            {
                                // Faz um alerta nos grupos em que a query está relacionada.
                                await Utilities.botClient.SendTextMessageAsync(
                                chatId: ChatIdGroup[k],
                                text: @$"🤖: Pessoal, acabei de executar a governaça dos dados de {GroupData} e encontrei inconsistências 😢 segue lista dos BI's que acabarem de ser desativados devido a desatualização de dados: 

*{string.Join("\n", ReportName.ToArray())}*",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);
                            }
                        }
                    }
                    else if (outdated == 0)
                    {
                        // Faz um alerta no grupo da governça sobre ao painel em questão com dados desatualizados.
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Dados de {GroupData} foram reestabelecidos.");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;

                        // Busca pelos paineis que estão relacionados a query em questão.
                        Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                        qry = "qryGetReportsByQuery.txt";
                        Odbc.dtm.CleanParamters(qry);
                        Odbc.dtm.ParamByName(qry, ":ID_QUERY", QueryId.ToString());
                        DataTable dtc = Odbc.dtm.ExecuteQuery(qry);
                        int enable = Tools.HasTrueValueInColumn(dtc, "ATIVO");

                        //
                        if (enable == 0)
                        {
                            await Utilities.botClient.SendTextMessageAsync(
                                chatId: /*5495003005*/ -975484125,
                                text: @$"@labtorre e @TorreSul

🤖: Os dados de {GroupData} estão atualizados ✅ Execelente trabalho time.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);
                        }

                        // Percorre por todos paineis encontrados.
                        for (int j = 0; j < dtc.Rows.Count; j++)
                        {
                            ReportId = int.Parse((string)dtc.Rows[j]["ID_PAINEL"]);
                            Enable = int.Parse((string)dtc.Rows[j]["ATIVO"]);
                            ReportName.Add((string)dtc.Rows[j]["NOME_PAINEL"]);

                            // Verifica se ele já está desativado ou não.
                            if (Enable == 0)
                            {
                                // Se estiver ativo, atualiza o valor do painel em questão na coluna "ATIVO".
                                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                                qry = "qryUpdateEnable.txt";
                                Odbc.dtm.CleanParamters(qry);
                                Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                                Odbc.dtm.ParamByName(qry, ":VALUE", "1");
                                Odbc.dtm.ExecuteNonQuery(qry);
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Painel de {ReportName.Last()} foi ativado.");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.Gray;
                                //Odbc.dtm.Disconect();

                            }
                        }




                        // Percorre os grupos em que a query está relacionada.
                        for (int k = 0; k < dtx.Rows.Count; k++)
                        {
                            if (Enable == 0)
                            {
                                // Faz um alerta nos grupos em que a query está relacionada.
                                await Utilities.botClient.SendTextMessageAsync(
                                chatId: ChatIdGroup[k],
                                text: @$"🤖: Os dados de {GroupData} foram reestabelecidos ✅ segue lista dos BI's que foram ativados e estão disponiveis novamente: 

*{string.Join("\n", ReportName.ToArray())}*",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);
                            }
                        }
                    }

                    // Zera todas as listas de informações para uma nova rotina de governança.
                    ChatIdGroup.Clear();
                    GroupName.Clear();
                    ReportName.Clear();
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erros genéricos.
                await Utilities.botClient.SendTextMessageAsync(
                chatId: 5495003005,
                text: @$"*Manipulador de erros acionado* 🪲 - {DateTime.Now}

*Classe:* GovernanceJob.cs 🔎

Erro no trabalho de *{JobName}* realizado as *{Time}* devido a {ex.Message}",
                parseMode: ParseMode.Markdown,
                cancellationToken: Utilities.cts);
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(@$"Manipulador de erros acionado - {DateTime.Now}

Classe: GovernanceJob.cs

Erro no trabalho de {JobName} realizado as {Time} devido a {ex.Message}

{ex}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
                Program.sound = new SoundPlayer();
                Program.sound.SoundLocation = @$"{Tools.GetDirectoryProject()}\Resources\AlertError.wav";
                Program.sound.Play();
                Program.sound.Dispose();
            }

            Console.WriteLine(" ");

        }
    }
}

