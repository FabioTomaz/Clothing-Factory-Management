using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class ProdutoPersonalizado
    {
        private ProdutoBase produtoBase;
        private String tamanho;
        private String cor;
        private int unidadesStock;
        private double preco;
        private Modelo modelo;
        private Desenho desenho;

        internal ProdutoBase ProdutoBase
        {
            get
            {
                return produtoBase;
            }

            set
            {
                produtoBase = value;
            }
        }

        public string Tamanho
        {
            get
            {
                return tamanho;
            }

            set
            {
                tamanho = value;
            }
        }

        public string Cor
        {
            get
            {
                return cor;
            }

            set
            {
                cor = value;
            }
        }

        public int UnidadesStock
        {
            get
            {
                return unidadesStock;
            }

            set
            {
                unidadesStock = value;
            }
        }

        public double Preco
        {
            get
            {
                return preco;
            }

            set
            {
                preco = value;
            }
        }

        internal Modelo Modelo
        {
            get
            {
                return modelo;
            }

            set
            {
                modelo = value;
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
