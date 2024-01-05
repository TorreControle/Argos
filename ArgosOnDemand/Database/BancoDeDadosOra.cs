/*
 * Created by SharpDevelop.
 * User: p008019788
 * Date: 05/09/2017
 * Time: 11:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace ArgosOnDemand.Database
{
    /// <summary>
    /// Description of BancoDeDadosOra.
    /// </summary>
    public class BancoDeDadosOra
    {
        public static DataModuleOleDb dtm;

        public BancoDeDadosOra(string nomeProjeto, string strConexao)
        {
            dtm = new DataModuleOleDb(nomeProjeto + ".DataModule");
            dtm.Conectar(strConexao);

        }

        public static void Conectar(string nomeProjeto, string strConexao)
        {
            new BancoDeDadosOra(nomeProjeto, strConexao);
        }
    }
}
