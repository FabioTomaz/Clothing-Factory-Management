using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
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
    /// Interaction logic for DetalhesProdutoBase.xaml
    /// </summary>
    public partial class DetalhesProdutoBase : Window
    {
        private DataHandler dataHandler;
        private ProdutoBase produtoBase;
        private ObservableCollection<ProdutoPersonalizado> produtosPersonalizados; 
        public DetalhesProdutoBase(DataHandler dataHandler, int referencia)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.produtoBase = dataHandler.getProdutoBaseFromDB(referencia);
            nomeProduto.Text = produtoBase.Nome;
            ivaProduto.Text = produtoBase.IVA1.ToString();
            refProduto.Text = produtoBase.Referencia.ToString();
            gestorProduto.Text = produtoBase.GestorProducao.NFuncionario.ToString();
            instrucoesProduto.Text = produtoBase.InstrProd;
            dataProduto.Text = produtoBase.DataAlteraçao.ToString("dd/MM/yyyy");
            produtosPersonalizados=dataHandler.getProdutosPersonalizadosFromProdutoBaseDB(referencia);
            produtosPers.ItemsSource = produtosPersonalizados;
           
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProdutoPersonalizado produtoSelecionado = ((ProdutoPersonalizado)produtosPers.SelectedItem);
            if (produtoSelecionado != null)
            {
                DetalhesProdutoPersonalizado window = new DetalhesProdutoPersonalizado(dataHandler, produtoSelecionado.ProdutoBase.Referencia, produtoSelecionado.Tamanho, produtoSelecionado.Cor, produtoSelecionado.ID);
                window.Show();
            }
        }
    }
}
