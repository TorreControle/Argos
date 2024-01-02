using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ArgosDot.commands
{
    public class PedidosAbertos : ICommand
    {
        // 
        public static List<string> KeyWordsList = new List<string>();

        //
        public string ActivatorCommand { get; set; }

        // 
        public string ResponseText { get; set; }

        // 
        public bool IsCompleted { get; set; }


        // 
        public PedidosAbertos()
        {
            string[] words = { "pedidos", "abertos" };
            foreach (string word in words) { KeyWordsList.Add(word); }
            ActivatorCommand = Updates.GetTranscribeText();
            ResponseText = null;
            IsCompleted = false;
        }


        // 
        public async Task Run()
        {
            try
            {
                BancoDeDadosODBC.Conectar("ArgosDot", Utilities.DSN.Databricks);
                string qryPedidosAbertos = "qryPedidosAbertos.txt";
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryPedidosAbertos);

                ResponseText = $@"Neste momento estamos com total de {dtResult.Rows[0]["PEDIDO"]} pedidos em aberto";
                Updates.SetResponseText(ResponseText);
                TextToSpeech.SpeechSynthesis(Updates.GetResponseText(), Utilities.Directory.Audio.Output);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                BancoDeDadosODBC.dtm.Desconectar();

            }

        }

    }

}
