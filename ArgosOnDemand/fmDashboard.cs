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
    public partial class fmDashboard : Form
    {
        // Instanciando o Bot

        public static TelegramBotClient? botClient = new(Environment.GetEnvironmentVariable("ARGOS_ONDEMAND_TOKEN", EnvironmentVariableTarget.User));


        public fmDashboard()
        {
            InitializeComponent();
        }

        private void fmDashboard_Load(object sender, EventArgs e)
        {
            btnDesligar.Enabled = false;
            pnlOnOff.BackColor = Color.DarkRed;
            btnLigar_Click(sender, e);
        }

        private async void btnLigar_Click(object sender, EventArgs e)
        {
            // Inicia a instância do bot no servidor do Telegram.

            botClient.StartReceiving(
                Updates.Handler,
                Errors.Handler,
                Utilities.Telegram.Historic,
                cancellationToken: Utilities.CancellationToken.Token);


            // Alerta de execução do sistema.

            await Monitoring.ExecutionAlert();

            btnLigar.Enabled = false;
            btnDesligar.Enabled = true;
            pnlOnOff.BackColor = Color.FromArgb(18, 143, 134);

        }

        private void btnDesligar_Click(object sender, EventArgs e)
        {
            btnLigar.Enabled = true;
            btnDesligar.Enabled = false;
            Speech.ToSpeak("Encerrando atividades");
            pnlOnOff.BackColor = Color.DarkRed;
        }

        private void sairDashboard_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void homeDashboard_Click(object sender, EventArgs e)
        {
            fmMain main = new fmMain();
            main.Show();
        }
    }
}
