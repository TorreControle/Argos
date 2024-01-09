using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation
{
    public class Utilities
    {
        // Instância do bot.
        public static TelegramBotClient botClient = new(Environment.GetEnvironmentVariable("ARGOS_AUTOMATION_TOKEN", EnvironmentVariableTarget.User));

        // Recebedor de atualizações
        public static ReceiverOptions receiver = new()
        {
            AllowedUpdates = new UpdateType[]
            {
                UpdateType.Message,
            }
        };

        //
        public static CancellationToken cts = new();

    }

}
