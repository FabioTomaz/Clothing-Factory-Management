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
using System.Text.RegularExpressions;

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
            pesquisaNOME.IsChecked = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            editarCliente.IsEnabled = false;
            detalhesCliente.IsEnabled = false;
            clientes.Focus();
            ObservableCollection<Cliente> items = dataHandler.getClientesFromDB();
            if (items != null)
                clientes.ItemsSource = items;
        }

        private void registarCliente_Click(object sender, RoutedEventArgs e)
        {
            RegistarCliente page = new RegistarCliente(dataHandler);
            NavigationService.Navigate(page);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (pesquisaNOME.IsChecked == true)
                clientes.ItemsSource = dataHandler.getClientesInDBFromName(txtnomeCl.Text);
            else if (pesquisaNCLIENTE.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtnomeCl.Text) && Regex.IsMatch(txtnomeCl.Text, @"^\d+$"))
                {
                    ObservableCollection<Cliente> cl = new ObservableCollection<Cliente>();
                    cl.Add(dataHandler.getClientesInDBnCliente(Convert.ToInt32(txtnomeCl.Text)));
                    clientes.ItemsSource = cl;
                }
                else
                    clientes.ItemsSource = dataHandler.getClientesFromDB();
            }
            else if (pesquisaNIF.IsChecked == true)
                clientes.ItemsSource = dataHandler.getClientesInDBNIF(txtnomeCl.Text);
            
            else if (pesquisaMAIL.IsChecked == true)
                clientes.ItemsSource = dataHandler.getClientesInDBFromEmail(txtnomeCl.Text);
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
                editarCliente.IsEnabled = true;
                detalhesCliente.IsEnabled = true;
            }
        }

        public void refresh()
        {
            clientes.Focus();
            ObservableCollection<Cliente> items = dataHandler.getClientesFromDB();
            if (items != null)
                clientes.ItemsSource = items;
        }

        private void txtnomeCl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            searchButton_Click(sender, e);
            e.Handled = true;
        }

        private void clientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetalhesCliente window = new DetalhesCliente(dataHandler, (Cliente)clientes.SelectedItem);
            window.Show();
        }

        private void pesquisaNOME_Checked(object sender, RoutedEventArgs e)
        {
            txtnomeCl.Text = "";
            txtnomeCl.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Nome do Cliente");
        }

        private void pesquisaNCLIENTE_Checked(object sender, RoutedEventArgs e)
        {
            txtnomeCl.Text = "";
            txtnomeCl.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Numero de Cliente");
        }

        private void pesquisaNIF_Checked(object sender, RoutedEventArgs e)
        {
            txtnomeCl.Text = "";
            txtnomeCl.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por NIF do Cliente");
        }

        private void pesquisaMAIL_Checked(object sender, RoutedEventArgs e)
        {
            txtnomeCl.Text = "";
            txtnomeCl.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por E-mail do Cliente");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtnomeCl.Text = "";
            this.refresh();
        }
    }
}
