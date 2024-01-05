/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using Telegram.Bot.Types.ReplyMarkups;

namespace ArgosOnDemand.Various
{
    public class Buttons
    {
        // CD's

        public static ReplyKeyboardMarkup btnCD = new(new[]
        {
                    new KeyboardButton[] {"Pedidos"},
                    new KeyboardButton[] {"Recebimento"},
                    new KeyboardButton[] {"Pedidos em vermelho"}
        })
        {
            ResizeKeyboard = true
        };


        // Alfandegado

        public static ReplyKeyboardMarkup btnAlfandegado = new(new[]
           {
                    new KeyboardButton[] {"Carga"},
                    new KeyboardButton[] {"Descarga"},
                    new KeyboardButton[] {"Demurrages"},
                    new KeyboardButton[] {"Análise Santos"},
        })
        {
            ResizeKeyboard = true
        };


        // Transporte

        public static ReplyKeyboardMarkup btnTransporte = new(new[]
           {

                    new KeyboardButton[] {"Central de Fretes"},
                    new KeyboardButton[] {"Transporte + Frota"},
                    new KeyboardButton[] {"Transporte Bahia"},
        })
        {
            ResizeKeyboard = true
        };


        // Administração

        public static ReplyKeyboardMarkup btnAdm = new(new[]
           {
                    new KeyboardButton[] {"Argos, o que você está vendo?"},
                    new KeyboardButton[] {"Argos, encerre as atividades"},
                    new KeyboardButton[] {"F11"},
                    new KeyboardButton[] {"Print"},
                    new KeyboardButton[] {"Ative o Argos Point"},
        })
        {
            ResizeKeyboard = true
        };


        // Vários

        public static ReplyKeyboardMarkup btnSNAnalise = new(new[]
           {
                    new KeyboardButton[] {"Sim, analise esta operação."},
                    new KeyboardButton[] {"Não, não desejo."},
        })
        {
            ResizeKeyboard = true
        };
    }
}
