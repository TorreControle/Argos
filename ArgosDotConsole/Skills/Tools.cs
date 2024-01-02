





using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ArgosDot
{
    public class Tools
    {
        //Método de integraçao com o ChatGPT.
        public static async Task<string> ChatGPT(string text)
        {
            string apiKey = "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";

            // Create an instance of the OpenAIService class
            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiKey,
                Organization = "org-aaltqMciaDg7HJcO2CJQkR7x"

            });

            // Create a chat completion request
            var completionResult = await gpt3.ChatCompletion.CreateCompletion
                (new ChatCompletionCreateRequest()
                {
                    Messages = new List<ChatMessage>(new ChatMessage[]
                    { new ChatMessage("user", text) }),
                    Model = "gpt-3.5-turbo-16k"
                });

            // Check if the completion result was successful and handle the response
            if (completionResult.Successful)
            {
                return completionResult.Choices[0].Message.Content;
            }
            else
            {
                return $@"

Erro: {completionResult.Error.Message}";
            }
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


        // Gerador de Logs
        public static void GenerateLog(string lines, string path)
        {
            VerifyDir(Utilities.Folders.Log);

            try
            {
                StreamWriter file = new StreamWriter(path, true);
                file.WriteLine(lines);
                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}", $"Erro no método GenerateLog {ex.GetType()}");
            }
        }

    }
}
