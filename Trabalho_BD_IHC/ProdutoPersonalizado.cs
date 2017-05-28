using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class ProdutoPersonalizado
    {
        private ProdutoBase produtoBase;
        private String tamanho;
        private String cor;
        private int id;
        private double preco;
        private Etiqueta etiqueta;
        private int unidadesStock;
        private int quantidade;
        private ObservableCollection<MaterialTextil> materiaisTexteis;

        public ProdutoPersonalizado()
        {
            this.ProdutoBase = new ProdutoBase();
            this.etiqueta = new Etiqueta();
            this.materiaisTexteis = new ObservableCollection<MaterialTextil>();
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

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
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

        public Etiqueta Etiqueta
        {
            get
            {
                return etiqueta;
            }

            set
            {
                etiqueta = value;
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

        public ProdutoBase ProdutoBase
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

        public ObservableCollection<MaterialTextil> MateriaisTexteis
        {
            get
            {
                return materiaisTexteis;
            }

            set
            {
                materiaisTexteis = value;
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
    }
}
