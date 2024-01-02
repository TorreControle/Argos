using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArgosDot.commands
{
    public class Empty : ICommand
    {
        // 
        public List<string> KeyWordsList = new List<string>();

        //
        public string ActivatorCommand { get; set; }

        // 
        public string ResponseText { get; set; }

        // 
        public bool IsCompleted { get; set; }


        // 
        public Empty()
        {
            string[] words = { "N/A" };
            foreach (string word in words) { KeyWordsList.Add(word); }
            ActivatorCommand = Updates.GetTranscribeText();
            ResponseText = null;
            IsCompleted = false;
        }


        // 
        public async Task Run()
        {
            try
            {
                ResponseText = "";
                Updates.SetResponseText(ResponseText);
                TextToSpeech.SpeechSynthesis(Updates.GetResponseText(), Utilities.Directory.Audio.Output);

            }
            catch (Exception e)
            {

            }
            finally
            {

            }

        }

    }

}
