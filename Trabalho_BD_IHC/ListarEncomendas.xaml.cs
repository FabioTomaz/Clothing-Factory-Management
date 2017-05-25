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
    /// Interaction logic for ListarEncomendas.xaml
    /// </summary>
    public partial class ListarEncomendas : Page
    {
        private DataHandler dataHandler;
        public ListarEncomendas(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            detalhesEncomenda.IsEnabled = false;
            cancelarEncomenda.IsEnabled = false;
            editarEncomenda.IsEnabled = false;
            entregarEncomenda.IsEnabled = false;
            encomendas.Focus();
            ObservableCollection<Encomenda> items = dataHandler.getEncomendasFromDB();
            if (items != null)
                encomendas.ItemsSource = items; 
        }

        private void CancelarEncomenda(Encomenda encomenda)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE Encomenda SET ESTADO = 5 WHERE N_ENCOMENDA=@EncomendaN ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EncomendaN", encomenda.NEncomenda);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Erro","Falha ao cancelar a encomenda na base de dados.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }
        

        private void cancelarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = encomendas.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende cancelar esta encomenda?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    CancelarEncomenda((Encomenda)encomendas.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<Encomenda>)encomendas.ItemsSource).ElementAt(listViewIndex).Estado = "Cancelada";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void registarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            RegistarEncomenda page = new RegistarEncomenda(dataHandler);
            NavigationService.Navigate(page);
        }

        private void encomendas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (encomendas.SelectedItems.Count > 0)
            {
                detalhesEncomenda.IsEnabled = true;    
                cancelarEncomenda.IsEnabled = true;
                editarEncomenda.IsEnabled = true;
                entregarEncomenda.IsEnabled = true;
            }
        }

        private void editarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            EditarEncomenda page = new EditarEncomenda(dataHandler, (Encomenda)encomendas.SelectedItem);
            NavigationService.Navigate(page);
        }

        private void detalhesEncomenda_Click(object sender, RoutedEventArgs e)
        {
            DetalhesEncomenda page = new DetalhesEncomenda(dataHandler, (Encomenda)encomendas.SelectedItem);
            page.Show();
        }
    }
}
