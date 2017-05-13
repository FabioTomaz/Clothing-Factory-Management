using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Encomenda
    {
        private Cliente cliente;
        private int nEncomenda;
        private String estado;
        private DateTime dataConfirmacao;
        private DateTime dataEntrega;
        private DateTime dataPrevistaEntrega;
        private double desconto;
        private String localEntrega;
        private Utilizador gestorVendas;
        private int quantidade;
        private double preco;

        public Cliente Cliente
        {
            get
            {
                return cliente;
            }

            set
            {
                cliente = value;
            }
        }

        public int NEncomenda
        {
            get
            {
                return nEncomenda;
            }

            set
            {
                nEncomenda = value;
            }
        }

        public string Estado
        {
            get
            {
                return estado;
            }

            set
            {
                estado = value;
            }
        }

        public DateTime DataConfirmacao
        {
            get
            {
                return dataConfirmacao;
            }

            set
            {
                dataConfirmacao = value;
            }
        }

        public DateTime DataEntrega
        {
            get
            {
                return dataEntrega;
            }

            set
            {
                dataEntrega = value;
            }
        }

        public DateTime DataPrevistaEntrega
        {
            get
            {
                return dataPrevistaEntrega;
            }

            set
            {
                dataPrevistaEntrega = value;
            }
        }

        public double Desconto
        {
            get
            {
                return desconto;
            }

            set
            {
                desconto = value;
            }
        }

        public string LocalEntrega
        {
            get
            {
                return localEntrega;
            }

            set
            {
                localEntrega = value;
            }
        }


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

        public Utilizador GestorVendas
        {
            get
            {
                return gestorVendas;
            }

            set
            {
                gestorVendas = value;
            }
        }
    }
}
