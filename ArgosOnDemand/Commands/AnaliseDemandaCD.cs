using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using MySqlX.XDevAPI;
using System.Data;
using Graphics = ArgosOnDemand.Skill.Graphics;

namespace ArgosOnDemand.Commands
{
    internal class AnaliseDemandaCD : IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public AnaliseDemandaCD()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosanaliseademandado");
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
            var unidade = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 7);
            await Send.Text(Updates.chatId, $"Positivo!! Fazendo cálculos e e agrupando dados do {unidade}.");

            //

            try
            {
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryAnaliseDemandaCD = "qryAnaliseDemandaCD.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryAnaliseDemandaCD);
                BancoDeDadosODBC.dtm.ParamByName(qryAnaliseDemandaCD, ":UNIDADE", unidade);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryAnaliseDemandaCD);
                BancoDeDadosODBC.dtm.Desconectar();

                //Graphics.Bar(dtResult, "Pedidos", "Mes", "Quantidade de pedidos", "Mês", @$"Total de pedidos por mês no {unidade} - Atualização: {DateTime.Now}", $"AnaliseDemanda");
                //Graphics.Bar(dtResult, "Linhas", "Mes", "Quantidade de linhas", "Mês", @$"Total de linhas por mês no {unidade} - Atualização: {DateTime.Now}", $"AnaliseDemandaLinhas");


                var resposta = await Tools.ChatGPT(@$"
De acordo com os dados abaixo referente aos pedidos do {unidade}, gere uma análise de demanda e um texto sobre, seja analítico. 

Dados: 

{Tools.WriteData(dtResult)}");

                await Send.Text(Updates.chatId, resposta, replyToMessageId: Updates.messageId);
                //await Send.Photo(Updates.chatId, $@"{Utilities.Directory.Folders.Charts}\AnaliseDemanda.jpg", caption: resposta, replyToMessageId: Updates.messageId);
                //await Send.Photo(Updates.chatId, $@"{Utilities.Directory.Folders.Charts}\AnaliseDemandaLinhas.jpg", caption: "*Análise de Linhas*");

            }
            catch (IndexOutOfRangeException ex)
            {
                // Em caso da solicitação da unidade não retornar nada.

                await Send.Text(Updates.chatId, @$"

Não encontrei nada no sistema 😢

*Erro de consulta*: Índice fora do intervalo ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Você pode ter passado a unidade {unidade} errado, por favor verifique e tente novamente."
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
Ocorreu um erro ao consultar a demanda da unidade *{unidade}* ❌ 

Por favor entre em contato com a Torre de Controle.");

                return;
            }
        }
    }
}
