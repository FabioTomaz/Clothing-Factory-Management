using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class ProdutoBase
    {
        private int referencia;
        private String nome;
        private double IVA;
        private Desenho desenho;

        public int Referencia
        {
            get
            {
                return referencia;
            }

            set
            {
                referencia = value;
            }
        }

        public string Nome
        {
            get
            {
                return nome;
            }

            set
            {
                nome = value;
            }
        }

        public double IVA1
        {
            get
            {
                return IVA;
            }

            set
            {
                IVA = value;
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
    }
}
