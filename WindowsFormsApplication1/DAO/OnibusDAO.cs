using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using WindowsFormsApplication1.POJO;
using System.Data;
using System.Text;
using System.Xml.Linq;

using System.Windows.Forms;

namespace WindowsFormsApplication1.DAO
{
    class OnibusDAO
    {


       
        public int adicionaOnibus(String nome, String linha, String letra)
     {
            int retorno = 0;
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("insert into onibus (nome,linha,letra) values(@nome,@linha,@letra);", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("nome", nome.ToString()));
                cmd.Parameters.Add(new MySqlParameter("linha", linha.ToString()));
                cmd.Parameters.Add(new MySqlParameter("letra", letra.ToString()));
                // cmd.Parameters.Add(new NpgsqlParameter("ultimoPonto", "false"));
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("SELECT max(id_onibus) FROM onibus;", conn.connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    retorno = Convert.ToInt16(dataReader[0]);

                }

                dataReader.Close();

                //close Connection
                conn.CloseConnection();
            }
                return retorno;
            
        }
        public int atualizaOnibus(int id,String nome, String linha, String letra)
        {
            int retorno = 0;
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE onibus SET nome=@nome,linha=@linha,letra=@letra where Id_onibus=@id;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("nome", nome.ToString()));
                cmd.Parameters.Add(new MySqlParameter("linha", linha.ToString()));
                cmd.Parameters.Add(new MySqlParameter("letra", letra.ToString()));
                cmd.Parameters.Add(new MySqlParameter("id", id));
                // cmd.Parameters.Add(new NpgsqlParameter("ultimoPonto", "false"));
                cmd.ExecuteNonQuery();

                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    retorno = Convert.ToInt16(dataReader[0]);

                }

                dataReader.Close();

                //close Connection
                conn.CloseConnection();
            }
            return retorno;

        }
        public List<OnibusObj> listaPontoDAO()
    {
            OnibusObj pont;
            Conexao conn = new Conexao();
            List<OnibusObj> listaPontos = new List<OnibusObj>();
            if (conn.OpenConnection() == true)
            {

                MySqlCommand cmd = new MySqlCommand("Select  Id_onibus, nome, linha,letra from onibus where Id_onibus!=-1;", conn.connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    pont = new OnibusObj();
                            pont.Id_onibus1 = Convert.ToInt16(dataReader["Id_onibus"]);
                            pont.Letra = dataReader["letra"].ToString();
                            pont.Linha = dataReader["linha"].ToString();
                            pont.Nome= dataReader["nome"].ToString();

                    listaPontos.Add(pont);
                    }
                //close Data Reader
                dataReader.Close();

                //close Connection
                conn.CloseConnection();
            }
            
            return listaPontos;
        }
        public void ExcluirOnibus(Double id)
        {
            Conexao conn = new Conexao();
            if (conn.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand("delete from onibus where Id_onibus=@id;", conn.connection);
                cmd.Parameters.Add(new MySqlParameter("id", id.ToString()));
                cmd.ExecuteNonQuery();
                conn.CloseConnection();
            }

        }
    }
}
