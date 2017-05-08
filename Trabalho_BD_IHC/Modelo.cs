using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Modelo
    {
        private int nModelo;
        private Desenho desenho;
        private Etiqueta etiqueta;
        private List<MaterialTextil> materialTextil;


        public int NModelo
        {
            get
            {
                return nModelo;
            }

            set
            {
                nModelo = value;
            }
        }

        internal Desenho Desenho
        {
            get
            {
                return desenho;
            }

            set
            {
                desenho = value;
            }
        }

        internal Etiqueta Etiqueta
        {
            get
            {
                return etiqueta;
            }

            set
            {
                etiqueta = value;
            }
        }

        public List<MaterialTextil> MaterialTextil
        {
            get
            {
                return materialTextil;
            }

            set
            {
                materialTextil = value;
            }
        }
    }
}
