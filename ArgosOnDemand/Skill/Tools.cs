/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using ArgosOnDemand.HttpServices;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using QRCoder;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Telegram.Bot;

namespace ArgosOnDemand.Skill
{
    public class Tools
    {
        private HttpClient _httpClient;

        public static void DownloadPhoto(string fileIdPhoto, string destination, long chatId)
        {
            try
            {
                var fileInfo = fmDashboard.botClient.GetFileAsync(fileIdPhoto);
                var filePath = fileInfo.Result.FilePath;

                var client = new WebClient();
                client.DownloadFile("https://api.telegram.org/file/bot" + Utilities.Telegram.Token + "/" + filePath, destination);
            }
            catch (Exception exception)
            {
                fmDashboard.botClient.SendTextMessageAsync(chatId, "Opa, erro no download:\n\n" + exception.Message);
            }
        }

        public static void DownloadVoice(string fileIdVoice, string destination, long chatId)
        {
            try
            {
                var fileInfo = fmDashboard.botClient.GetFileAsync(fileIdVoice);
                var filePath = fileInfo.Result.FilePath;

                var client = new WebClient();
                client.DownloadFile("https://api.telegram.org/file/bot" + Utilities.Telegram.Token + "/" + filePath, destination);
            }
            catch (Exception exception)
            {
                fmDashboard.botClient.SendTextMessageAsync(chatId, "Opa, erro no download:\n\n" + exception.Message);
            }
        }

        public static string TextProcessing(string text, bool alphas = false, bool graveAccent = false, bool numerics = false, bool dashes = false, bool spaces = false, bool periods = false, bool interrogation = false, bool exclamation = false, bool atsing = false, bool hashtag = false, bool dollarsign = false, bool porcent = false, bool equal = false, bool comma = false, bool bar1 = false, bool bar2 = false, bool twopoints = false, bool pointcomma = false, bool smaller = false, bool larger = false, bool add = false, bool underline = false, bool asterisk = false, bool emoji = false)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            if (new[] { alphas, graveAccent, numerics, dashes, spaces, periods, interrogation, exclamation, atsing, hashtag, dollarsign, porcent, equal, comma, bar1, bar2, twopoints, pointcomma, smaller, larger, add, underline, asterisk, emoji }.All(x => x == false)) return text;
            var whitelistChars = new HashSet<char>(string.Concat(
                alphas ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" : "",
                graveAccent ? "`" : "\\`",
                numerics ? "0123456789" : "",
                dashes ? "-" : "",
                periods ? "." : "",
                spaces ? " " : "",
                interrogation ? "?" : "",
                exclamation ? "!" : "",
                atsing ? "@" : "",
                hashtag ? "#" : "",
                dollarsign ? "$" : "",
                porcent ? "%" : "",
                equal ? "=" : "",
                comma ? "," : "",
                bar1 ? "/" : "",
                bar2 ? "\\" : "",
                twopoints ? ":" : "",
                pointcomma ? ";" : "",
                smaller ? "<" : "",
                larger ? ">" : "",
                add ? "+" : "",
                underline ? "_" : "\\_",
                asterisk ? "*" : "\\*",
                emoji ? @"\p{Cs}" : ""
            ).ToCharArray());
            var scrubbedMessageText = text.Aggregate(new StringBuilder(), (sb, @char) =>
            {
                if (whitelistChars.Contains(@char)) sb.Append(@char);
                return sb;
            }).ToString();
            return scrubbedMessageText.ToLower();
        }

        public static void OpenUrl(string url)
        {
            Process.Start("explorer.exe", $@"{url}");
        }

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
                    Model = "gpt-4"
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

        public static void GenerateQrCode(string info, string nameFile)
        {
            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(info, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);
            Image qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(Utilities.Directory.Folders.QrCode + $"{nameFile}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            qrCodeImage.Dispose();
        }

        public static string WriteData(DataTable data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            data.Rows.Cast<DataRow>().ToList().ForEach(dataRow =>
            {
                data.Columns.Cast<DataColumn>().ToList().ForEach(column =>
                {
                    stringBuilder.AppendFormat("{0}:{1} ", column.ColumnName, dataRow[column]);
                });
                stringBuilder.Append(Environment.NewLine);
            });
            return stringBuilder.ToString();
        }


        public static IConfiguration BuildConfig()
        {
            var dir = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
                .AddUserSecrets(Assembly.GetExecutingAssembly());

            return configBuilder.Build();
        }

        private async Task<GenerateImageResponse> GenerateImages(GenerateImageRequest prompt, CancellationToken cancellation = default)
        {
            using var rq = new HttpRequestMessage(HttpMethod.Post, "/v1/images/generations");

            var jsonRequest = JsonSerializer.Serialize(prompt, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            rq.Content = new StringContent(jsonRequest);
            rq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var apiKey = "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";
            rq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var subscriptionId = "org-aaltqMciaDg7HJcO2CJQkR7x";
            rq.Headers.TryAddWithoutValidation("OpenAI-Organization", subscriptionId);

            var response = await _httpClient.SendAsync(rq, HttpCompletionOption.ResponseHeadersRead, cancellation);

            response.EnsureSuccessStatusCode();

            var content = response.Content;

            var jsonResponse = await content.ReadFromJsonAsync<GenerateImageResponse>(cancellationToken: cancellation);

            return jsonResponse;
        }


        // Método que obtém o diretório do projeto.
        public static string GetDirectoryProject()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
        }

        //public static string OCR(string directoryPhoto)
        //{
        //    try
        //    {
        //        var Ocr = new IronTesseract();
        //        Ocr.Language = OcrLanguage.Portuguese;
        //        Ocr.Configuration.TesseractVersion = TesseractVersion.Tesseract5;

        //        var Input = new OcrInput();

        //        Input.AddImage(directoryPhoto);
        //        var Result = Ocr.Read(Input);
        //        return Result.Text;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "N�o consegui ler o c�digo, por favor reevie com uma imagem mais n�tida.\n\nErro: " + ex.Message;
        //    }

        //}

        //public static string ReadBarCode(string directoryPhoto)
        //{
        //    BarcodeResult Result = IronBarCode.BarcodeReader.ReadASingleBarcode(directoryPhoto, BarcodeEncoding.QRCode | BarcodeEncoding.Code128, BarcodeReader.BarcodeRotationCorrection.Extreme, BarcodeReader.BarcodeImageCorrection.DeepCleanPixels);

        //    if (Result != null)
        //    {
        //        return Result.Text;
        //    }
        //    else
        //    {
        //        return "Essa imagem n�o cont�m um c�digo legivel, por favor envie outra foto ou a mesma mais n�tida.";
        //    }
        //}
    }
}