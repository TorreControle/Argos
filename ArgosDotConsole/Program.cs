using ArgosDot.commands;
using ArgosDot.skills;
using Google.Cloud.Speech.V1;
using Microsoft.Speech.Recognition;
using NAudio.Wave;
using System;
using System.Globalization;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
namespace ArgosDot
{
    class Program
    {
        // Indica se o reconhecimento assíncrono está completo.  
        static bool completed;


        // Campo dedicado a criação e manipulação das opções dentro de uma gramática
        private static VoiceTrigger s_trigger;


        // Campo usado para manupulação de arquivos WAV.
        private static SoundPlayer typewriter = new SoundPlayer();


        static void Main()
        {
            // Cria o reconhecedor de fala já atribuindo o idioma "Português - Brasileiro".
            Recognizer.s_recognizer = new SpeechRecognitionEngine(new CultureInfo("pt-br"));


            // Cria um gatilho de voz para reconhecimento de fala
            s_trigger = new VoiceTrigger();


            #region Trecho do código onde se carregam as gramáticas para o reconhecimento.

            // Gramática de ativação gravador de áudio.
            Grammar activationWord = s_trigger.GetChooses(Utilities.Directory.Grammar.ActivationWord, "Palavras de ativação");
            Recognizer.s_recognizer.LoadGrammarAsync(activationWord);

            #endregion


            #region Bloco de código onde os eventos da aplicação são ativados

            // Attach event handlers.  
            Recognizer.s_recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(SpeechDetectedHandler);
            Recognizer.s_recognizer.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(SpeechHypothesizedHandler);
            Recognizer.s_recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizedHandlerAsync);
            Recognizer.s_recognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(RecognizeCompletedHandler);
            Recognizer.s_recognizer.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(RecognizeAudioStateHandler);

            #endregion


            // Atribui entrada ao reconhecedor e inicia um assíncrono e inicia o reconhecimento de fala.
            Recognizer.s_recognizer.SetInputToDefaultAudioDevice();
            Recognizer.s_recognizer.RecognizeAsync(RecognizeMode.Multiple);

