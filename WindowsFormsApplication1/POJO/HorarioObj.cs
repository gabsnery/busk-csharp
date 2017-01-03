using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.POJO
{
    class HorarioObj
    {
        private int id_horario;
        private int id_onibus;
        private string horario;
        private int dia;

        public int Id_horario
        {
            get
            {
                return id_horario;
            }

            set
            {
                id_horario = value;
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

        public string Horario
        {
            get
            {
                return horario;
            }

            set
            {
                horario = value;
            }
        }

        public int Dia
        {
            get
            {
                return dia;
            }

            set
            {
                dia = value;
            }
        }
    }
}
