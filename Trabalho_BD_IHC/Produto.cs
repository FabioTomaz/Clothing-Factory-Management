using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Produto
    {
        private int referencia;
        private String nome;
        private double IVA;
        private Desenho desenho;
        private String tamanho;
        private String cor;
        private int unidadesStock;
        private double preco;
        private Modelo modelo;

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
    }
}
