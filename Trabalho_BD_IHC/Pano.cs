using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Pano : MaterialTextil
    {
        private String tipo;
        private double areaArmazem;
        private double gramagem;
        private double precoMetroQuadrado;

        public string Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
            }
        }

        public double AreaArmazem
        {
            get
            {
                return areaArmazem;
            }

            set
            {
                areaArmazem = value;
            }
        }

        public double Gramagem
        {
            get
            {
                return gramagem;
            }

            set
            {
                gramagem = value;
            }
        }

        public double PrecoMetroQuadrado
        {
            get
            {
                return precoMetroQuadrado;
            }

            set
            {
                precoMetroQuadrado = value;
            }
        }
    }
}
