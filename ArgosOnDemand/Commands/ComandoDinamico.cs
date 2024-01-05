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
    // Classe que representa o comando em que o Argos "cria" query para ele mesmo.

    public class ComandoDinamico : IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Construtor do comando, trazendo do banco de dados as informações do comando.

        public ComandoDinamico()
        {
            // Consulta de infomações do comando.

            BancoDeDadosOra.Conectar("ArgosOnDemand", Utilities.Conections.Servers.ReportsWMS);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosOra.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosOra.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosquantos");
            DataTable dtComandos = BancoDeDadosOra.dtm.ExecuteQuery(qryComandos);
            BancoDeDadosOra.dtm.Desconectar();
            DataRow row = dtComandos.Rows[0];


            // Atribuindo valores as propriedades.  

            id = int.Parse(row["ID"].ToString());
            comando = row["GATILHO"].ToString();
            tipoComando = row["TIPO_COMANDO"].ToString();
            tipoSaida = row["TIPO_SAIDA"].ToString();
            query = row["QUERY"].ToString();
            saida = row["SAIDA"].ToString();

        }


        // Método de execução do comando.

        public async Task TriggerAsync()
        {

            // Executa no banco de dados a o valor na coluna Query do comando em questão.

            try
            {
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                BancoDeDadosODBC.dtm.Limpa_Parametros(comando);
                BancoDeDadosODBC.dtm.ParamByName(comando, ":MESSAGETEXT", Updates.messageText);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(comando);
                BancoDeDadosODBC.dtm.Desconectar();
                DataRow row = dtResult.Rows[0];

                MessageBox.Show(row["Selecti"].ToString());

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
Ocorreu um erro ao consultar ❌ 

*Erro:* {ex.Message}

Por favor entre em contato com a Torre de Controle.");
                return;
            }
        }
    }
}
