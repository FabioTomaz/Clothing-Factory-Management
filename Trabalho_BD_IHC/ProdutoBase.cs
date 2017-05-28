using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Trabalho_BD_IHC
{
    public class ProdutoBase
    {
        private int referencia;
        private String nome;
        private double IVA;
        private DateTime dataAlteraçao;
        private String instrProd;
        private Utilizador gestorProducao;
        private byte[] pic;

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

        public DateTime DataAlteraçao
        {
            get
            {
                return dataAlteraçao;
            }

            set
            {
                dataAlteraçao = value;
            }
        }

        public string InstrProd
        {
            get
            {
                return instrProd;
            }

            set
            {
                instrProd = value;
            }
        }

        public Utilizador GestorProducao
        {
            get
            {
                return gestorProducao;
            }

            set
            {
                gestorProducao = value;
            }
        }

        public byte[] Pic
        {
            get
            {
                return pic;
            }

            set
            {
                pic = value;
            }
        }
    }
}
