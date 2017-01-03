using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.POJO;

namespace WindowsFormsApplication1.DAO
{
    class areaDAO
    {


        public void adicionaPonto(Double latitude, Double longitude)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("insert into area (latitude,longitude) values(@latitude,@longitude);", conn.connection);
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
                MySqlCommand cmd = new MySqlCommand("delete from area where id=@id;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id.ToString()));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
        public List<areaObj> listaPontoDAO()
        {
            areaObj pont;
            Conexao conn = new Conexao();
            List<areaObj> listaPontos = new List<areaObj>();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("Select  id, latitude, longitude from area;", conn.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    pont = new areaObj();
                    pont.Id = Convert.ToInt16(dataReader["id"]);
                    pont.Latitude = dataReader["latitude"].ToString();
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
