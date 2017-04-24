using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class ConteudoEncomenda
    {
        private Encomenda encomenda;
        private ProdutoPersonalizado produtoPersonalizado;
        private int quantidade;

        public int Quantidade
        {
            get
            {
                return quantidade;
            }

            set
            {
                quantidade = value;
            }
        }

        internal Encomenda Encomenda
        {
            get
            {
                return encomenda;
            }

            set
            {
                encomenda = value;
            }
        }

        internal ProdutoPersonalizado ProdutoPersonalizado
        {
            get
            {
                return produtoPersonalizado;
            }

            set
            {
                produtoPersonalizado = value;
            }
        }
    }
}
      