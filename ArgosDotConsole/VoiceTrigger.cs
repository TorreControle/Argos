using Microsoft.Speech.Recognition;


namespace ArgosDot
{
    public class VoiceTrigger
    {
        // Campo para manipulação de objetos do tipo Grammar.

        private static Grammar _grammar;

        // 
        public VoiceTrigger()
        {

        }

        // Método para obter as possíveis escolhas de reconhecimento de fala.
        public Grammar GetChooses(string path, string setName)
        {
            _grammar = new Grammar(path)
            {
                Name = setName
            };
            return _grammar;
        }

    }

}
