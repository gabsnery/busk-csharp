using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.POJO
{
    class PontoObj
    {
        private int         id;
        private string latitude;
        private string longitude;
        private bool selecionado;
        private int ordem;
        private int terminal;
        public PontoObj()
        {
            selecionado = false;
        }
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

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

        public bool Selecionado
        {
            get
            {
                return selecionado;
            }

            set
            {
                selecionado = value;
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

        public int Terminal
        {
            get
            {
                return terminal;
            }

            set
            {
                terminal = value;
            }
        }
    }
}
