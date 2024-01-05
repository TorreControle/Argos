using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using MySqlX.XDevAPI.Relational;
using System.Data;

namespace ArgosOnDemand.Commands
{
    public class GeradorQry : IResponse
    {
        // será acionado após passar pelo chat gpt
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".

        public GeradorQry() 
        {
            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosgerar");
            DataTable dtComandos = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos);
            BancoDeDadosODBC.dtm.Desconectar();
            DataRow row = dtComandos.Rows[0];


            // Atribuindo valores as propriedades.

            id = int.Parse(row["id"].ToString());
            comando = row["comando"].ToString();
            tipoComando = row["tipo_comando"].ToString();
            tipoSaida = row["tipo_saida"].ToString();
            query = row["query"].ToString();
            saida = row["saida"].ToString();
        }

        public async Task TriggerAsync()
        {
            int hash = Updates.messageText.IndexOf(" #");
            var promptUser = Updates.messageText.Substring(hash + 1);
            await Send.Text(Updates.chatId, "Positivo!! Gerando query.", replyToMessageId: Updates.messageId);
            //string info = Updates.messageText[(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true, interrogation: false).IndexOf("doca ") + 42)..];

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
            string qryGeradorQry = "qryGeradorQry.txt";
            DataTable dtGeradorQry = BancoDeDadosODBC.dtm.ExecuteQuery(qryGeradorQry);
            BancoDeDadosODBC.dtm.Desconectar();


            // Variável para armazenar os nomes das colunas
            string colunaNomes = "";


            //
            var realData = Tools.WriteData(dtGeradorQry);

            // Percorra as colunas do DataTable e concatene os nomes
            foreach (DataColumn coluna in dtGeradorQry.Columns)
            {
                colunaNomes += coluna.ColumnName + ", ";
            }

            // Remova a vírgula extra no final, se houver
            if (!string.IsNullOrEmpty(colunaNomes))
            {
                colunaNomes = colunaNomes.Substring(0, colunaNomes.Length - 2);
            }


            var resposta = await Tools.ChatGPT(@$"

{promptUser}.

Colunas para referência:

{colunaNomes}.

Siga o padrão desses da reais e brutos para otimizar a consulta:

{realData}

O nome da tabela é ouro_operacao_cd.operacao_cd_fato_pedidos

Sem explicação e descrição, quero literalmente apenas o script");

            await Send.Text(Updates.chatId, @$"Consulta SQL gerada: 

{resposta}");


            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
            DataTable dt = BancoDeDadosODBC.dtm.ExecuteString(resposta);
            BancoDeDadosODBC.dtm.Desconectar();


            await Send.Text(Updates.chatId, @$"Resultado:

{dt.Rows[0][dt.Columns[0]]}");

        }

    }
}
