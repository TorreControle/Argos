using System.Data;
using System.Data.Odbc;
using System.Reflection;

namespace ArgosAutomation.Databases
{
    public class DataModuleOdbc
    {
        private readonly string? resourceName = string.Empty;
        public OdbcConnection? OdbcConection;
        private Assembly assembly;
        private OdbcTransaction? transaction = null;
        public bool Conected = false;
        public List<Queries> lqueries = new List<Queries>();

        //
        public class Queries
        {
            public string? name;
            public string? trigger;
            public string? query;
        }

        //
        public DataModuleOdbc(string RName)
        {
            resourceName = RName;
            assembly = Assembly.GetExecutingAssembly();
            string[] resNames = assembly.GetManifestResourceNames();
            // Começo do 1 e não do 0 por que o primeiro é um resource "genérico"
            for (ushort f = 1; f <= resNames.Length - 1; f++)
            {
                // Se for um resource pertencente ao data module
                if (resNames[f].IndexOf(RName) != -1)
                {
                    Stream stream = assembly.GetManifestResourceStream(resNames[f]);
                    StreamReader reader = new StreamReader(stream);
                    // Crio a lista-cópia de todas as queries
                    Queries sq = new Queries();
                    sq.name = resNames[f];
                    sq.query = reader.ReadToEnd();
                    lqueries.Add(sq);
                    reader.Dispose();
                }
            }
        }

        //
        public void Connect(string con)
        {
            OdbcConection = new OdbcConnection(con);
            OdbcConection.Open();
            Conected = true;
        }

        //
        public void Disconect()
        {
            if (OdbcConection.State == ConnectionState.Open)
            {
                OdbcConection.Close();
                Conected = false;
            }
        }

        // Recupera a query original
        public void CleanParamters(string query)
        {
            bool achou = false;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].name == resourceName + "." + query) //procura se a query existe
                {
                    Stream stream = assembly.GetManifestResourceStream(lqueries[f].name);
                    StreamReader reader = new StreamReader(stream);
                    lqueries[f].query = reader.ReadToEnd();
                    achou = true;
                    break;
                }
            }
            if (!achou)
            {
                throw new Exception("Query não existe no datamodule (" + resourceName + "." + query + ")");
            }
        }

        // Substitui os parâmetros da query pelos valores reais
        public void ParamByName(string query, string param, string value)
        {
            bool achou = false;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].name == resourceName + "." + query)
                {
                    string im = lqueries[f].query.Replace(param, value);
                    lqueries[f].query = im;
                    achou = true;
                    break;
                }
            }
            if (!achou)
            {
                throw new Exception("Query não existe no datamodule (" + resourceName + "." + query + ")");
            }
        }

        //
        public DataTable ExecuteQuery(string query)
        {
            if (Conected)
            {
                DataTable table = new();
                DataColumn dataColumn = new();
                DataRow dataRow;
                DataTable schemaTable;
                OdbcCommand dbCommand;
                OdbcDataReader dbReader;
                bool find = false;

                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].name == resourceName + "." + query)
                    {
                        find = true;
                        // Cria o comando SQL e da um set da query em questão.

                        dbCommand = OdbcConection.CreateCommand();
                        dbCommand.CommandText = lqueries[f].query;


                        // Executa a query

                        dbReader = dbCommand.ExecuteReader();


                        // Obtemos o esquema da tabela via DataReader

                        schemaTable = dbReader.GetSchemaTable();


                        // definimos um novo objeto DataTable


                        // percorremos cada linha da tabela e criamos uma nova coluna

                        foreach (DataRow row in schemaTable.Rows)
                        {
                            dataColumn.ColumnName = row["ColumnName"].ToString();
                            table.Columns.Add(dataColumn.ColumnName);
                        }


                        // lemos o datareader e preenchemos o DataTable

                        while (dbReader.Read())
                        {
                            dataRow = table.NewRow();


                            // percorre o DataTable e atribui os dados 

                            for (int i = 0; i <= table.Columns.Count - 1; i++)
                            {
                                dataRow[i] = dbReader[i];
                            }


                            // inclui a linha no DataTable


                            table.Rows.Add(dataRow);

                        }

                        break;
                    }
                }

                //

                if (find)
                {
                    return table;
                }
                else
                {
                    throw new Exception("Query não existe no datamodule (" + resourceName + "." + query + ")");
                }


            }
            else
            {
                throw new Exception("Banco de dados desconectado!");
            }
        }

        //
        public void ExecuteNonQuery(string nquery)
        {
            if (Conected)
            {
                OdbcCommand dbCommand;

                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].name == resourceName + "." + nquery)
                    {
                        // Cria o comando SQL e da um set da query em questão.

                        dbCommand = OdbcConection.CreateCommand();
                        dbCommand.CommandText = lqueries[f].query;


                        // Executa a query

                        dbCommand.ExecuteReader();

                    }
                }
            }
            else
            {
                throw new Exception("Banco de dados desconectado!");
            }

        }

    }
}
