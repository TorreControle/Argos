using ArgosOnDemand.Database;
using ArgosOnDemand.HttpServices;
using ArgosOnDemand.Skill;
using System.Data;
using System.Net;

namespace ArgosOnDemand.Commands
{
    internal class ImageGenerator : IResponse
    {
        public int id { get; set; }
        public string? comando { get; set; }
        public string? tipoComando { get; set; }
        public string? tipoSaida { get; set; }
        public string? query { get; set; }
        public string? saida { get; set; }

        private HttpClient _httpClient;
        private WebClient client = new();

        public ImageGenerator()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "argoscrie");
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
            await Send.Text(Updates.chatId, "Claro, aguarde uns instantes a sua imagem já está sendo gerada!");
            var config = Tools.BuildConfig();
            IOpenAIProxy aiClient = new OpenAIHttpService(config);
            var nImages = int.Parse(config["OpenAi:DALL-E:N"]);
            var imageSize = config["OpenAi:DALL-E:Size"];
            var prompt = new GenerateImageRequest(Updates.messageText, nImages, imageSize);
            var result = await aiClient.GenerateImages(prompt);
            var img = await aiClient.DownloadImage(result.Data[0].Url);
            await File.WriteAllBytesAsync(@$"{Tools.GetDirectoryProject()}\Resources\img.jpg", img);
            await Send.Photo(Updates.chatId, @$"{Tools.GetDirectoryProject()}\Resources\img.jpg", replyToMessageId: Updates.messageId, caption: "Aqui sua imagem!");

        }
    }
}
