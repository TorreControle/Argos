using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.IO;
using System.Threading.Tasks;


namespace ArgosDot
{
    public class Tools
    {
        //Método de integraçao com o ChatGPT.
        public static async Task<string> ChatGPT(string text)
        {
            var openAI = new OpenAIAPI("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");

            var completionRequest = new CompletionRequest()
            {
                Model = "text-davinci-003",
                Prompt = $"{text}, resuma e simplifique sua resposta",
                MaxTokens = 4000,
            };

            var completion = await openAI.Completions.CreateCompletionAsync(completionRequest);
            Updates.SetTranscribeText("");
            return completion.Completions[0].Text;
        }







        // Método que obtém o diretório do projeto.
        public static string GetDirectoryProject()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
        }


        // Verificador de diretórios
        public static void VerifyDir(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}", $"Erro no método VerifyDir {ex.GetType()}");
            }
        }


        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //o arquivo está indiposnível pelas seguintes causas:
                //está sendo escrito
                //utilizado por uma outra thread
                //não existe ou sendo criado
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //arquivo está disponível
            return false;
        }


        //// Gerador de Logs
        //public static void GenerateLog(string lines, string path)
        //{
        //    VerifyDir(Utilities.Folders.Log);

        //    try
        //    {
        //        StreamWriter file = new StreamWriter(path, true);
        //        file.WriteLine(lines);
        //        file.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"{ex.Message}", $"Erro no método GenerateLog {ex.GetType()}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

    }
}
