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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarFiliais.xaml
    /// </summary>
    public partial class ListarFiliais : Page
    {
        private DataHandler dataHandler;
        public ListarFiliais(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            editarFilial.IsEnabled = false;
            detalhesFilial.IsEnabled = false;
            Filiais.Focus();
            Filiais.ItemsSource = dataHandler.getFiliaisFromDB();
        }

        private void Filiais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (Filiais.SelectedItems.Count > 0)
            {
                editarFilial.IsEnabled = true;
                detalhesFilial.IsEnabled = true;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(txtNfilial.Text.Length == 0)
                Xceed.Wpf.Toolkit.MessageBox.Show("Tem que introduzir algo na caixa de texto antes de poder pesquisar", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                Filiais.ItemsSource = dataHandler.searchFiliaisInDB(Convert.ToInt32(txtNfilial.Text));
        }

        private void txtsearchFi_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            SearchButton_Click(sender, e);
            e.Handled = true;
        }

        private void registarFilial_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegistarFilial(dataHandler));
        }

        private void detalhesFilial_Click(object sender, RoutedEventArgs e)
        {
            DetalhesFilial window = new DetalhesFilial(dataHandler, (filial)Filiais.SelectedItem);
            window.Show();
        }

        private void editarFilial_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditarFilial(dataHandler, (filial)Filiais.SelectedItem));
        }

    }


}

