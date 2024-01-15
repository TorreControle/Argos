﻿using ArgosAutomation.Databases;
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
        /// Id do painel em questão.
        /// </summary>
        public int? ReportId { get; set; }
        /// <summary>
        /// Nome do painel em questão
        /// </summary>
        public string? ReportName { get; set; }
        /// <summary>
        /// Chat Id do grupo do painel em questão.
        /// </summary>
        public List<long?> ChatIdGroup = new();
        /// <summary>
        /// Grupo de dados em que o BI está alocado.
        /// </summary>
        public string? GroupData { get; set; }
        /// <summary>
        /// Script da query de governança.
        /// </summary>
        public string? Script { get; set; }
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





                    /* Atribui os valores pertencentes a queries.
                    ReportId = int.Parse((string)dt.Rows[i]["PAINEL_ID"]);
                    ReportName = (string)dt.Rows[i]["NOME_PAINEL"];
                    GroupData = (string)dt.Rows[i]["GRUPO_DADOS"];
                    Script = (string)dt.Rows[i]["SCRIPT"];

                    // Saída no console sobre a execução dos trabalhos
                    Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Monitoramento de atualização de dados de {GroupData} iniciado, dados do painel de {ReportName} sendo monitorados.");

                    // Executa a querie de governaça no Datalake e trás se na coluna "ALERTA" de cada query existe algum valor = 1.
                    Odbc.Connect("ArgosAutomation", "DSN=Databricks");
                    qry = "qryGetAlert.txt";
                    Odbc.dtm.CleanParamters(qry);
                    Odbc.dtm.ParamByName(qry, ":SCRIPT", Script);
                    DataTable dts = Odbc.dtm.ExecuteQuery(qry);
                    int outdated = Tools.HasTrueValueInColumn(dts, "ALERTA");

                    //
                    Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                    qry = "qryGetGroupQueriesGovernance.txt";
                    Odbc.dtm.CleanParamters(qry);
                    Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                    DataTable dtx = Odbc.dtm.ExecuteQuery(qry);

                    for (int j = 0; j < dtx.Rows.Count; j++)
                    {
                        ChatIdGroup.Add(long.Parse((string)dtx.Rows[j]["CHAT_ID_GROUP"]));
                    }


                    //outdated = 0;
                    // Se houver algum valor = 1 o painel em questão é desativado.
                    if (outdated == 1)
                    {
                        // Busca pelo painel em questão.
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Dados de {GroupData} foram encontrados desatualizados.");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                        qry = "qryGetReport.txt";
                        Odbc.dtm.CleanParamters(qry);
                        Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                        DataTable dtc = Odbc.dtm.ExecuteQuery(qry);
                        //Odbc.dtm.Disconect();


                        // Verifica se ele já está desativado ou não.
                        if (dtc.Rows[0]["ATIVO"].ToString() != "0")
                        {
                            // Faz um alerta no grupo da governça sobre ao painel em questão com dados desatualizados.
                            await Utilities.botClient.SendTextMessageAsync(
                                chatId: -975484125,
                                text: @$"🤖: Os dados de *{GroupData}* estão desatualizados, o painel de *{ReportName}* foi desativado ⚠️.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);

                            // Atualiza o valor do painel em questão na coluna "ATIVO".
                            Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                            qry = "qryUpdateEnable.txt";
                            Odbc.dtm.CleanParamters(qry);
                            Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                            Odbc.dtm.ParamByName(qry, ":VALUE", "0");
                            Odbc.dtm.ExecuteNonQuery(qry);
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Painel de {ReportName} foi desativado.");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            //Odbc.dtm.Disconect();

                            for (int j = 0; j < dtx.Rows.Count; j++)
                            {
                                // Faz um alerta no grupo pertencente ao painel em questão.
                                await Utilities.botClient.SendTextMessageAsync(
                                    chatId: ChatIdGroup[j],
                                    text: @$"🤖: Pessoal, alguns *{GroupData}* foram verificados e encontrados desatualizados, o painel de *{ReportName}* foi desativado ⚠️.",
                                    parseMode: ParseMode.Markdown,
                                    cancellationToken: Utilities.cts);
                            }
                        }
                    }
                    else if (outdated == 0)
                    {
                        // Se não houver valor = 1 na coluna "ALERTA" da query de governança do painel em questão, significa que o painel pode ser ativado novamente. 
                        // Busca pelo painel em questão.
                        Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Dados do painel de {ReportName} atualizados.");
                        Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                        qry = "qryGetReport.txt";
                        Odbc.dtm.CleanParamters(qry);
                        Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                        DataTable dtc = Odbc.dtm.ExecuteQuery(qry);
                        //Odbc.dtm.Disconect();

                        // Verifica se ele já está desativado ou não.
                        if (dtc.Rows[0]["ATIVO"].ToString() != "1")
                        {
                            // Atualiza o valor do painel em questão na coluna "ATIVO".
                            Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                            qry = "qryUpdateEnable.txt";
                            Odbc.dtm.CleanParamters(qry);
                            Odbc.dtm.ParamByName(qry, ":ID", ReportId.ToString());
                            Odbc.dtm.ParamByName(qry, ":VALUE", "1");
                            Odbc.dtm.ExecuteNonQuery(qry);
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] GovernanceJob: Painel de {ReportName} foi ativado novamente.");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            //Odbc.dtm.Disconect();

                            for (int j = 0; j < dtx.Rows.Count; j++)
                            {
                                // Faz um alerta no grupo pertencente ao painel em questão.
                                await Utilities.botClient.SendTextMessageAsync(
                                chatId: ChatIdGroup[j],
                                text: @$"🤖: A atualização dos dados de *{GroupData}* foi restaurada, o painel de *{ReportName}* foi ativado novamente ✅.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: Utilities.cts);

                            }
                        }
                    }*/

                    ChatIdGroup.Clear();
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

