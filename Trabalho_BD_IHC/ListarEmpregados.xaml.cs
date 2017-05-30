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
    /// Interaction logic for ListarEmpregados.xaml
    /// </summary>
    public partial class ListarEmpregados : Page
    {
        private DataHandler dataHandler;
        public ListarEmpregados(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            editarEmpregado.IsEnabled = false;
            detalhesEmpregado.IsEnabled = false;
            empregados.Focus();
            empregados.ItemsSource = dataHandler.getEmpregadosFromDB();
        }

        private void empregados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (empregados.SelectedItems.Count > 0)
            {
                editarEmpregado.IsEnabled = true;
                detalhesEmpregado.IsEnabled = true;
            }
        }

        private void registarEmpregado_Click(object sender, RoutedEventArgs e)
        {
            RegistarEmpregado page = new RegistarEmpregado(dataHandler);
            this.NavigationService.Navigate(page);
        }
        private void verDetalhesEmpregado(object sender, RoutedEventArgs e)
        {
            DetalhesEmpregado window = new DetalhesEmpregado(dataHandler, (Utilizador)empregados.SelectedItem);
            window.Show();
        }

        public void refresh()
        {
            editarEmpregado.IsEnabled = false;
            detalhesEmpregado.IsEnabled = false;
            empregados.Focus();
            empregados.ItemsSource = dataHandler.getEmpregadosFromDB();
        }

        private void editarEmpregado_Click(object sender, RoutedEventArgs e)
        {
            Utilizador u = (Utilizador)empregados.SelectedItem;
            if (u.NFuncionario == 1)
                Xceed.Wpf.Toolkit.MessageBox.Show("Não tem permissões para editar a informação deste empregado!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                this.NavigationService.Navigate(new EditarEmpregado(dataHandler, u));
            }

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            empregados.ItemsSource = dataHandler.searchEmpregadosInDB(txtnameSearch.Text);
        }
    }
}
