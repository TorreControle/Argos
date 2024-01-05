/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

namespace ArgosOnDemand.Commands
{
    // Classe manipuladora de comandos.
    public class CommandHandler
    {
        // Campo do tipo IResponse que possibilita instanciarmos qualquer classe que implemente a interface.

        private readonly IResponse response;

        public int id { get; set; }                  // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Método construtor que retorna os membros de uma classe que implementa a interface IResponse.

        public CommandHandler(IResponse response)
        {
            this.response = response;
            id = response.id;
            comando = response.comando;
            tipoComando = response.tipoComando;
            tipoSaida = response.tipoSaida;
            query = response.query;
            saida = response.saida;
        }


        // Método manipulador que "chama" o método trigger de toda a classe que implementa a interface IResponse

        async public Task Answer()
        {
            // Método que aciona o método principal de execução de um comando.

            await response.TriggerAsync();

        }
    }
}
