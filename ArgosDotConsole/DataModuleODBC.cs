/*
 * Created by SharpDevelop.
 * User: cguerra.adm
 * Date: 10/12/2015
 * Time: 16:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Reflection;

namespace ArgosDot
{
    /// <summary>
    /// Classe para simular um DataModule do Delphi. Criar uma pasta no projeto e criar TXT abaixo dela.
    /// Criar assim: public static DataModule dtm = new DataModule("ControleRelatorios.DataModule") onde ControleRelatorios é o projeto e DataModule é a pasta
    /// </summary>
    public class DataModuleODBC
    {
        private readonly string resourceName = string.Empty;
        private OdbcConnection OdbcConexao;
        private Assembly assembly;
        private OdbcTransaction transaction = null;

        public bool Conectado = false;

        public class queries
        {
            public string nome;
            public string gatilho;
            public string query;
        }

        // Faz uma cópia das queries 
        public List<queries> lqueries = new List<queries>();

        // constructor passando o local do data module
        // Exemplo: "nomedoprojeto.DataModule" ("DataModule" deve ser uma pasta dentro do projeto)
        public DataModuleODBC(string RName)
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
                if (lqueries[f].nome == resourceName + "." + query) //procura se a query existe
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
            OdbcConexao = new OdbcConnection(strconect);
            OdbcConexao.Open();
            Conectado = true;
        }

        public void Desconectar()
        {
            if (OdbcConexao.State == ConnectionState.Open)
            {
                OdbcConexao.Close();
                Conectado = false;
            }
        }

        public DataTable ExecuteQuery(string nquery)
        {
            if (Conectado)
            {
                DataTable tabelaResultado = new DataTable();
                DataColumn dataColumn = new DataColumn();
                DataRow dataRow;
                DataTable schemaTabela;
                OdbcCommand dbCommand;
                OdbcDataReader dbReader;
                bool achou = false;

                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].nome == resourceName + "." + nquery)
                    {
                        // Cria o comando SQL e da um set da query em questão.

                        dbCommand = OdbcConexao.CreateCommand();
                        dbCommand.CommandText = lqueries[f].query;


                        // Executa a query

                        dbReader = dbCommand.ExecuteReader();


                        // Obtemos o esquema da tabela via DataReader

                        schemaTabela = dbReader.GetSchemaTable();


                        // definimos um novo objeto DataTable


                        // percorremos cada linha da tabela e criamos uma nova coluna

                        foreach (DataRow row in schemaTabela.Rows)
                        {
                            dataColumn.ColumnName = row["ColumnName"].ToString();
                            tabelaResultado.Columns.Add(dataColumn.ColumnName);
                        }


                        // lemos o datareader e preenchemos o DataTable

                        while (dbReader.Read())
                        {
                            dataRow = tabelaResultado.NewRow();


                            // percorre o DataTable e atribui os dados 

                            for (int i = 0; i <= tabelaResultado.Columns.Count - 1; i++)
                            {
                                dataRow[i] = dbReader[i];
                            }


                            // inclui a linha no DataTable

                            achou = true;
                            tabelaResultado.Rows.Add(dataRow);

                        }

                        break;
                    }
                }

                //

                if (achou)
                {
                    return tabelaResultado;
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

        public void Execute(string nquery)
        {
            if (Conectado)
            {
                OdbcCommand dbCommand;

                for (ushort f = 0; f <= lqueries.Count - 1; f++)
                {
                    if (lqueries[f].nome == resourceName + "." + nquery)
                    {
                        // Cria o comando SQL e da um set da query em questão.

                        dbCommand = OdbcConexao.CreateCommand();
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

