/*
 * Created by SharpDevelop.
 * User: cguerra.adm
 * Date: 10/12/2015
 * Time: 16:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace ArgosOnDemand.Database
{
    /// <summary>
    /// Classe para simular um DataModule do Delphi. Criar uma pasta no projeto e criar TXT abaixo dela.
    /// Criar assim: public static DataModule dtm = new DataModule("ControleRelatorios.DataModule") onde ControleRelatorios é o projeto e DataModule é a pasta
    /// </summary>
    public class DataModuleOleDb
    {
        private readonly string resourceName = string.Empty;
        private OleDbConnection OleDbConexao;
        private Assembly assembly;
        private OleDbTransaction transaction = null;

        public bool Conectado = false;

        public class queries
        {
            public string nome;
            public string query;
        }

        // Faz uma cópia das queries 
        public List<queries> lqueries = new List<queries>();





        // constructor passando o local do data module
        // Exemplo: "nomedoprojeto.DataModule" ("DataModule" deve ser uma pasta dentro do projeto)
        public DataModuleOleDb(string RName)
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
                    queries sq = new queries();
                    sq.nome = resNames[f];
                    sq.query = reader.ReadToEnd();
                    lqueries.Add(sq);
                    reader.Dispose();
                }
            }
        }

        // Recupera a query original
        public void Limpa_Parametros(string query)
        {
            bool achou = false;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].nome == resourceName + "." + query)
                {
                    Stream stream = assembly.GetManifestResourceStream(lqueries[f].nome);
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
        public void ParamByName(string query, string parametro, string valor)
        {
            bool achou = false;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].nome == resourceName + "." + query)
                {
                    string im = lqueries[f].query.Replace(parametro, valor);
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

        // Lê uma query como está para ser executada, originária da lista-cópia
        public string LeQueryExec(string query)
        {
            string r = string.Empty;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].nome == resourceName + "." + query)
                {
                    r = lqueries[f].query;
                    break;
                }
            }
            if (r == string.Empty)
            {
                throw new Exception("Query não existe no datamodule (" + resourceName + "." + query + ")");
            }
            return r;
        }

        // Lê uma query como está definida no datamodule
        public string LeQueryOriginal(string qry)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceName + "." + qry);
            if (stream == null)
            {
                    throw new Exception("Query não existe no datamodule (" + resourceName + "." + qry + ")");
            }
            else
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void Conectar(string strconect)
        {
            OleDbConexao = new OleDbConnection(strconect);
            OleDbConexao.Open();
            Conectado = true;
        }

        public void Desconectar()
        {
            if (OleDbConexao.State == ConnectionState.Open)
            {
                OleDbConexao.Close();
                Conectado = false;
            }
        }

        public DataTable Execute(string qry, string con)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter adapter = new OleDbDataAdapter(qry, con);
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        public DataTable ExecuteQuery(string nquery)
        {
            if (Conectado)
            {
                bool achou = false;

                DataSet ds = new();
                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].nome == resourceName + "." + nquery)
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(lqueries[f].query, OleDbConexao);
                        adapter.Fill(ds);
                        achou = true;
                        break;
                    }
                }

                if (achou)
                {


                    return ds.Tables[0];

                }
                else
                {
                    throw new Exception("Query não existe no datamodule (" + resourceName + "." + nquery + ")");
                }
            }
            else
            {

                throw new Exception("Banco de dados desconectado!");
            }
        }

        public void ExecuteNonQuery(string nquery)
        {
            if (Conectado)
            {
                bool achou = false;
                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].nome == resourceName + "." + nquery)
                    {
                        OleDbCommand cmd = new OleDbCommand();
                        cmd = OleDbConexao.CreateCommand();

                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = lqueries[f].query;
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        achou = true;
                        break;
                    }
                }
                if (!achou)
                {
                    throw new Exception("Query não existe no datamodule (" + resourceName + "." + nquery + ")");
                }
            }
            else
            {
                throw new Exception("Banco de dados desconectado!");
            }
        }

        public void BeginTransaction()
        {
            transaction = OleDbConexao.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
            transaction = null;
        }

        public void RollBack()
        {
            transaction.Rollback();
            transaction = null;
        }
        // Torna todos os parâmetros nulos. Ainda não tem uso
        public void NullParameters(string nquery)
        {
            bool achou = false;
            for (ushort f = 0; f <= lqueries.Count - 1; f++)
            {
                if (lqueries[f].nome == resourceName + "." + nquery)
                {
                    achou = true;
                    int i = lqueries[f].query.IndexOf(':');
                    while (i != -1)
                    {
                        int h = lqueries[f].query.IndexOf(',', i);
                        if (h == -1)
                        {
                            h = lqueries[f].query.IndexOf(' ', i);
                        }
                        if (h == -1)
                        {
                            h = lqueries[f].query.IndexOf('\r', i);
                        }
                        string p = lqueries[f].query.Substring(i, h - i);
                        lqueries[f].query = lqueries[f].query.Replace(p, "null");
                        i = lqueries[f].query.IndexOf(':');
                    }
                    break;
                }
            }
            if (!achou)
            {
                throw new Exception("Query não existe no datamodule (" + resourceName + "." + nquery + ")");
            }
        }
    }
}

