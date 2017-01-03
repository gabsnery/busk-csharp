using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using WindowsFormsApplication1.POJO;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1.DAO
{
    class HorarioDAO
    {

        public void adicionaHorarioDAO(String hora, int id_onibus,int dia)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("insert into horario (horario,id_onibus,dia) values(@horario,@id_onibus,@dia);", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("horario", hora));
                cmd.Parameters.Add(new MySqlParameter("id_onibus", id_onibus));
                cmd.Parameters.Add(new MySqlParameter("dia", dia));
                // cmd.Parameters.Add(new MySqlParameter("ultimoPonto", "false"));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }
        }
        public void deletaHorario(int horario)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM horario WHERE id_horario=@latitude;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("latitude", horario));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
        public List<HorarioObj> listaPontoDAO(int id)
        {
            HorarioObj horaObj;
            Conexao conn = new Conexao();
            List<HorarioObj> listaPontos = new List<HorarioObj>();
            if (conn.OpenConnection() == true)
            {
               
                   
                    string cmdSeleciona = "Select  id_horario, horario, id_onibus,dia from horario where id_onibus=@id;";
               
                MySqlCommand cmd = new MySqlCommand(cmdSeleciona, conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id));
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    horaObj = new HorarioObj();
                            horaObj.Id_horario = Convert.ToInt16(dataReader["id_horario"]);
                            horaObj.Horario= dataReader["horario"].ToString();
                            horaObj.Id_onibus = Convert.ToInt16(dataReader["id_onibus"]);
                    horaObj.Dia = Convert.ToInt16(dataReader["dia"]);
                    listaPontos.Add(horaObj);
                        }
                    }
            return listaPontos;
        }

    }
}
