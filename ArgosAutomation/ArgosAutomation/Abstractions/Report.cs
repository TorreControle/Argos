using ArgosAutomation.Databases;
using System.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ArgosAutomation.Abstractions
{
    /// <summary>
    /// Classe que representa um report no Telegram.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// ID do painel.
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Comando atrelado ao painel.
        /// </summary>
        public string? Command { get; set; }
        /// <summary>
        /// Nome.
        /// </summary>
        private string? ReportName { get; set; }
        /// <summary>
        /// Link de acesso.
        /// </summary>
        private string? Link { get; set; }
        /// <summary>
        /// Chat ID respectivo ao grupo do painel.
        /// </summary>
        public List<long?> ChatIdGroup = new();
        /// <summary>
        /// Nome do grupo.
        /// </summary>
        public List<string?> GroupName = new();
        /// <summary>
        /// Vertical de negócio.
        /// </summary>
        private List<string?> VerticalBusiness = new();
        /// <summary>
        /// Hora que o painel é reportado.
        /// </summary>
        private string? TimeReport { get; set; }
        /// <summary>
        /// Rotina na qual o report está agendado.
        /// </summary>
        private string? RoutineName { get; set; }
        /// <summary>
        /// Expressão Cron definida pela regra de negócio.
        /// </summary>
        private string? ExpressionCron { get; set; }
        /// <summary>
        /// Tipo de envio do painel.
        /// </summary>
        private string? Type { get; set; }
        /// <summary>
        /// Armazena se o painel está ativo ou não.
        /// </summary>
        public int? Enable { get; set; }
        /// <summary>
        /// Armazena se o painel está sendo gerado ou não.
        /// </summary>
        public int? BeingGenerated { get; set; }
        /// <summary>
        /// Legenda do print.
        /// </summary>
        private string? Caption { get; set; }
        /// <summary>
        /// Manipula o fluxo de arquivos no Windows.
        /// </summary>
        private FileStream? File { get; set; }
        /// <summary>
        /// Manipulador de arquivo na API do Telegram.
        /// </summary>
        private InputFile? PrintPath { get; set; }
        /// <summary>
        /// Armazena as query's as serem executadas dentro da classe.
        /// </summary>
        private string qry;
        /// <summary>
        /// Armazena as tabelas as serem obtidas dentro da classe.
        /// </summary>
        private DataTable dt;

        /// <summary>
        /// Constrói um objeto Report partindo do ID do painel em questão.
        /// </summary>
        /// <param name="id">ID do painel</param>
        /// <param name="reportTime">Horário em que o painel é reportado no formato "HH:mm:ss".</param>
        public Report(int id, string reportTime)
        {
            // Faz a consulta no banco de dados do painel a ser construído.
            Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
            qry = "qryGetReportInfoID.txt";
            Odbc.dtm.CleanParamters(qry);
            Odbc.dtm.ParamByName(qry, ":ID", id.ToString());
            Odbc.dtm.ParamByName(qry, ":HORA_ENVIO", reportTime);
            dt = Odbc.dtm.ExecuteQuery(qry);
            //Odbc.dtm.Disconect();

            // Atribui os valores as propriedades.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Id = int.Parse((string)dt.Rows[i]["ID"]);
                Command = (string)dt.Rows[i]["COMANDO"];
                ReportName = (string)dt.Rows[i]["NOME_PAINEL"];
                Link = (string)dt.Rows[i]["LINK"];
                ChatIdGroup.Add(long.Parse((string)dt.Rows[i]["CHAT_ID_GRUPO"]));
                GroupName.Add((string)dt.Rows[i]["NOME_GRUPO"]);
                VerticalBusiness.Add((string)dt.Rows[i]["VERTICAL_NEGOCIO"]);
                TimeReport = (string)dt.Rows[i]["HORA_REPORT"];
                RoutineName = (string)dt.Rows[i]["NOME_ROTINA"];
                ExpressionCron = (string)dt.Rows[i]["EXPRESSAO_CRON"];
                Type = (string)dt.Rows[i]["TIPO_ENVIO"];
                Enable = int.Parse((string)dt.Rows[i]["ATIVO"]);
            }

            Caption = @$"🤖: Segue painel de *{ReportName}* atualizado 📊.";
            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Informações do painel {ReportName} obtidas do banco de dados! Construindo report.");

        }
        /// <summary>
        /// Constrói um objeto Report partindo do comando do painel em questão.
        /// </summary>
        /// <param name="command">Comando atrelado ao painel</param>
        /// <param name="reportTime">Horário em que o painel é reportado no formato "HH:mm:ss".</param>
        public Report(string command)
        {
            // Faz a consulta no banco de dados do painel a ser construído.
            Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
            qry = "qryGetReportInfoName.txt";
            Odbc.dtm.CleanParamters(qry);
            Odbc.dtm.ParamByName(qry, ":COMANDO", command);
            dt = Odbc.dtm.ExecuteQuery(qry);
            //Odbc.dtm.Disconect();

            // Atribui os valores as propriedades.
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Id = int.Parse((string)dt.Rows[i]["ID"]);
                Command = (string)dt.Rows[i]["COMANDO"];
                ReportName = (string)dt.Rows[i]["NOME_PAINEL"];
                Link = ((string)dt.Rows[i]["LINK"]);
                ChatIdGroup.Add(long.Parse((string)dt.Rows[i]["CHAT_ID_GRUPO"]));
                GroupName.Add((string)dt.Rows[i]["NOME_GRUPO"]);
                VerticalBusiness.Add((string)dt.Rows[i]["VERTICAL_NEGOCIO"]);
                Type = ((string)dt.Rows[i]["TIPO_ENVIO"]);
                Enable = int.Parse((string)dt.Rows[i]["ATIVO"]);
            }

            Caption = $"🤖: {Tools.ChatGPT(@$"Criar uma legenda curta para a foto de um painel de {ReportName} de análise logística de {VerticalBusiness} essa legenda é para o Telegram e será vista por lideres, supervisores, coordenadores, gerentes e diretores de uma empresa chamada Multilog SA, inicie a frase assim: Segue report de {ReportName}.").Result}";
            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Informações do painel {ReportName} obtidas do banco de dados! Construindo report.");

        }
        /// <summary>
        /// Método responsável por gerar o print do painel a ser reportado.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        async public Task Generate()
        {

            // Atualializa o valor na coluna GERANDO do painel em questão para 1, pois esse é o estado da aplicação em que ela está gerando um report.
            Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Gerando painel de {ReportName}.");
            Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
            qry = "qryUpdateGenerating.txt";
            Odbc.dtm.CleanParamters(qry);
            Odbc.dtm.ParamByName(qry, ":VALUE", "1");
            Odbc.dtm.ParamByName(qry, ":ID", $"{Id}");
            Odbc.dtm.ExecuteNonQuery(qry);
            //Odbc.dtm.Disconect();

            string path = @"C:\Temp\ArgosAutomation\arquivos\imagens";
            string filePath = Path.Combine(path, "report.jpg");

            // Verifica se o diretório existe, se não existir, cria o diretório.
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Verifica se o arquivo existe, se não existir, cria o arquivo.
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Close(); // Importante fechar o arquivo após criá-lo
            }

            // Tenta fazer a execução da função "GenerateReport" que foi definida dentro de um profile em PowerShell usando o link do painel em questão que está armazenado na propriedade Link do objeto.
            try
            {
                await Tools.ExecuteScript($"GenerateReport {Link.Replace("&", "\"&\"")}", "powershell.exe");
                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Painel de {ReportName} gerado com êxito.");
            }
            catch (Exception ex)
            {
                // Tratamento de erros genéricos.
                throw new Exception(@$"Não foi possivel gerar o report de {ReportName}

Erro:

{ex.Message}");


            }
            finally
            {
                // Independente do exito ou falha da geração do report,o mesmo valor na coluna GERANDO do painel em questão é atualizado para 0 pois a aplicação precisa estar desocupada para continuar a gerar novos reports.
                Odbc.Connect("ArgosAutomation", "DSN=SRVAZ31-ARGOS");
                qry = "qryUpdateGenerating.txt";
                Odbc.dtm.CleanParamters(qry);
                Odbc.dtm.ParamByName(qry, ":VALUE", "0");
                Odbc.dtm.ParamByName(qry, ":ID", $"{Id}");
                Odbc.dtm.ExecuteNonQuery(qry);
                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Atualização do estado do painel de {ReportName} atualizado com êxito.");
                //Odbc.dtm.Disconect();
            }

        }
        /// <summary>
        /// Método responsável por fazer o envio de um arquivo de foto.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        async public Task ToSend(long chatId)
        {
            try
            {
                // Obtém o arquivo .jpg resultado do método Generate.
                File = new FileStream(@"C:\Temp\ArgosAutomation\arquivos\imagens\report.jpg", FileMode.Open, FileAccess.Read);
                PrintPath = InputFile.FromStream(File);
                await Utilities.botClient.SendChatActionAsync(
                    chatId: chatId,
                    chatAction: ChatAction.UploadPhoto,
                    cancellationToken: Utilities.cts);
                await Utilities.botClient.SendPhotoAsync(chatId: chatId, photo: PrintPath, caption: Caption, disableNotification: true, parseMode: ParseMode.Markdown);
                Console.WriteLine(@$" [{DateTime.Now:dd/MM/yyyy - HH:mm:ss}] Report: Report do painel de {ReportName} enviado com êxito.");
                File.Dispose();
                File.Close();
            }
            catch (Exception ex)
            {
                // Tratamento de erros genéricos.
                throw new Exception(@$"Messagem:

Não foi possivel enviar o report de {ReportName} devido a {ex.Message}");

            }
        }
    }
}
