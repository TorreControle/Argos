using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgosOnDemand.Commands
{
    internal class PerfilPedido : IResponse
    {
        public int id { get; set; }
        public string? comando {get; set;}
        public string? tipoComando {get; set;}
        public string? tipoSaida {get; set;}
        public string? query {get; set;}
        public string? saida { get; set; }

        public PerfilPedido()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argosqualoperfildopedido");
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
            var pedido = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 8);
            var cliente = Updates.messageText.Substring(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("*") + 10);
            pedido = pedido[..pedido.IndexOf(" da *")];
            cliente = cliente[..cliente.IndexOf("?")];

            try
            {
                await Send.Text(Updates.chatId, $"Claro {Updates.firstName}! Calculando perfil do pedido nº {pedido.ToUpper()} no sistema 🔎.");
                BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.Databricks);
                string qryPerfilPedido = "qryPerfilPedido.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryPerfilPedido);
                BancoDeDadosODBC.dtm.ParamByName(qryPerfilPedido, ":PEDIDO", pedido);
                BancoDeDadosODBC.dtm.ParamByName(qryPerfilPedido, ":CLIENTE", cliente);
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryPerfilPedido);
                var result = dtResult.Rows[0]["Tempo"].ToString();
                BancoDeDadosODBC.dtm.Desconectar();
                await Task.Delay(TimeSpan.FromSeconds(3));
                await Send.Text(Updates.chatId, @$"{dtResult.Rows[0]["Tempo"]}");

            }
            catch (IndexOutOfRangeException) 
            {
                await Send.Text(Updates.chatId, @$"Sinto muito mas não pude encontrar o pedido {pedido} para fazer a análise de perfil 😢 verifique o número do pedido ou entre em contato com meus administradores na Torre de Controle.");

            }
        }
    }
}
