using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesProdutoPersonalizado.xaml
    /// </summary>
    public partial class DetalhesProdutoPersonalizado : Window
    {
        private DataHandler dataHandler;
        private ProdutoPersonalizado prod;
        public DetalhesProdutoPersonalizado(DataHandler dataHandler, int referencia, string tamanho, string cor, int id)
        {
            InitializeComponent();
            this.dataHandler=dataHandler;
            prod = new ProdutoPersonalizado();
            prod = dataHandler.getProdutoPersonalizadoFromDB(referencia, tamanho, cor, id);
            refProduto.Text = prod.ProdutoBase.Referencia.ToString();
            tamanhoProduto.Text = prod.Tamanho;
            corProduto.Text = prod.Cor;
            versaoProduto.Text = prod.ID.ToString();
            precoProduto.Text = prod.Preco.ToString();
            unidadesProduto.Text = prod.UnidadesStock.ToString();
            nEtiqueta.Text = prod.Etiqueta.Numero.ToString();
            normasEtiqueta.Text = prod.Etiqueta.Normas.ToString();
            paisEtiqueta.Text = prod.Etiqueta.PaisFabrico.ToString();
            composicaoEtiqueta.Text = prod.Etiqueta.Composicao.ToString();
            materias.ItemsSource = dataHandler.getMateriaisFromProdutoPersonalizado(prod);
        }

        private void produtosPers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MaterialTextil produtoSelecionado = ((MaterialTextil)materias.SelectedItem);
            if (produtoSelecionado != null)
            {
                DetalhesMaterial window = new DetalhesMaterial(dataHandler, produtoSelecionado);
                window.Show();
            }
        }
    }
}
