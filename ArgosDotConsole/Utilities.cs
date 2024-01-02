namespace ArgosDot
{
    public class Utilities
    {

        // Strings de conexões do Data Sources ODBC.
        public class DSN
        {
            public static string Databricks = "DSN=Databricks";
        }


        // Caminhos de pastas em geral.
        public class Folders
        {
            public static string Log = $@"{Tools.GetDirectoryProject()}\Resources\log\";
            public static string Project = $"{Tools.GetDirectoryProject()}";
        }


        // Caminhos de diretórios em geral.
        public class Directory
        {
            public class Grammar
            {
                public static string ActivationWord = $@"{Tools.GetDirectoryProject()}\resources\grammars\ActivationWord.xml";
            }

            public class Audio
            {
                public static string Output = $@"{Tools.GetDirectoryProject()}\resources\audio\Output.wav";
                public static string ConfirmationSign = $@"{Tools.GetDirectoryProject()}\resources\audio\ConfirmationSign.wav";
                public static string NoData = $@"{Tools.GetDirectoryProject()}\resources\audio\NoData.wav";

            }

        }

    }

}
