/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using System.Data;

namespace ArgosOnDemand.Commands
{
    // Classe que representa o comando "Argos, pedidos em doca [UNIDADE]."

    public class PedidosEmDocaUnidade : IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public PedidosEmDocaUnidade()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argospedidosemdocano");
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


        // Método de execução do comando.

        public async Task TriggerAsync()
        {
            // Obtém a unidade solicitada

            string unidade = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true, interrogation: false).IndexOf("doca ") + 28);


            // Executa no datalake a query referente ao comando em questão.

            try
            {
                await Send.Text(Updates.chatId, $"Positivo {Updates.firstName}! Análise sendo gerada 🔄");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryComandos = "qryPedidosDocaUnidade.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
                BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":UNIDADE", unidade.ToUpper());
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos);
                BancoDeDadosODBC.dtm.Desconectar();


                // Faz o envio no Telegram.

                await Send.Text(Updates.chatId, $@"No momento o *{unidade}* está com {dtResult.Rows[0]["pedidos_doca"]} pedidos em doca 📦 com o tempo *médio* em doca de {dtResult.Rows[0]["media_tempo"]}h ⏱.");

            }

            catch (IndexOutOfRangeException ex)
            {
                // Em caso da solicitação da unidade não retornar nada.

                await Send.Text(Updates.chatId, @$"

Não encontrei nada no sistema 😢

*Erro de consulta*: Índice fora do intervalo ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Talvez a unidade passada *{unidade}* não corresponda com o que está no meu sistema, por favor verifique e tente novamente."
);
                return;
            }

            catch (InvalidOperationException ex)
            {
                // Em caso da solicitação da unidade retornar erro .

                await Send.Text(Updates.chatId, @$"

*Erro sistemico*: Operação inválida ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Por favor entre em contato com a Torre de Controle.");

                return;
            }

            catch (Exception ex)
            {
                // Em caso de algum erro entre a conexão e o envio da mensagem.

                await Send.Text(Updates.chatId, @$"
Ocorreu um erro ao consultar os dados ❌ 

*Erro:* {ex.Message}

Por favor entre em contato com a Torre de Controle.");

                return;

            }
        }
    }
}
