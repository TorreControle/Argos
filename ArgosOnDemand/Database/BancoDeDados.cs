/*
 * Created by SharpDevelop.
 * User: t_sfukugawa.alp
 * Date: 20/06/2017
 * Time: 16:16
 * 
 * To change this SLA use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;

namespace ArgosOnDemand.Database
{
    /// <summary>
    /// Description of IniciaBancoDados.
    /// </summary>
    public class BancoDeDados
    {

        public static DataModuleMySQL dtm;

        public BancoDeDados(string nomeProjeto, string strConexao)
        {
            dtm = new DataModuleMySQL(nomeProjeto + ".DataModule");
            dtm.Conectar(strConexao);
        }

        public static void Conectar(string nomeProjeto, string strConexao)
        {
            new BancoDeDados(nomeProjeto, strConexao);
        }
    }
}
