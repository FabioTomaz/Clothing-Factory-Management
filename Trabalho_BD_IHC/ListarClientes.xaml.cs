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
            clientes.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.COD_POSTAL=ZONA.COD_POSTAL", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Cliente> items = new ObservableCollection<Cliente>();
                while (reader.Read())
                {
                    Cliente C = new Cliente();
                    C.Nome = reader["NOME"].ToString();
                    C.Nif = reader["NIF"].ToString();
                    C.Nib = reader["NIB"].ToString();
                    C.Email = reader["EMAIL"].ToString();
                    C.Telemovel = reader["TELEMOVEL"].ToString();
                    C.CodigoPostal = reader["COD_POSTAL"].ToString();
                    C.Rua = reader["RUA"].ToString();
                    C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                    items.Add(C);
                }

                clientes.ItemsSource = items;

                dataHandler.closeSGBDConnection();
            }
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
            catch (Exception ex)
            {
                throw new Exception("Falha ao eliminar cliente. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }

        private void registarCliente_Click(object sender, RoutedEventArgs e)
        {
            RegistarCliente page = new RegistarCliente(dataHandler);
            NavigationService.Navigate(page);
        }

        private void removerCliente_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = clientes.SelectedIndex;
            try
            {
                RemoveCliente((Cliente)clientes.SelectedItem);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
               return;
            }
            try { 
                ((ObservableCollection<Cliente>)clientes.ItemsSource).RemoveAt(listViewIndex);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
               return;
            }
        }

        private void editarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente page = new EditarCliente(dataHandler, (Cliente)clientes.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void clientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clientes.SelectedItems.Count > 0)
            {
                removerCliente.IsEnabled = true;
                editarCliente.IsEnabled = true;
            }
        }
    }
}
