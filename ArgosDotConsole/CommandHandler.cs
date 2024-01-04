using ArgosDot.skills;
using Microsoft.Speech.Recognition;
using System;
using System.Threading.Tasks;

namespace ArgosDot
{
    public class CommandHandler
    {
        //
        private readonly ICommand _command;
        public string ActivatorCommand { get; set; }
        public string ResponseText { get; set; }
        public bool IsCompleted { get; set; }

        public CommandHandler(ICommand command)
        {
            _command = command;
            ActivatorCommand = command.ActivatorCommand;
            ResponseText = command.ResponseText;
            IsCompleted = command.IsCompleted;
        }

        public async Task Start()
        {
            try
            {
                //
                await _command.Run();

                //
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($@" Prompt do usuário: {ActivatorCommand}");
                Tools.GenerateLog($@"{DateTime.Now} - Prompt: {ActivatorCommand}", Utilities.Folders.Log + "log.txt");

                //
                await TextToSpeech.ToSpeak(Utilities.Directory.Audio.Output);
            }
            catch (Exception)
            {

            }
            finally
            {
                //
                Recognizer.s_recognizer.RecognizeAsync(RecognizeMode.Multiple);

                //
                ResponseText = Updates.GetResponseText();
                IsCompleted = true;

                //
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" Resposta: {ResponseText}\n");
                Tools.GenerateLog($@"{DateTime.Now} - Resposta: {ResponseText}
_______________________________________________________________________________________________________________________________
", Utilities.Folders.Log + "log.txt");

            }

        }

    }

}

