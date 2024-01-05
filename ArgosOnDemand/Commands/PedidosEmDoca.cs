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
using Graphics = ArgosOnDemand.Skill.Graphics;

namespace ArgosOnDemand.Commands
{
    // Classe que representa o comando "Argos, pedidos em doca por unidade."

    public class PedidosEmDoca : IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public PedidosEmDoca()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argospedidosemdocaporunidade");
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
            // Executa no datalake a query referente ao comando em questão.

            try
            {
                await Send.Text(Updates.chatId, $"Positivo {Updates.firstName}! gerando análise 🔄");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryPedidosDoca = "qryPedidosDoca.txt";
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryPedidosDoca);
                BancoDeDadosODBC.dtm.Desconectar();


                // Cria o gráfico.

                Graphics.Bar(dtResult, "pedidos_doca", "unidade", "Quantidade", "Unidade", @$"Pedidos em doca por unidade - {DateTime.Now}", "PedidosDoca");


                // Faz o envio no Telegram.

                await Send.Photo(Updates.chatId, $@"{Utilities.Directory.Folders.Charts}\PedidosDoca.jpg", caption: "Segue análise de pedidos em doca por unidade 📦", replyToMessageId: Updates.messageId);
                /*await Send.Text(Updates.chatId, @$"Total de pedidos em doca por unidade - {DateTime.Now} 📦

*{dtResult.Rows[0]["unidade"]}:* 
Total: {dtResult.Rows[0]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[0]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[1]["unidade"]}:* 
Total: {dtResult.Rows[1]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[1]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[2]["unidade"]}:* 
Total: {dtResult.Rows[2]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[2]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[3]["unidade"]}:* 
Total: {dtResult.Rows[3]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[3]["media_tempo"]}h ⏰

*{dtResult.Rows[4]["unidade"]}:* 
Total: {dtResult.Rows[4]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[4]["media_tempo"]}h ⏰

*{dtResult.Rows[5]["unidade"]}:* 
Total: {dtResult.Rows[5]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[5]["media_tempo"]}h ⏰

*{dtResult.Rows[6]["unidade"]}:* 
Total: {dtResult.Rows[6]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[6]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[7]["unidade"]}:* 
Total: {dtResult.Rows[7]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[7]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[8]["unidade"]}:* 
Total: {dtResult.Rows[8]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[8]["media_tempo"]}h ⏰
                              
*{dtResult.Rows[9]["unidade"]}:* 
Total: {dtResult.Rows[9]["pedidos_doca"]}
Tempo médio: {dtResult.Rows[9]["media_tempo"]}h ⏰

");*/
            }
            catch (IOException ex)
            {
                // Em caso de algum erro entre a conexão e o envio da mensagem.

                await Send.Text(Updates.chatId, @$"Aguarde um momento, processando o gráfico anterior. Solicite novamente daqui a alguns instantes.");

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