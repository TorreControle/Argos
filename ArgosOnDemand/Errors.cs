/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using ArgosOnDemand.Skill;
using Telegram.Bot;

namespace ArgosOnDemand
{
    public class Errors
    {
        //

        public static fmDashboard dashboard = new();


        //

        async public static Task Handler(ITelegramBotClient botClient, Exception Exception, CancellationToken CancellationToken)
        {
            await Monitoring.SendError(Exception);
            MessageBox.Show(@$"Erro: {Exception.Message}", Exception.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}