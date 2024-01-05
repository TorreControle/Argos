/*
 * Created by SharpDevelop.
 * User: p008019788
 * Date: 05/09/2017
 * Time: 11:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ArgosOnDemand.Database
{
    /// <summary>
    /// Description of BancoDeDadosOra.
    /// </summary>
    public class BancoDeDadosOraSul
    {
        public static DataModuleOleDb dtm;

        public BancoDeDadosOraSul(string nomeProjeto, string strConexaoOraSul)
        {
            dtm = new DataModuleOleDb(nomeProjeto + ".DataModule");
            dtm.Conectar(strConexaoOraSul);
        }

        public static void Conectar(string nomeProjeto, string strConexaoOraSul)
        {
            new BancoDeDadosOraSul(nomeProjeto, strConexaoOraSul);
        }
    }
}
