using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.DAO
{
    class logar
    {


        public bool veLogin(String login)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {


                string cmdSeleciona = "Select login from login where login='" + login + "'";
                MySqlCommand cmd = new MySqlCommand(cmdSeleciona, conn.connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();


                if (dataReader.Read())
                {
                    conn.CloseConnection();
                    return true;

                }
               
            }
            conn.CloseConnection();
            return false;
        }
        public bool veSenha(String login, String senha)
        {
          




                Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {


                string cmdSeleciona = "Select senha from login where login='" + login + "' AND senha='"+senha+"'";
                MySqlCommand cmd = new MySqlCommand(cmdSeleciona, conn.connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();


                if (dataReader.Read())
                {
                    conn.CloseConnection();
                    return true;

                }

            }
            conn.CloseConnection();
            return false;
        }
    }
}
