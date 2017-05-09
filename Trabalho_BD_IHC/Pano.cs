using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Pano : MaterialTextil
    {
        private String tipo;
        private double areaArmazem;
        private int gramagem;
        private double precoMetroQuadrado;

        public Pano() : base()
        {

        }

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

        public int Gramagem
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
