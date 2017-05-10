using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class DesenhoPersonalizado
    {
        private int nDesPers;
        private DesenhoBase desenho;
        private Etiqueta etiqueta;
        private List<MaterialTextil> materialTextil;


        public int NDesPers
        {
            get
            {
                return nDesPers;
            }

            set
            {
                nDesPers = value;
            }
        }

        internal DesenhoBase Desenho
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
