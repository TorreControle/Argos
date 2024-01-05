using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using System.Data;

namespace ArgosOnDemand.Commands
{
    public class GeradorSenhaMotorista : IResponse
    {
        public int id { get ; set ; }
        public string? comando { get ; set ; }
        public string? tipoComando { get ; set ; }
        public string? tipoSaida { get ; set ; }
        public string? query { get ; set ; }
        public string? saida { get ; set ; }

        public Random randNum = new();


        public GeradorSenhaMotorista()
        {
            // Consulta de infomações do comando.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryComandos = "qryComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
            BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", "cpf");
            DataTable dtComandos = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos);
            BancoDeDadosODBC.dtm.Desconectar();
            DataRow row = dtComandos.Rows[0];


            // Atribuindo valores as propriedades.

            id = int.Parse(row["id"].ToString());
            comando = row["comando"].ToString();
            tipoComando = row["tipo_comando"].ToString();
            tipoSaida = row["tipo_saida"].ToString();
            query = row["query"].ToString();
            saida = row["saida"].ToString();
        }

        public async Task TriggerAsync()
        {
            string cpf = Updates.messageText[(Tools.TextProcessing(Updates.messageText, alphas: true, numerics: true, hashtag: true, asterisk: true).IndexOf("#") + 6)..];
            BancoDeDadosODBC.Conectar("ArgosOnDemand", Utilities.Conections.DataSources.MariaDB);
            string qryValidaCadastroMotorista = "qryValidaCadastroMotorista.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryValidaCadastroMotorista);
            BancoDeDadosODBC.dtm.ParamByName(qryValidaCadastroMotorista, ":CPF", cpf);
            DataTable dtValidaCadastroMotorista = BancoDeDadosODBC.dtm.ExecuteQuery(qryValidaCadastroMotorista);
            DataRow row = dtValidaCadastroMotorista.Rows[0];

            if (row["CPF"].ToString() != "Sem cadastro")
            {
                await Send.Text(Updates.chatId, $"👤 Motorista: {row["NOME"]} cadastrado!", replyToMessageId: Updates.messageId);
                string qryInsereNovaSenha = "qryInsereNovaSenha.txt";
                BancoDeDadosODBC.dtm.Limpa_Parametros(qryInsereNovaSenha);
                BancoDeDadosODBC.dtm.ParamByName(qryInsereNovaSenha, ":ID", Updates.userId.ToString());
                BancoDeDadosODBC.dtm.ParamByName(qryInsereNovaSenha, ":NOME", row["NOME"].ToString());
                BancoDeDadosODBC.dtm.ParamByName(qryInsereNovaSenha, ":CPF", row["CPF"].ToString());
                BancoDeDadosODBC.dtm.Execute(qryInsereNovaSenha);
                string qryObtemSenha = "qryObtemSenha.txt";
                DataTable dtObtemSenha = BancoDeDadosODBC.dtm.ExecuteQuery(qryObtemSenha);
                Tools.GenerateQrCode($"{dtObtemSenha.Rows[0]["SENHA"]}", "senhaMotorista");
                //await Send.Text(Updates.chatId, $"Sua senha de liberação 🔑 é a *{dtObtemSenha.Rows[0]["SENHA"]}* apresente-a na portaria da unidade destino e boa viagem!! 🚛", protectContent: true, replyToMessageId: Updates.messageId);
                await Send.Photo(Updates.chatId, Utilities.Directory.Folders.QrCode + "senhaMotorista.jpeg", caption: $"Sua senha de liberação 🔑 é a *{dtObtemSenha.Rows[0]["SENHA"]}* e esse QR Code é a sua senha de liberação. Apresente-o na portaria da unidade destino! \n\n Boa viagem 🚛");
            }
            else
            {
                await Send.Text(Updates.chatId, "Ops...😬 Notei que você ainda não possui cadastro \n\n Solicite o seu cadastro", replyToMessageId: Updates.messageId);
            }
        }
    }
}
