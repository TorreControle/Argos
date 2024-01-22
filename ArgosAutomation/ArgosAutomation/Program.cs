using ArgosAutomation.Databases;
using ArgosAutomation.Jobs;
using Quartz;
using Quartz.Impl;
using System.Data;
using System.Media;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation
{
    public class Program
    {
        // 
        private static StdSchedulerFactory factory = new();
        private static IJobDetail? job;
        private static ITrigger? trigger;
        public static SoundPlayer? sound;

        //
        public static async Task Main(string[] args)
        {
            // Estilizando cabeçalho.
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"                                      Argos Automation\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"                       Módulo de Automação de processos robóticos (RPA).");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($@"
 Ambiente: {Environment.GetEnvironmentVariable("ENVIRONMENT_DESCRIPTION", EnvironmentVariableTarget.User)}
 Domínio: {Environment.UserDomainName}
 Máquina: {Environment.MachineName}
 Usuário: {Environment.UserName}
 Diretório do projeto: {Environment.CurrentDirectory}
 Caminho do executável: {System.Reflection.Assembly.GetExecutingAssembly().Location}
 CLR Version: {Environment.Version}
 Versão do sistema: {Environment.OSVersion}
");
            Console.ForegroundColor = ConsoleColor.Gray;


            // Instância do agendador e inicia o monitoramento.
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            //
            try
            {
                //
                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                string qry = "qryGetJobs.txt";
                DataTable dt = Odbc.dtm.ExecuteQuery(qry);
                //Odbc.dtm.Disconect();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TIPO"].ToString() == "DIVULGACAO")
                    {
                        //
                        job = JobBuilder.Create<ReportJob>()
                            .WithIdentity($"{dt.Rows[i]["NOME"]}", $"{dt.Rows[i]["TIPO"]}")
                            .UsingJobData("ID", int.Parse((string)dt.Rows[i]["ID"]))
                            .UsingJobData("Nome", (string)dt.Rows[i]["NOME"])
                            .UsingJobData("Hora", (string)dt.Rows[i]["HORA"])
                            .UsingJobData("Expressão Cron", (string)dt.Rows[i]["EXPRESSAO_CRON"])
                            .Build();

                        // Acione a execução do trabalho agora e repita a cada 1 segundo.
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"Gatilho da {dt.Rows[i]["Nome"]}", $"{dt.Rows[i]["TIPO"]}")
                            .WithDescription("Teste descrição da trigger.")
                            .WithPriority(10)
                            .ForJob(job)
                            .WithCronSchedule((string)dt.Rows[i]["EXPRESSAO_CRON"])
                            .Build();

                        var poiu = dt.Rows[i]["TIPO"].ToString();

                    }
                    else if (dt.Rows[i]["TIPO"].ToString() == "GOVERNANCA")
                    {
                        //
                        job = JobBuilder.Create<GovernanceJob>()
                            .WithIdentity($"{dt.Rows[i]["NOME"]}", $"{dt.Rows[i]["TIPO"]}")
                            .UsingJobData("ID", int.Parse((string)dt.Rows[i]["ID"]))
                            .UsingJobData("Nome", (string)dt.Rows[i]["NOME"])
                            .UsingJobData("Hora", (string)dt.Rows[i]["HORA"])
                            .UsingJobData("Expressão Cron", (string)dt.Rows[i]["EXPRESSAO_CRON"])
                            .Build();

                        // Acione a execução do trabalho agora e repita a cada 1 segundo.
                        trigger = TriggerBuilder.Create()
                            .WithIdentity($"Gatilho da {dt.Rows[i]["Nome"]}", $"{dt.Rows[i]["TIPO"]}")
                            .WithDescription("Teste descrição da trigger.")
                            .WithPriority(1)
                            .ForJob(job)
                            .WithCronSchedule((string)dt.Rows[i]["EXPRESSAO_CRON"])
                            .Build();
                    }

                    // Diga ao Quartz para agendar o trabalho usando nosso gatilho.
                    await scheduler.ScheduleJob(job, trigger);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Program: {dt.Rows[i]["Nome"]} carregada!");
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
                Console.WriteLine(@$" 
");

            }
            catch (Exception ex)
            {
                // Tratamento de erros genéricos.
                await Utilities.botClient.SendTextMessageAsync(
                chatId: 5495003005,
                text: @$"*Manipulador de erros acionado* 🪲 - *{DateTime.Now}*

*Classe:* Program.cs 🖥

Erro no carregamento dos trabalhos devido a {ex.Message}",
                parseMode: ParseMode.Markdown,
                cancellationToken: Utilities.cts);

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(@$"Manipulador de erros acionado - {DateTime.Now}

Classe: Program.cs

Erro no carregamento dos trabalhos devido a {ex.Message}

{ex}");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;

                sound = new SoundPlayer();
                sound.SoundLocation = @$"{Tools.GetDirectoryProject()}\Resources\AlertError.wav";
                sound.Play();
                sound.Dispose();
            }

            // Inicia a instância do bot no servidor do Telegram.
            Utilities.botClient.StartReceiving(
                UpdateHandler.Init,
                ErrorHandler.Init,
                Utilities.receiver,
                cancellationToken: Utilities.cts);

            await Utilities.botClient.SendTextMessageAsync(
                                        chatId: 5495003005,
                                        text: @$"🤖: *Acabei de ser ligado* 💡

👤 Usuário: {Environment.UserName}
💻 Nome da máquina: {Dns.GetHostName()}
💻 Ambiente: {Environment.GetEnvironmentVariable("ENVIRONMENT_DESCRIPTION", EnvironmentVariableTarget.User)}
🌐 IP: {Dns.GetHostByName(Dns.GetHostName()).AddressList[1]} - {Environment.UserDomainName}
🕒 Data e hora: {DateTime.Now}",
                                        parseMode: ParseMode.Markdown,
                                        cancellationToken: Utilities.cts);

            //
            Console.ReadKey();

        }
    }
}
