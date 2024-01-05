using System.Speech.Synthesis;

namespace ArgosOnDemand.Skill
{
    public class Speech
    {
        //

        public static SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        public static string Text { get; set; }


        //

        public static void ToSpeak(string text)
        {
            Text = text;
            speechSynthesizer.Rate = 2;
            speechSynthesizer.Volume = 100;

            speechSynthesizer.SpeakAsync(Text);
        }
    }
}
