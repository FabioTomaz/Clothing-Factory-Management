using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Fecho : AcessoriosCostura
    {
        private double tamanhoDente;
        private double comprimento;
        private double largura;

        public double TamanhoDente
        {
            get
            {
                return tamanhoDente;
            }

            set
            {
                tamanhoDente = value;
            }
        }

        public double Comprimento
        {
            get
            {
                return comprimento;
            }

            set
            {
                comprimento = value;
            }
        }

        public double Largura
        {
            get
            {
                return largura;
            }

            set
            {
                largura = value;
            }
        }
    }
}
