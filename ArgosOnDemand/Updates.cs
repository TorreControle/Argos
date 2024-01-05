/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

using ArgosOnDemand.Commands;
using ArgosOnDemand.Database;
using ArgosOnDemand.Skill;
using System.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bots.Types;
using Conections = ArgosOnDemand.Utilities.Conections;

namespace ArgosOnDemand
{
    public class Updates
    {
        // Propriedades

        static public long userId { get; set; }
        static public long chatId { get; set; }
        static public string? chatTitle { get; set; }
        static public DateTime messageDate { get; set; }
        static public int messageId { get; set; }
        static public string? messageText { get; set; }
        static public int updateId { get; set; }
        static public string? firstName { get; set; }
        static public string? lastName { get; set; }
        static public string? userName { get; set; }


        // Manipulador de atualizações do bot.

        async public static Task Handler(ITelegramBotClient botClient, Telegram.Bot.Types.Update Update, CancellationToken CancellationToken)
        {
            // Atribuindo valores.

            userId = Update.Message.From.Id;
            chatId = Update.Message.Chat.Id;
            chatTitle = Update.Message.Chat.Title;
            messageDate = Update.Message.Date.ToLocalTime();
            messageId = Update.Message.MessageId;
            messageText = Update.Message.Text;
            updateId = Update.Id;
            firstName = Update.Message.From.FirstName;
            lastName = Update.Message.From.LastName;
            userName = Update.Message.From.Username;


            // Monitoramento de atualizações do bot. 

            await Monitoring.SendLog();


            // Orientação a banco de dados de comandos.

            BancoDeDadosODBC.Conectar("ArgosOnDemand", Conections.DataSources.MariaDB);
            string qryTabComandos = "qryTabComandos.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryTabComandos);
            DataTable dtTabComandos = BancoDeDadosODBC.dtm.ExecuteQuery(qryTabComandos);


            // Adiciona os dados recuperados da tabela t_comandos_argos e grava na TabComandosList.

            List<string?> commandsList = new();

            for (int i = 0; i <= dtTabComandos.Rows.Count - 1; i++)
            {
                commandsList.Add(dtTabComandos.Rows[i]["comando"].ToString());
            }


            // Verificação de usuários

            string qryPermissoes = "qryPermissoes.txt";
            BancoDeDadosODBC.dtm.Limpa_Parametros(qryPermissoes);
            BancoDeDadosODBC.dtm.ParamByName(qryPermissoes, ":CHATID", chatId.ToString());
            DataTable dtPermissoes = BancoDeDadosODBC.dtm.ExecuteQuery(qryPermissoes);
            DataRow Permissoes = dtPermissoes.Rows[0];


            // Verificação de usuários vinculada ao banco de dados.

            if (Permissoes["ID"].ToString() == chatId.ToString())
            {
                // Se receber um Update do tipo texto.


                if (Update.Message.Type == MessageType.Text)
                {
                    // Tratamento de caracteres de texto.

                    string text = Tools.TextProcessing(messageText, alphas: true, numerics: true, hashtag: true, atsing: true, twopoints: true);


                    //

                    if (messageText.Contains('#'))
                    {
                        text = text[..text.IndexOf("#")];
                    }
                    else if (messageText.Contains('@'))
                    {
                        text = text[..text.IndexOf("@")];
                    }
                    else if (messageText.Contains(':'))
                    {
                        text = text[..text.IndexOf(":")];
                    }                    


                    // Verifica se a mensagem de texto enviada é um comando ou não.

                    if (commandsList.Contains(text))
                    {
                        // Se for um comando, faz uma consulta na t_comandos_argos retornando a respectiva linha ao comando.

                        string qryComandos = "qryComandos.txt";
                        BancoDeDadosODBC.dtm.Limpa_Parametros(qryComandos);
                        BancoDeDadosODBC.dtm.ParamByName(qryComandos, ":MESSAGETEXT", text);
                        DataTable dtComandos = BancoDeDadosODBC.dtm.ExecuteQuery(qryComandos);


                        // Se linha respectiva ao comando e na coluna "TIPO_COMANDO" for Mensagem.

                        if (dtComandos.Rows[0]["TIPO_COMANDO"].ToString() == "Mensagem")
                        {
                            // Comando de teste

                            if (text == "teste")
                            {
                                CommandHandler teste = new(new Teste());
                                await teste.Answer();
                            }

                        }


                        // Se linha respectiva ao comando e na coluna "TIPO_COMANDO" for Consulta.

                        else if (dtComandos.Rows[0]["TIPO_COMANDO"].ToString() == "Consulta")
                        {
                            // Consulta de pedidos em doca por unidade.

                            if (text.Contains("argospedidosemdocaporunidade"))
                            {
                                CommandHandler pedidosDoca = new(new PedidosEmDoca());
                                await pedidosDoca.Answer();
                            }


                            // Consulta de pedidos em doca passando uma unidade como parâmetro.

                            if (text.Contains("argospedidosemdocano"))
                            {
                                CommandHandler pedidosDocaUnidade = new(new PedidosEmDocaUnidade());
                                await pedidosDocaUnidade.Answer();
                            }


                            // Consulta de pedidos.

                            if (text.Contains("argoscomoestopedido"))
                            {
                                CommandHandler statusPedido = new(new StatusPedido());
                                await statusPedido.Answer();
                            }


                            // Consulta de CESV.

                            if (text.Contains("argosconsulteacesv"))
                            {
                                CommandHandler consultaCESV = new(new ConsultaCESV());
                                await consultaCESV.Answer();
                            }


                            // Consulta de CTE.

                            if (text.Contains("argosconsulteocte"))
                            {
                                CommandHandler consultaCTE = new(new ConsultaCTE());
                                await consultaCTE.Answer();
                            }


                            // Comando dinâmico

                            if (text.Contains("argosquantos"))
                            {
                                CommandHandler comandoDinamico = new(new ComandoDinamico());
                                await comandoDinamico.Answer();
                            }


                            // Gerador de senha para motoristas

                            if (text.Contains("argosgerarsenhadeliberao"))
                            {
                                await Send.Text(chatId, "Positivo! Informe seu CPF no formato abaixo para a validação de cadastro do motorista: \n\n CPF: 00000000000");
                            }


                            //

                            if (text.StartsWith("cpf"))
                            {
                                CommandHandler geradorSenhaMotorista = new(new GeradorSenhaMotorista());
                                await geradorSenhaMotorista.Answer();
                            }

                        }


                        // Se linha respectiva ao comando e na coluna "TIPO_COMANDO" for Mensagem.

                        if (dtComandos.Rows[0]["TIPO_COMANDO"].ToString() == "Análise")
                        {
                            // Comando de teste

                            if (text.StartsWith("argosanaliseademandado"))
                            {
                                CommandHandler analiseDemandaCD = new(new AnaliseDemandaCD());
                                await analiseDemandaCD.Answer();
                            }


                            if (text.StartsWith("argosqualoperfildopedido"))
                            {
                                CommandHandler perfilPedido = new(new PerfilPedido());
                                await perfilPedido.Answer();
                            }


                            //

                            if (text.StartsWith("argosgerar"))
                            {
                                CommandHandler geradorQry = new(new GeradorQry());
                                await geradorQry.Answer();
                            }

                        }


                        // Se linha respectiva ao comando e na coluna "TIPO_COMANDO" for Mensagem.

                        if (dtComandos.Rows[0]["TIPO_COMANDO"].ToString() == "Criação")
                        {
                            //

                            if (text.StartsWith("argoscrie"))
                            {
                                CommandHandler imageGenerator = new(new ImageGenerator());
                                await imageGenerator.Answer();
                            }

                        }


                        // Para tipos de comando que não estão declarados na tabela.

                        else
                        {
                            return;
                        }

                    }


                    // Para mensegens recebidas que não são comandos. 

                    else
                    {
                        if (text.StartsWith("argos"))
                        {
                            if (text == "argos") 
                            {
                                return;
                            }
                            else
                            {
                                string txt = messageText.Substring(7);
                                var resposta = await Tools.ChatGPT(txt);
                                await Send.Text(chatId, resposta, replyToMessageId: messageId);
                            }
                        }
                    }
                }

                // Senão

                else
                {
                    return;
                }
            }


            // Mensagem de bloqueio de usuários não permitidos a falar com o bot.

            else
            {
                await Send.Text(chatId, "Me desculpe mas você *não* tem permissão para falar comigo \U0001F6AB entre em contato com a Torre de Controle e solicite acesso, seu ID é: " + chatId + ".");
            }


            //

            BancoDeDadosODBC.dtm.Desconectar();
        }
    }

}