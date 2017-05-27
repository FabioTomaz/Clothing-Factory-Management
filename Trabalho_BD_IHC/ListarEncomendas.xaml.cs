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
        

        private void cancelarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = encomendas.SelectedIndex;

            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que pretende cancelar esta encomenda?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                String resultado = dataHandler.cancelarEncomenda(((Encomenda)encomendas.SelectedItem).NEncomenda);
                Xceed.Wpf.Toolkit.MessageBox.Show(resultado, "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void entregarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            String resultado = dataHandler.entregarEncomenda(((Encomenda)encomendas.SelectedItem).NEncomenda);
            Xceed.Wpf.Toolkit.MessageBox.Show(resultado, "Resultado" ,MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void refresh()
        {
            encomendas.Focus();
            ObservableCollection<Encomenda> items = dataHandler.getEncomendasFromDB();
            if (items != null)
                encomendas.ItemsSource = items;
        }
    }
}
