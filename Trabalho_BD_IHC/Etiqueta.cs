using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Etiqueta
    {
        private String paisFabrico;
        private String composicao;
        private String normas;
        private int numero;

        public string PaisFabrico
        {
            get
            {
                return paisFabrico;
            }

            set
            {
                paisFabrico = value;
            }
        }

        public string Composicao
        {
            get
            {
                return composicao;
            }

            set
            {
                composicao = value;
            }
        }

        public string Normas
        {
            get
            {
                return normas;
            }

            set
            {
                normas = value;
            }
        }

        public int Numero
        {
            get
            {
                return numero;
            }

            set
            {
                numero = value;
            }
        }
    }
}
