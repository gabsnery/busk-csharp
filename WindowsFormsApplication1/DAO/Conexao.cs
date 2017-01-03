using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1.DAO
{
    class Conexao
    {

        public MySqlConnection connection; 
        public MySqlDataAdapter bdAdapter;
        public string connString = null;
      
        string serverName = "mysql5012.smarterasp.net";
        string userName;
        string databaseName ;
        string password ;
        public Conexao()
        {

            connString = "SERVER=" + serverName + ";" + "DATABASE=" +
            databaseName + ";" + "UID=" + userName + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connString);
        }
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        break;

                    case 1045:
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException )
            {

                return false;
            }

        }
        
        }
}
