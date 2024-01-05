/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

namespace ArgosOnDemand.Commands
{
    // Interface que representa os comportamentos de um comando.

    public interface IResponse
    {
        public int id { get; set; }                 // ID do comando na tabela T_COMANDOS_ARGOS.
        public string? comando { get; set; }         // Gatilho que aciona o comando.
        public string? tipoComando { get; set; }     // Tipo do comando.
        public string? tipoSaida { get; set; }       // Como que é o retorno do comando.
        public string? query { get; set; }           // Query que deve ser executada caso tipoComando for "Consulta".
        public string? saida { get; set; }           // Texto de saida para caso tipoComando for "Texto".


        // Método que aciona o método principal de execução de um comando.

        public abstract Task TriggerAsync();

    }
}
