/*
Argos - Sistema Especialista Torre de Controle

Data de cria��o: 14/07/2022
Data de produ��o: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 J�ssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

namespace ArgosOnDemand
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new fmDashboard());
        }
    }
}