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
    // Classe que representa o comando "Argos, como está o pedido [NUMERO DO PEDIDO]."

    public class StatusPedido : IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public StatusPedido()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB); //conexao com banco
            string qryComandos = "qryComandos.txt"; // verifica ha o comando no banco de dados
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos); // verifica se ha a querry
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argoscomoestopedido"); //
            DataTable dtComandos = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos); //
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
            // Obtém o pedido e o cliente da solicitação.

            var pedido = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 8);
            var cliente = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("*") + 11);
            pedido = pedido.Substring(0, pedido.IndexOf(" do"));

            // Executa no datalake a query referente ao comando em questão.

            try
            {
                await Send.Text(Updates.chatId, $"Claro {Updates.firstName}! Buscando pedido nº {pedido.ToUpper()} 🔎.");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks); 
                string qryStatusPedido = "qryStatusPedido.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryStatusPedido);
                BancoDeDadosODBC.dtm.ParamByName(qryStatusPedido, ":PEDIDO", pedido);
                BancoDeDadosODBC.dtm.ParamByName(qryStatusPedido, ":CLIENTE", cliente);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryStatusPedido);
                BancoDeDadosODBC.dtm.Desconectar();
                DataRow row = dtResult.Rows[0];


                // Faz o envio no Telegram.

                await Send.Text(Updates.chatId, @$"

Encontrei o pedido {pedido} 📦 do cliente {row["NOME"]} com o seguinte status: 

*Site: *{row["UNIDADE"]}
*Cliente: *{row["NOME"]}
*Pedido: *{row["PEDIDO"]}
*Total de produtos: *{row["TOTAL_PRODUTOS"]}
*Quantidade: *{row["QUANTIDADE"]}
*Tempo em aberto (Interface x Conferido): *{row["TEMPO"]}
*Status: *{row["STATUS"]}
*Data integração: *{row["DATA_INTEGRACAO"]}
*Data reserva: *{row["DATA_RESERVA"]}
*Final separação: *{row["FIM_SEPARACAO"]}
*Final da conferência: *{row["FIM_CONFERENCIA"]}
*Data expedição: *{row["DATA_EXPEDICAO"]}
*Transportador: *{row["TRANSPORTADOR"]}
*CESV: *{row["CESV"]}
*Source: *{row["source"]}.");

            }

            catch (IndexOutOfRangeException ex)
            {
                // Em caso da solicitação da unidade não retornar nada.

                await Send.Text(Updates.chatId, @$"

Não encontrei nada no sistema 😢

*Erro de consulta*: Índice fora do intervalo ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Você pode ter passado o nº do pedido *{pedido}* ou o ID do cliente {cliente} errado, por favor verifique e tente novamente."
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

                Thread.Sleep(3000);
                await Send.Text(Updates.chatId, @$"
Ocorreu um erro ao consultar o pedido *{pedido}* ❌ 

*Erro:* {ex.Message}

Por favor entre em contato com a Torre de Controle.");

                return;
            }
        }
    }
}