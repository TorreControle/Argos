using Google.Cloud.Speech.V1;
using static Google.Cloud.Speech.V1.SpeechClient;

namespace ArgosDot.skills
{
    public class SpeechToText
    {
        // Cria a instância para o uso da API.
        private static SpeechClient speechClient = Create();

        // Substitua pelo formato do áudio que está sendo enviado (por exemplo, Linear16 para PCM de 16 bits).
        private RecognitionConfig.Types.AudioEncoding audioEncoding = RecognitionConfig.Types.AudioEncoding.Linear16;

        // Configura o fluxo de áudio
        private StreamingRecognizeStream streamingCall = speechClient.StreamingRecognize();

    }
}
