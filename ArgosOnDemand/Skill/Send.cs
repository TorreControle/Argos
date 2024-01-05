/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ArgosOnDemand.Skill
{
    public class Send
    {
        // Método de envio de texto

        async public static Task Text(long chatId, string text, IReplyMarkup? replyMarkup = null, bool? protectContent = false, int? replyToMessageId = null)
        {
            await fmDashboard.botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                replyMarkup: replyMarkup,
                disableNotification: true,
                protectContent: protectContent,
                replyToMessageId: replyToMessageId,
                parseMode: ParseMode.Markdown);
        }


        // Método de envio de foto

        async public static Task Photo(long chatId, string directory, IReplyMarkup? replyMarkup = null, string? caption = null, int? replyToMessageId = null)
        {
            Stream photo = File.OpenRead(directory);
            await fmDashboard.botClient.SendPhotoAsync(
                  chatId: chatId,
                  photo: photo,
                  caption: caption.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                  replyMarkup: replyMarkup,
                  disableNotification: true,
                  replyToMessageId: replyToMessageId,
                  parseMode: ParseMode.Markdown);
            photo.Dispose();
            photo.Close();
        }


        // Método de envio de vídeo

        async public static Task Video(long chatId, string directory, IReplyMarkup? replyMarkup = null, string? thumb = null, string? caption = null, int? replyToMessageId = null, bool? supportsStreaming = null)
        {
            Stream video = File.OpenRead(directory);
            await fmDashboard.botClient.SendVideoAsync(
                    chatId: chatId,
                    video: video,
                    thumb: thumb,
                    caption: caption.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                    replyMarkup: replyMarkup,
                    disableNotification: true,
                    replyToMessageId: replyToMessageId);
        }


        // Método de envio de documento

        async public static Task Document(long chatId, string directory, IReplyMarkup? replyMarkup = null, string? caption = null, int? replyToMessageId = null)
        {
            Stream document = File.OpenRead(directory);
            await fmDashboard.botClient.SendDocumentAsync(
                    chatId: chatId,
                    document: document,
                    caption: caption.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                    replyMarkup: replyMarkup,
                    disableNotification: true,
                    replyToMessageId: replyToMessageId);
        }


        // Método de envio de áudio

        async public static Task Audio(long chatId, string directory, IReplyMarkup? replyMarkup = null, int? replyToMessageId = null)
        {
            Stream audio = File.OpenRead(directory);
            await fmDashboard.botClient.SendAudioAsync(
                     chatId: chatId,
                     audio: audio,
                     replyMarkup: replyMarkup,
                     disableNotification: true,
                     replyToMessageId: replyToMessageId);
        }


        // Método de envio de animações

        async public static Task Animation(long chatId, string link, IReplyMarkup? replyMarkup = null, string? caption = null, int? replyToMessageId = null)
        {
            await fmDashboard.botClient.SendAnimationAsync(
                    chatId: chatId,
                    animation: link,
                    caption: caption.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                    replyMarkup: replyMarkup,
                    disableNotification: true,
                    replyToMessageId: replyToMessageId);
        }
    }
}
