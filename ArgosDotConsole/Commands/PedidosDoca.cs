using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ArgosDot.commands
{
    public class PedidosDoca : ICommand
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
        public PedidosDoca()
        {
            string[] words = { "pedidos", "doca" };
            foreach (string word in words) { KeyWordsList.Add(word); }
            ActivatorCommand = Updates.GetTranscribeText();
            ResponseText = null;
            IsCompleted = false;
        }


        // 
        public async Task Run()
        {

            // Obtém a unidade solicitada

            string unidade = ActivatorCommand.Substring(ActivatorCommand.IndexOf("no ") + 17).ToUpper();

            try
            {
                BancoDeDadosODBC.Conectar("ArgosDot", Utilities.DSN.Databricks);
                string qryPedidosDoca = "qryPedidosDoca.txt";
                DataTable dtResult = BancoDeDadosODBC.dtm.ExecuteQuery(qryPedidosDoca);
                ResponseText = $@"A unidade {dtResult.Rows[0]["unidade"]} está com {dtResult.Rows[0]["pedidos_doca"]} pedidos em doca e de acordo com meus cálculos esses {dtResult.Rows[0]["pedidos_doca"]} pedidos estão com um tempo médio em doca de {dtResult.Rows[0]["media_tempo"]} horas.";
                Updates.SetResponseText(ResponseText);
                TextToSpeech.SpeechSynthesis(Updates.GetResponseText(), Utilities.Directory.Audio.Output);

            }
            catch (IndexOutOfRangeException ex)
            {
                // Em caso da solicitação da unidade não retornar nada.

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($@"Erro de consulta: Índice fora do intervalo ({ex.GetType()})
Descrição: {ex.Message}");
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }

            catch (InvalidOperationException ex)
            {
                // Em caso da solicitação da unidade retornar erro .

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($@"Erro: Operação inválida ({ex.GetType()})
Descrição: {ex.Message}");
                Console.BackgroundColor = ConsoleColor.Black;
                return;
            }

            catch (Exception ex)
            {
                // Em caso de algum erro entre a conexão e o envio da mensagem.

                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($@"Erro: {ex.Message}");
                Console.BackgroundColor = ConsoleColor.Black;
                return;

            }
            finally
            {
                BancoDeDadosODBC.dtm.Desconectar();

            }

        }

    }

}
