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

    public class ConsultaCESV : IResponse
    {
        public int id { get; set; }                  // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public ConsultaCESV()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosconsulteacesv");
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
            // Obtém a CESV.

            var cesv = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 6);


            // Executa no datalake a query referente ao comando em questão.

            try
            {
                await Send.Text(Updates.chatId, $"Claro {Updates.firstName}! Buscando CESV nº {cesv.ToUpper()} 🔎.");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryComandos = "qryConsultaCESV.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
                BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":CESV", cesv);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos);
                BancoDeDadosODBC.dtm.Desconectar();
                DataRow row = dtResult.Rows[0];



                // Faz o envio no Telegram.

                await Send.Text(Updates.chatId, @$"

CESV nº {cesv} encontrada na unidade {row["sis_nome_filial"]} ✅ segue resultado da consulta.

*nº CESV:* {row["CESV"]}
*Cliente:* {row["cliente"]}
*Data da entrada:* {row["DATA_CESV_ENTRADA"]}
*CESV Fim:* {row["DATA_CESV_FIM"]}
*Data do deslacre :* {row["DATA_OS_DESLACRE"]}
*Liberação:* {row["DOC_LIBERACAO"]}
*CIF OS:* {row["DOC_CIF_OS"]}
*Inicio OS:* {row["DATA_OS_INICIO"]}
*Fim OS:* {row["DATA_OS_FIM"]}
*Quantidade lote:* {row["QTD_LOTE"]}
*Filial:* {row["sis_nome_filial"]}");

            }
            catch (IndexOutOfRangeException ex)
            {
                // Em caso de algum erro entre a conexão e o envio da mensagem.

                await Send.Text(Updates.chatId, @$"
Não encontrei nada no sistema 😢

*Erro de consulta*: Índice fora do intervalo ({ex.GetType()}) ❌

*Descrição:* {ex.Message}

Verifique o nº da CESV *{cesv.ToUpper()}* e tente novamente."
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
Ocorreu um erro ao consultar CESV *{cesv.ToUpper()}* ❌ 

*Erro:* {ex.GetType()}

{ex.Message}

Por favor entre em contato com a Torre de Controle.");

                return;
            }
        }
    }
}