            //Iniciando reconhecimento de fala assincrono
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($" __________________________________________________________________________________\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"                                 Argos Dot\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"      Módulo de conversação utilizando processamento de linguagem natural.\n");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($@" Usuário: {Environment.UserName}
 Máquina: {Environment.MachineName}
 Domínio: {Environment.UserDomainName}
 CLR Version: {Environment.Version}
 Versão do sistema: {Environment.OSVersion}
 Diretório: {Environment.CurrentDirectory}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($" __________________________________________________________________________________\n\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" Olá! Eu sou o Argos a IA da Multilog S.A, vamos conversar?\n");

            // Aguarda a conclusão da operação de reconhecimento de fala, enquanto completed for igual a false a aplicação continua aguardando.
            completed = false;
            while (!completed)
            {
                Thread.Sleep(333);
            }

        }

        // Evento manipulador do reconhecimento da fala, é ativado sempre que o reconhecedor detecta uma fala declarada na gramática.
        static async void SpeechRecognizedHandlerAsync(object sender, SpeechRecognizedEventArgs e)
        {
            // Verifica o resultado do reconhecimento.
            if (!e.Result.Equals(null))
            {
                //
                Updates.SetRecognizedText(e.Result.Text);
                Updates.SetConfidenceText(e.Result.Confidence);
                Updates.SetTranscribeText("N/A");
                Updates.SetResponseText("N/A");
                Updates.GrammarName = e.Result.Grammar.Name;


                //
                if (Updates.GetRecognizedText().Equals("argos") && Updates.GetConfidenceText() >= 0.60)
                {
                    Recognizer.s_recognizer.RecognizeAsyncStop();
                    await TextToSpeech.ToSpeak(Utilities.Directory.Audio.ConfirmationSign);

                    // Substitua pelo formato do áudio que está sendo enviado (por exemplo, Linear16 para PCM de 16 bits).
                    RecognitionConfig.Types.AudioEncoding audioEncoding = RecognitionConfig.Types.AudioEncoding.Linear16;

                    // Cria a instância para o uso da API.
                    var speechClient = SpeechClient.Create();

                    // Configura o fluxo de áudio
                    var streamingCall = speechClient.StreamingRecognize();

                    // Configura o áudio inicial para iniciar o fluxo de reconhecimento
                    await streamingCall.WriteAsync(new StreamingRecognizeRequest
                    {
                        StreamingConfig = new StreamingRecognitionConfig
                        {
                            Config = new RecognitionConfig
                            {
                                Encoding = audioEncoding,
                                SampleRateHertz = 16000,
                                LanguageCode = "pt-BR",
                            },
                            InterimResults = true,
                        },
                    });

                    // Captura o áudio do microfone
                    var waveIn = new WaveInEvent();
                    waveIn.WaveFormat = new WaveFormat(16000, 1); // Frequência de amostragem de 16000Hz, canal mono
                    waveIn.DataAvailable += async (s, es) =>
                    {
                        if (es.BytesRecorded > 0)
                        {
                            await streamingCall.WriteAsync(new StreamingRecognizeRequest
                            {
                                AudioContent = Google.Protobuf.ByteString.CopyFrom(es.Buffer, 0, es.BytesRecorded),
                            });
                        }
                    };
                    waveIn.StartRecording();


                    // Aguarda o término da captura de áudio (você pode definir um tempo limite ou condição de parada)
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    // Para a captura do áudio e finaliza o fluxo de reconhecimento
                    waveIn.StopRecording();
                    await streamingCall.WriteCompleteAsync();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine(" Processamento de linguagem natural ativado.");

                    // Recebe as respostas da API e imprime o texto reconhecido
                    var responseStream = streamingCall.GetResponseStream();
                    while (await responseStream.MoveNextAsync())
                    {
                        foreach (var result in responseStream.Current.Results)
                        {
                            foreach (var alternative in result.Alternatives)
                            {
                                Updates.SetTranscribeText(alternative.Transcript.ToLower());
                            }
                        }
                    }

                    if (Updates.GetTranscribeText().Contains("pedido") && Updates.GetTranscribeText().Contains("aberto"))
                    {
                        CommandHandler command = new CommandHandler(new PedidosAbertos());
                        await command.Start();
                    }
                    else if (Updates.GetTranscribeText().Equals("testando"))
                    {
                        CommandHandler command = new CommandHandler(new Teste());
                        await command.Start();
                    }
                    else if (Updates.GetTranscribeText().Equals("N/A"))
                    {
                        CommandHandler command = new CommandHandler(new Empty());
                        await command.Start();
                    }
                    else
                    {
                        CommandHandler command = new CommandHandler(new Conversation());
                        await command.Start();
                    }

                }

            }

        }

        // Evento manipulador da detecção de uma fala, é ativado sempre que o reconhecedor detecta vozes.
        static void SpeechDetectedHandler(object sender, SpeechDetectedEventArgs e)
        {
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine($"\n Fala detectada");
            //Console.ForegroundColor = ConsoleColor.White;

        }

        // Evento manipulador de hipóteses de uma fala, não necessariamente é o reconhecimento já feito, mas sim são as hipóteses conforme o reconhecimento é processado.
        static void SpeechHypothesizedHandler(object sender, SpeechHypothesizedEventArgs e)
        {


        }

        // Evento manipulador de um reconhecimento da fala completo, é ativado sempre que o reconhecedor termina o processo de reconhecimento através dos métodos RecognizeAsyncCancel() e o RecognizeAsyncStop().
        static void RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
            //completed = true;
        }

        // Evento manipulador do estado do reconhecedor, é ativado sempre que o reconhecedor muda o seu estado entre "Speech"(Falando), "Stopped(Parou)", "Silence(Silêncio)".
        static void RecognizeAudioStateHandler(object sender, AudioStateChangedEventArgs e)
        {

            //           Console.ForegroundColor = ConsoleColor.Gray;
            //           Console.WriteLine($@"
            //Status: {e.AudioState}");



        }

    }

}