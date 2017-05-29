using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarClientes.xaml
    /// </summary>
    public partial class ListarClientes : Page
    {
        private DataHandler dataHandler;
        public ListarClientes(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            removerCliente.IsEnabled = false;
            editarCliente.IsEnabled = false;
            detalhesCliente.IsEnabled = false;
            clientes.Focus();
            ObservableCollection<Cliente> items = dataHandler.getClientesFromDB();
            if(items!=null)
                clientes.ItemsSource = items;
        }

        private void RemoveCliente(Cliente cliente)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE Cliente WHERE NIF=@clienteNIF ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@clienteNIF", cliente.Nif);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
            MessageBox.Show("Cliente Removido com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void registarCliente_Click(object sender, RoutedEventArgs e)
        {
            RegistarCliente page = new RegistarCliente(dataHandler);
            NavigationService.Navigate(page);
        }

        private void removerCliente_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = clientes.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende eliminar este cliente?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    RemoveCliente((Cliente)clientes.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<Cliente>)clientes.ItemsSource).RemoveAt(listViewIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            clientes.ItemsSource = dataHandler.searchClientesInDB(txtnomeCl.Text);
        }
        private void editarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente page = new EditarCliente(dataHandler, (Cliente)clientes.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void verdetalhesCliente(object sender, RoutedEventArgs e)
        {
            DetalhesCliente window = new DetalhesCliente(dataHandler, (Cliente)clientes.SelectedItem);
            window.Show();
        }

        private void clientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (clientes.SelectedItems.Count > 0)
            {
                removerCliente.IsEnabled = true;
                editarCliente.IsEnabled = true;
                detalhesCliente.IsEnabled = true;
            }
        }

        public void refresh() {
            clientes.Focus();
            ObservableCollection<Cliente> items = dataHandler.getClientesFromDB();
            if (items != null)
                clientes.ItemsSource = items;
        }
    }
}
