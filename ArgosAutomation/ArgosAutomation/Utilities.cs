using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

        public class Buttons
        {
            public static ReplyKeyboardMarkup btnCD = new(new[]
            {
                    new KeyboardButton[] {"Pedidos"},
                    new KeyboardButton[] {"Recebimento" },
                    new KeyboardButton[] {"Expedição Linhas"},
                    new KeyboardButton[] {"Expedição Pedidos"},
                    new KeyboardButton[] {"Acompanhamento Riffel"},
                    new KeyboardButton[] {"Acompanhamento Expedição"},
                    new KeyboardButton[] {"Acompanhamento Elgin"},
                    new KeyboardButton[] {"Acompanhamento Elgin Joinville"},
                    new KeyboardButton[] {"Ocupação Sul"},
                    new KeyboardButton[] {"Ocupação Sudeste"},
                    new KeyboardButton[] {"Ocupação AG2"},
                    new KeyboardButton[] {"Ocupação AGQ"},
                    new KeyboardButton[] {"Ocupação CD5"},
                    new KeyboardButton[] {"Ocupação Mauá"},
                    new KeyboardButton[] {"Ocupação CD SJP"},
                    new KeyboardButton[] {"Ocupação Sumaré"},
                    new KeyboardButton[] {"Ocupação Barueri 2"},
                    new KeyboardButton[] {"Ocupação Tecnopark"},
                    new KeyboardButton[] {"Ocupação Joinville 1"},
                    new KeyboardButton[] {"Ocupação CD Itajaí 3"},
                    new KeyboardButton[] {"Ocupação CD Curitiba"},
                    new KeyboardButton[] {"Ocupação CD Barueri 1"},
                })
            {
                ResizeKeyboard = true
            };

            public static ReplyKeyboardMarkup btnAlfandegado = new(new[]
               {
                    new KeyboardButton[] {"Demurrages"},
                    new KeyboardButton[] {"Análise Santos"},
                    new KeyboardButton[] {"Controle de Containers"},
                    new KeyboardButton[] {"Planejamento Containers"},
                    new KeyboardButton[] {"Carregamentos Sudeste"},
                    new KeyboardButton[] {"Carregamentos Sul"},
                    new KeyboardButton[] {"Carregamento Santos"},
                    new KeyboardButton[] {"Carregamento Campinas"},
                    new KeyboardButton[] {"Carregamento Mooca"},
                    new KeyboardButton[] {"Carregamento Barueri"},
                    new KeyboardButton[] {"Carregamento Itajaí"},
                    new KeyboardButton[] {"Carregamento Joinville"},
                    new KeyboardButton[] {"Carregamento São Jose"},
                    new KeyboardButton[] {"Carregamento Curitiba"},
                })
            {
                ResizeKeyboard = true
            };

            public static ReplyKeyboardMarkup btnTransporte = new(new[]
               {
                    new KeyboardButton[] {"Transporte KMM"},
                    new KeyboardButton[] {"Transporte LOGIX"},
                    new KeyboardButton[] {"Detalhamento frota por unidade"},
                    new KeyboardButton[] {"Central de Fretes"},
                    new KeyboardButton[] {"Verticais Nordeste"},
                    new KeyboardButton[] {"Verticais Sudeste"},
                    new KeyboardButton[] {"Verticais Sul"},
                })
            {
                ResizeKeyboard = true
            };

            public static ReplyKeyboardMarkup btnAcompOper = new(new[]
               {
                    new KeyboardButton[] {"Pedidos"},
                    new KeyboardButton[] {"Recebimento" },
                    new KeyboardButton[] {"Expedição Linhas"},
                    new KeyboardButton[] {"Expedição Pedidos"},
                    new KeyboardButton[] {"Acompanhamento Riffel"},
                    new KeyboardButton[] {"Acompanhamento Expedição"},
                    new KeyboardButton[] {"Acompanhamento Elgin"},
                    new KeyboardButton[] {"Acompanhamento Elgin Joinville"},
                    new KeyboardButton[] {"Ocupação Sul"},
                    new KeyboardButton[] {"Ocupação Sudeste"},
                    new KeyboardButton[] {"Ocupação AG2"},
                    new KeyboardButton[] {"Ocupação AGQ"},
                    new KeyboardButton[] {"Ocupação CD5"},
                    new KeyboardButton[] {"Ocupação Mauá"},
                    new KeyboardButton[] {"Ocupação CD SJP"},
                    new KeyboardButton[] {"Ocupação Sumaré"},
                    new KeyboardButton[] {"Ocupação Barueri 2"},
                    new KeyboardButton[] {"Ocupação Tecnopark"},
                    new KeyboardButton[] {"Ocupação Joinville 1"},
                    new KeyboardButton[] {"Ocupação CD Itajaí 3"},
                    new KeyboardButton[] {"Ocupação CD Curitiba"},
                    new KeyboardButton[] {"Ocupação CD Barueri 1"},
                    new KeyboardButton[] {"Demurrages"},
                    new KeyboardButton[] {"Análise Santos"},
                    new KeyboardButton[] {"Controle de Containers"},
                    new KeyboardButton[] {"Planejamento Containers"},
                    new KeyboardButton[] {"Carregamentos Sudeste"},
                    new KeyboardButton[] {"Carregamentos Sul"},
                    new KeyboardButton[] {"Carregamento Santos"},
                    new KeyboardButton[] {"Carregamento Campinas"},
                    new KeyboardButton[] {"Carregamento Mooca"},
                    new KeyboardButton[] {"Carregamento Barueri"},
                    new KeyboardButton[] {"Carregamento Itajaí"},
                    new KeyboardButton[] {"Carregamento Joinville"},
                    new KeyboardButton[] {"Carregamento São Jose"},
                    new KeyboardButton[] {"Carregamento Curitiba"},
                    new KeyboardButton[] {"Transporte KMM"},
                    new KeyboardButton[] {"Transporte LOGIX"},
                    new KeyboardButton[] {"Detalhamento frota por unidade"},
                    new KeyboardButton[] {"Central de Fretes"},
                    new KeyboardButton[] {"Verticais Nordeste"},
                    new KeyboardButton[] {"Verticais Sudeste"},
                    new KeyboardButton[] {"Verticais Sul"},
                })
            {
                ResizeKeyboard = true
            };

        }

    }

}
