/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using System.Net;
using System.Speech.Synthesis;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ArgosOnDemand.Skill
{
    public class Monitoring
    {
        // Instância do sintetizador de fala.

        public static SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();


        // Lista de ID's dos administradores

        public static long[] tabiDs = new long[]
        {
            6294979418,
            5495003005,
        };


        // Método que alerta sempre que o bot é ligado.

        async public static Task ExecutionAlert()
        {
            // Faz o envio no telegram aos administradores.

            for (int i = 0; i < tabiDs.Length; i++)
            {
                await fmDashboard.botClient.SendTextMessageAsync(
                chatId: tabiDs[i],
                text:

                $@"*Acabei de ser ligado 💡*

👤 Usuário: {Environment.UserName}
💻 Nome da máquina: {Dns.GetHostName()}
🌐 IP: {Dns.GetHostByName(Dns.GetHostName()).AddressList[1]} - {Environment.UserDomainName}
🕒 Data e hora: {DateTime.Now}".Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                parseMode: ParseMode.Markdown);

            }

            // Reproduz a frase nos auto falantes

            Speech.ToSpeak("Argos ativado!");

        }


        // Método que faz o envio do mesmo log de atualizações recebidas do Argos.

        async public static Task SendLog()
        {
            for (int i = 0; i <= tabiDs.Length - 1; i++)
            {
                await fmDashboard.botClient.SendTextMessageAsync(
                chatId: tabiDs[i],
                text:
@$"
*Mensagem:* {Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, spaces: true, comma: true)}
*Mensagem ID:* {Updates.messageId}
*Data e Hora:* {Updates.messageDate}
*Chat ID:* {Updates.chatId}
*Nome do Chat:* {Updates.chatTitle}
*Usuário ID:* {Updates.userId}
*Nome do usuário:* {Updates.firstName} {Updates.lastName}
*Usuário:* {Tools.TextProcessing(Updates.userName, alphas: true, numerics: true)}
*Atualização ID:* {Updates.updateId}".Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                parseMode: ParseMode.Markdown);

            };
        }


        // Método que faz o envio de algum erro.

        async public static Task SendError(Exception exception)
        {
            for (int i = 0; i < tabiDs.Length; i++)
            {
                await fmDashboard.botClient.SendTextMessageAsync(
                chatId: tabiDs[i],
                text:

                $@"Erro *{exception.GetType().Name}* no sistema ⚠️

*Mensagem:* {exception.Message}

*Fonte:* {exception.Source}

".Replace("*", "\\*").Replace("_", "\\_").Replace("`", "\\`").Replace("[", "\\["),
                parseMode: ParseMode.Markdown);

            }


            // Reproduz a frase nos auto falantes

            Speech.ToSpeak("Erro no sistema ");

        }

    }

}