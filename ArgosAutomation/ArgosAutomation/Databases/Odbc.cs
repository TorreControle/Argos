namespace ArgosAutomation.Databases
{
    public class Odbc
    {
        public static DataModuleOdbc dtm;

        public Odbc(string projectName, string con)
        {
            dtm = new DataModuleOdbc(projectName + ".DataModule");
            dtm.Connect(con);
        }

        public static void Connect(string projectName, string con)
        {
            new Odbc(projectName, con);
        }
    }
}
