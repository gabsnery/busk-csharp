using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.POJO
{
    class OnibusObj
    {

        private int     Id_onibus;
        private string nome;
        private string linha;
        private string letra;

        public OnibusObj()
        {
            this.linha = "A";
            
        }

        public int Id_onibus1
        {
            get
            {
                return Id_onibus;
            }

            set
            {
                Id_onibus = value;
            }
        }

        public string Nome
        {
            get
            {
                return nome;
            }

            set
            {
                nome = value;
            }
        }

        public string Linha
        {
            get
            {
                return linha;
            }

            set
            {
                linha = value;
            }
        }

        public string Letra
        {
            get
            {
                return letra;
            }

            set
            {
                letra = value;
            }
        }
    }
}





