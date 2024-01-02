namespace ArgosDot
{
    public class Updates
    {
        private static string RecognizedText { get; set; }   // Texto reconhecido pelo Microsoft.Speech
        private static float Confidence { get; set; }        // Taxa de confiança do reconhecimento de fala.
        private static string TranscribeText { get; set; }   // Texto transcrito da API
        private static string ResponseText { get; set; }     // Resposta da aplicação.
        public static string GrammarName { get; set; }       // Nome da gramática.


        // Esse método obtém o valor da propriedade RecognizedText em qualquer ponto da aplicação.
        public static string GetRecognizedText()
        {
            return RecognizedText;
        }


        // Esse método atribui o valor a propriedade RecognizedText em qualquer ponto da aplicação.
        public static void SetRecognizedText(string text)
        {
            RecognizedText = text;
        }


        // Esse método obtém o valor da propriedade Confidence em qualquer ponto da aplicação.
        public static float GetConfidenceText()
        {
            return Confidence;
        }


        // Esse método atribui o valor a propriedade Confidence em qualquer ponto da aplicação.
        public static void SetConfidenceText(float value)
        {
            Confidence = value;
        }


        // Esse método obtém o valor da propriedade TranscribeText em qualquer ponto da aplicação.
        public static string GetTranscribeText()
        {
            return TranscribeText;
        }


        // Esse método atribui o valor a propriedade TranscribeText em qualquer ponto da aplicação.
        public static void SetTranscribeText(string text)
        {
            TranscribeText = text;
        }


        // Esse método obtém o valor da propriedade ResponseText em qualquer ponto da aplicação.
        public static string GetResponseText()
        {
            return ResponseText;
        }


        // Esse método atribui o valor a propriedade ResponseText em qualquer ponto da aplicação.
        public static void SetResponseText(string text)
        {
            ResponseText = text;
        }


    }

}
