using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.POJO
{
    class onibusPontoObj
    {

        private int     id_onibus;
        private int     id_pontoOnibus;
        private int    ida=0;
        private int id_ponto;
        private string latitude;
        private string longitude;
        private int ordem;

        public string Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        public string Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }

        public int Id_onibus
        {
            get
            {
                return id_onibus;
            }

            set
            {
                id_onibus = value;
            }
        }

        public int Id_pontoOnibus
        {
            get
            {
                return id_pontoOnibus;
            }

            set
            {
                id_pontoOnibus = value;
            }
        }

        public int Ida
        {
            get
            {
                return ida;
            }

            set
            {
                ida = value;
            }
        }

        public int Id_ponto
        {
            get
            {
                return id_ponto;
            }

            set
            {
                id_ponto = value;
            }
        }

        public int Ordem
        {
            get
            {
                return ordem;
            }

            set
            {
                ordem = value;
            }
        }
    }
}
