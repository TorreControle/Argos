/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using Telegram.Bot;
using Telegram.Bot.Types;
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
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }


        // Método de envio de foto

        async public static Task Photo(long chatId, string directory, IReplyMarkup? replyMarkup = null, string? caption = null, int? replyToMessageId = null)
        {
            var photo = new FileStream($@"{directory}", FileMode.Open, FileAccess.Read);
            var PrintPath = InputFile.FromStream(photo);
            await fmDashboard.botClient.SendPhotoAsync(
                  chatId: chatId,
                  photo: PrintPath,
                  caption: caption.Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                  replyMarkup: replyMarkup,
                  disableNotification: true,
                  replyToMessageId: replyToMessageId);
            photo.Dispose();
            photo.Close();
        }

    }
}
