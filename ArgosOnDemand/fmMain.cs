/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using ArgosOnDemand.Skill;

namespace ArgosOnDemand
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
        }

        private void dashMenu_Click(object sender, EventArgs e)
        {
            fmDashboard dashboard = new fmDashboard();
            dashboard.Show();
        }

        private void sairMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnIrTelegram_Click(object sender, EventArgs e)
        {
            Tools.OpenUrl("https://web.telegram.org/z/#6061474214");
        }

        private void btnGitHub_Click(object sender, EventArgs e)
        {
            Tools.OpenUrl("https://github.com/TorreControle/Argos");
        }
    }
}
