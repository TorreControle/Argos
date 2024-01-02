using CliWrap;
using CliWrap.Buffered;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Data;
using System.Text;

namespace ArgosAutomation
{
    internal class Tools
    {
        // Método que obtém o diretório do projeto.
        public static string GetDirectoryProject()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
        }

        // Método responsável por executar powershell.
        public static async Task ExecuteScript(string script, string cli)
        {
            await Cli.Wrap(targetFilePath: @$"{cli}")
                .WithArguments(new[] { script })
                .ExecuteBufferedAsync();

        }

        //
        public static async Task<string> ChatGPT(string text)
        {
            // Create an instance of the OpenAIService class
            var gpt3 = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = Environment.GetEnvironmentVariable("OPENAI_TOKEN", EnvironmentVariableTarget.User),
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

        //
        public static int HasTrueValueInColumn(DataTable dataTable, string columnName)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                object cellValue = row[columnName];
                if (cellValue != null && cellValue.ToString().Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
            }

            return 0;
        }

        //
        public static string RemoveSpecialCharacters(string text, bool allowSpace = false)
        {
            string ret;

            if (allowSpace)
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
            else
                ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);

            return ret;
        }

        //
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                    sb.Append(text[i]);
                else
                    sb.Append(s_Diacritics[text[i]]);
            }

            return sb.ToString();
        }

        private static readonly char[] s_Diacritics = GetDiacritics();
        private static char[] GetDiacritics()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
                accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }

        private static string ReplaceValue(string original, string valorAntigo, string valorNovo)
        {
            int index = original.IndexOf(valorAntigo);

            if (index != -1)
            {
                // Realiza a substituição apenas no primeiro item encontrado
                string novoTexto = original.Substring(0, index) + valorNovo + original.Substring(index + valorAntigo.Length);
                return novoTexto;
            }

            // Retorna a string original se o valorAntigo não for encontrado
            return original;
        }
    }
}
