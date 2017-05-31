﻿using System;
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
    /// Interaction logic for ListarFornecedores.xaml
    /// </summary>
    public partial class ListarFornecedores : Page
    {
        private DataHandler dataHandler;
        public ListarFornecedores(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            editarFornecedor.IsEnabled = false;
            detalhesFornecedor.IsEnabled = false;
            Fornecedores.Focus();
            ObservableCollection<Fornecedor> items = dataHandler.getFornecedoresFromDB();
            if(items!=null)
                Fornecedores.ItemsSource = items;
        }

        private void Fornecedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (Fornecedores.SelectedItems.Count > 0)
            {
                editarFornecedor.IsEnabled = true;
                detalhesFornecedor.IsEnabled = true;
            }
        }


        private void registarFornecedor_Click(object sender, RoutedEventArgs e)
        {
            RegistarFornecedor page = new RegistarFornecedor(dataHandler);
            this.NavigationService.Navigate(page);
        }
        private void verDetalhesFornecedor(object sender, RoutedEventArgs e)
        {
            DetalhesFornecedor window = new DetalhesFornecedor(dataHandler, (Fornecedor)Fornecedores.SelectedItem);
            window.Show();
        }

        public void refresh() {
            ObservableCollection<Fornecedor> items = dataHandler.getFornecedoresFromDB();
            if (items != null)
                Fornecedores.ItemsSource = items;
        }

        private void editarFornecedor_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EditarFornecedor(dataHandler, (Fornecedor)Fornecedores.SelectedItem));
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Fornecedores.ItemsSource = dataHandler.searchFornecedoresInDB(txtnomeForn.Text);
        }

        private void txtsearchFo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            searchButton_Click(sender, e);
            e.Handled = true;
        }
    }
}
