/*
Argos - Sistema Especialista Torre de Controle

Data de criação: 14/07/2022
Data de produção: 
Desenvolvedores: Willian Renato Lima da Silva, Email: willian.silva@multilog.com.br
                 Jéssica Akemi Yamamoto Saldanha, Email: jessica.yamamoto@multilog.com.br
*/

namespace ArgosOnDemand.Database
{
    public class BancoDeDadosODBC
    {

        public static DataModuleODBC dtm;

        public BancoDeDadosODBC(string nomeProjeto, string strConexao)
        {
            dtm = new DataModuleODBC(nomeProjeto + ".DataModule");
            dtm.Conectar(strConexao);
        }

        public static void Conectar(string nomeProjeto, string strConexao)
        {
            new BancoDeDadosODBC(nomeProjeto, strConexao);
        }

    }

}
