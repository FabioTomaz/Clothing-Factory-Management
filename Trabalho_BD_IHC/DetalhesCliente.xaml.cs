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
    public partial class DetalhesCliente : Window
    {
        Cliente cliente;
        DataHandler dataHandler;
        public DetalhesCliente(DataHandler dataHandler, Cliente cliente)
        {
            this.cliente = cliente;
            this.dataHandler = dataHandler;
            InitializeComponent();
            nomeCliente.Content = cliente.Nome;
            nCliente.Content = cliente.NCliente;
            nif.Content = cliente.Nif;
            nib.Content = cliente.Nib;
            email.Content = cliente.Email;
            telemovel.Content = cliente.Telemovel;
            cdgPostal.Content = cliente.CodigoPostal;
            distrito.Content = cliente.Distrito;
            localidade.Content = cliente.Localidade;
            rua.Content = cliente.Rua;
            nporta.Content = cliente.NCasa;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Encomenda> items = dataHandler.getEncomendasFromCliente(cliente.NCliente);
            if (items != null)
                encomendas.ItemsSource = items;
        }

        private void produtos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (encomendas.SelectedItems.Count == 1) { 
                DetalhesEncomenda window = new DetalhesEncomenda(dataHandler, ((Encomenda)encomendas.SelectedItem).NEncomenda);
                window.Show();
            }
        }
    }
}
