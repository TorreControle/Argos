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
    // Classe que representa o comando "Argos, consulte a CESV [NUMERO DA CESV]."

    public class ConsultaCTE : IResponse
    {
        public int id { get; set; }                  // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public ConsultaCTE()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosconsulteocte");
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
            var cte = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 6);
            var romaneio = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("*") + 8);
            cte = cte.Substring(0, cte.IndexOf(" romaneio"));


            // Executa no banco de dados a o valor na coluna Query do comando em questão.

            try
            {
                await Send.Text(Updates.chatId, $"Pra já {Updates.firstName} 😉! Consultando CTE nº {cte}");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryConsultaCTE = "qryConsultaCTE.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryConsultaCTE);
                BancoDeDadosODBC.dtm.ParamByName(qryConsultaCTE, ":CTE", cte);
                BancoDeDadosODBC.dtm.ParamByName(qryConsultaCTE, ":ROMANEIO", romaneio);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryConsultaCTE);
                BancoDeDadosODBC.dtm.Desconectar();
                DataRow row = dtResult.Rows[0];


                // Faz o envio no Telegram.

                await Send.Text(Updates.chatId, @$"
CTE *{cte}* Encontrado, segue resumo: 

*CTE: *{row["NUM_CONHECIMENTO"]}
*Série: *{row["SERIE"]}
*Cancelada?: *{row["CANCELADA"]}
*Unidade de negócio: *{row["UNIDADE_NEGOCIO"]}
*Prestador de serviço: *{row["PRESTADOR_SERVICO"]}
*Destinatário: *{row["DESTINATARIO"]}
*Local de coleta: *{row["LOCAL_COLETA"]}
*Local de entrega: *{row["LOCAL_ENTREGA"]}
*Emissão do romaneio: *{row["DATA_EMISSAO_ROMANEIO"]}
*Romaneio: *{row["NUM_ROMANEIO"]}
*Placa: *{row["PLACA_TRACAO"]}");

            }
            catch (IndexOutOfRangeException ex)
            {
                // Em caso de algum erro entre a conexão e o envio da mensagem.

                await Send.Text(Updates.chatId, @$"
Não encontrei nada no sistema 😢

*Erro de consulta*: Índice fora do intervalo ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Verifique o nº do CTE: *{cte}* ou o romaneio *{romaneio}* e tente novamente."
);
                return;
            }

            catch (InvalidOperationException ex)
            {
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
Ocorreu um erro ao consultar CTE nº *{cte}* ❌ 

*Erro:* {ex.GetType()}

{ex.Message}

Por favor entre em contato com a Torre de Controle.");
                return;
            }
        }

    }
}