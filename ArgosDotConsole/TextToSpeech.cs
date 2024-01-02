using Google.Cloud.TextToSpeech.V1;
using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace ArgosDot
{
    public class TextToSpeech
    {
        private static TextToSpeechClient client = TextToSpeechClient.Create();   //
        private static SoundPlayer typewriter = new SoundPlayer();
        private static VoiceSelectionParams voice = new VoiceSelectionParams
        {
            LanguageCode = "pt-BR",
            Name = "pt-BR-Standard-B",

        };  //
        private static AudioConfig audioConfig = new AudioConfig
        {
            AudioEncoding = AudioEncoding.Linear16,
            Pitch = -5.60,
        };              //
        public static int SpeechDuration { get; set; }


        //
        public static void SpeechSynthesis(string text, string path)
        {

            SynthesisInput input = new SynthesisInput
            {
                Text = text
            };

            using (var output = File.Create(path))
            {
                client.SynthesizeSpeech(input, voice, audioConfig).AudioContent.WriteTo(output);
            }

            Updates.SetResponseText("");
        }


        //
        public static async Task ToSpeak(string path)
        {
            TagLib.File file = TagLib.File.Create(path);
            typewriter.SoundLocation = path;
            SpeechDuration = (int)file.Properties.Duration.TotalSeconds;
            typewriter.Play();
            await Task.Delay(TimeSpan.FromSeconds(SpeechDuration));

        }

        //
        public static void StopSpeak()
        {
            typewriter.Stop();
        }

    }

}