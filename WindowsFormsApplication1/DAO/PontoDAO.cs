
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Xml.Linq;
using WindowsFormsApplication1.POJO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WindowsFormsApplication1.DAO
{
    class PontoDAO
    {

        
      
        public void adicionaPonto(Double latitude, Double longitude)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("insert into ponto (latitude,longitude) values(@latitude,@longitude);",conn.connection);
                cmd.Parameters.Add(new MySqlParameter("latitude", latitude.ToString()));
                cmd.Parameters.Add(new MySqlParameter("longitude", longitude.ToString()));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }
         
        }
        public void ExcluirPonto(Double id)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("delete from ponto where id_ponto=@id;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id.ToString()));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
        public void updatePontoTerminal(Double id,int terminal)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE ponto SET terminal=@terminal WHERE id_ponto=@id;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id.ToString()));
                cmd.Parameters.Add(new MySqlParameter("terminal", terminal));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
        public List<PontoObj> listaPontoDAO()
        {
            PontoObj pont;
            Conexao conn = new Conexao();
            List<PontoObj> listaPontos = new List<PontoObj>();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("Select  terminal,id_ponto, latitude, longitude from ponto;", conn.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    pont = new PontoObj();
                    pont.Id = Convert.ToInt16(dataReader["id_ponto"]);
                    pont.Latitude = dataReader["latitude"].ToString();
                    pont.Terminal = Convert.ToInt16(dataReader["terminal"].ToString());
                    pont.Longitude = dataReader["longitude"].ToString();

                    listaPontos.Add(pont);
                }
                dataReader.Close();
                conn.CloseConnection();
            }
            return listaPontos;
        }


    }
}
