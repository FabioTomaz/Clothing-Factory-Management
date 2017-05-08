using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Elastico : AcessoriosCostura
    {
        private double largura;
        private double comprimento;

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
    }
}
