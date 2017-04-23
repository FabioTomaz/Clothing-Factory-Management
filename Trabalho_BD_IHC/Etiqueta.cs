using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Etiqueta
    {
        private Desenho desenho;
        private String paisFabrico;
        private String composicao;
        private String normas;

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
    }
}
