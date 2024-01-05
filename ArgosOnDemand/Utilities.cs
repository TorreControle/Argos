/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ArgosOnDemand
{
    public static class Utilities
    {
        // Objeto para manipulação de datas.

        public static DateTime DateTime = DateTime.Now;
        public static DateTime BusinessDay = DateTime.Now;



        // Token de cancelamento

        public static CancellationTokenSource CancellationToken = new CancellationTokenSource();


        //

        public static class Telegram
        {
            // Token do Bot

            public static string Token = "6061474214:AAFAcZmUaU07EFJYOUOn7NPLFtUJhoMCTHg";


            // Recebedor de atualizações

            public static ReceiverOptions Historic = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                }
            };

        }


        // Conexões com banco 

        public static class Conections
        {
            public static class Servers
            {
                public static string Sistemas = "server=176.16.7.25;user=sistemas;database=argos;port=3306;password=nextel94";
                public static string SaraSTS = "Provider=SQLOLEDB ;Password=operador;User ID=operador;Data Source=176.16.36.122;Persist Security Info=True";
                public static string SaraCPS = "Provider=SQLOLEDB ;Password=operador;User ID=operador;Data Source=176.16.12.27;Persist Security Info=True";
                public static string SaraSP = "Provider=SQLOLEDB ;Password=operador;User ID=operador;Data Source=176.16.8.130;Persist Security Info=True";
                public static string SaraPSBAR = "Provider=SQLOLEDB ;Password=operador;User ID=operador;Data Source=176.16.4.215;Persist Security Info=True";
                public static string ReportsWMS = "Provider=MSDAORA ;Password=reports;User ID=reports;Data Source=WMS;Persist Security Info=True";
                public static string MTLG01P = "Provider=MSDAORA ;Password=iismultilog;User ID = aspnet; Data Source = MTLG01P; Persist Security Info=True";
            }

            public static class DataSources
            {
                public static string Databricks = "DSN=Databricks";
                public static string MariaDB = "DSN=SRVAZ31-ARGOS";
            }

        }


        // Diretórios

        public static class Directory
        {
            public static class Folders
            {
                public static string Charts = @$"{Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))}\Files\Charts";
                public static string QrCode = @$"{Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())))}\Files\QrCodes\";

            }
        }
    }
}
