using System.Media;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation
{
    /// <summary>
    /// 
    /// </summary>
    internal class ErrorHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public static string? Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static string? Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="ex"></param>
        /// <param name="CancellationToken"></param>
        /// <returns></returns>
        async public static Task Init(ITelegramBotClient botClient, Exception ex, CancellationToken CancellationToken)
        {
            //
            await Utilities.botClient.SendTextMessageAsync(
                chatId: 5495003005,
                text: @$"*Manipulador de erros acionado* 🪲 - {DateTime.Now}

*Classe:* ErrorHandler.cs ❌

Erro de *{ex.GetType()}* detectado

{ex.Message}",
                parseMode: ParseMode.Markdown,
                cancellationToken: CancellationToken);

            //
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@$"Manipulador de erros acionado - {DateTime.Now}

Classe: ErrorHandler.cs

Erro de {ex.GetType()} detectado

{ex.Message}

{ex}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Program.sound = new SoundPlayer();

            //
            Program.sound.SoundLocation = @$"{Tools.GetDirectoryProject()}\Resources\AlertError.wav";
            Program.sound.Play();
            Program.sound.Dispose();
        }
    }
}
