
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Xml.Linq;
using WindowsFormsApplication1.POJO;

namespace WindowsFormsApplication1.DAO
{
    class onibusPontoDAO
    {

        
        public void adicionaPontoOnibus(int id_ponto,  int id_onibus,int ida,int ordem)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("insert into  ponto_onibus (id_ponto,id_onibus,ida,ordem) values (@id_ponto,@id_onibus,@ida,@ordem);", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id_ponto", id_ponto.ToString()));
                cmd.Parameters.Add(new MySqlParameter("id_onibus", id_onibus.ToString()));
                cmd.Parameters.Add(new MySqlParameter("ida", ida));
                cmd.Parameters.Add(new MySqlParameter("ordem", ordem));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }
          
        }
        public void deletaPontoOnibus(int id_pontoOnibus)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM ponto_onibus WHERE id_pontoOnibus=@latitude;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("latitude", id_pontoOnibus));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
        public List<onibusPontoObj> listaPontoDAO(int id)
        {
            onibusPontoObj pont;
            Conexao conn = new Conexao();
            List<onibusPontoObj> listaPontos = new List<onibusPontoObj>();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("select p.id_ponto,	id_pontoOnibus,latitude,longitude,ida,ordem from ponto p inner join ponto_onibus po on p.id_ponto = po.id_ponto" +
                   " inner join onibus o "+
                "on po.id_onibus = o.id_onibus "+
"where o.id_onibus = @id", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    pont = new onibusPontoObj();
                    pont.Id_ponto = Convert.ToInt16(dataReader[0]);
                    pont.Latitude= dataReader["latitude"].ToString();
                    pont.Id_pontoOnibus = Convert.ToInt16(dataReader["id_pontoOnibus"]); 
                    pont.Ordem = Convert.ToInt16(dataReader["ordem"]);
                    pont.Longitude = dataReader["longitude"].ToString();
                    pont.Ida = Convert.ToInt16(dataReader["ida"]);

                    listaPontos.Add(pont);
                }
                dataReader.Close();
                conn.CloseConnection();
            }
            return listaPontos;
        }
    }
}






  
