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
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesEncomenda.xaml
    /// </summary>
    public partial class DetalhesEncomenda : Window
    {
        Encomenda encomenda;
        DataHandler dataHandler;
        public DetalhesEncomenda(DataHandler dataHandler, int numEncomenda)
        {
            this.dataHandler = dataHandler;
            this.encomenda = dataHandler.getEncomendaFromDB(numEncomenda);
            InitializeComponent();
            nomeCliente.Content = encomenda.Cliente.NCliente;
            nEncomenda.Content =encomenda.NEncomenda;
            estadoEncomenda.Content =encomenda.Estado;
            nCliente.Content =encomenda.Cliente.NCliente;
            nomeCliente.Content =encomenda.Cliente.Nome;
            desconto.Content =encomenda.Desconto;
            preco.Content =encomenda.Preco;
            dataConfirmaçao.Content =encomenda.DataConfirmacao;
            dataPrevistaEntrega.Content =encomenda.DataPrevistaEntrega;
            localEntrega.Content = encomenda.LocalEntrega;
            dataEntrega.Content =encomenda.DataEntrega;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<ProdutoPersonalizado> items = dataHandler.getProdutosFromEncomendaDB(encomenda.NEncomenda);
            if (items != null)
            {
                produtos.ItemsSource = items;
            }

        }

        private void GroupBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*
            if (produtos.SelectedItems.Count == 1)
            {
                DetalhesProdutoPersonalizado window = new DetalhesProdutoPersonalizado(dataHandler, (ProdutoPersonalizado)produtos.SelectedItem);
                window.Show();
            }
            */
        }
    }
}
