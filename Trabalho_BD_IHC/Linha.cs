using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Linha : MaterialTextil
    {
        private double grossura;
        private double comprimentoStock;
        private double preco100Metros;

        public double Grossura
        {
            get
            {
                return grossura;
            }

            set
            {
                grossura = value;
            }
        }



        public double ComprimentoStock
        {
            get
            {
                return comprimentoStock;
            }

            set
            {
                comprimentoStock = value;
            }
        }

        public double Preco100Metros
        {
            get
            {
                return preco100Metros;
            }

            set
            {
                preco100Metros = value;
            }
        }
    }
}
