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
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarEncomendas.xaml
    /// </summary>
    public partial class ListarEncomendas : Page
    {
        private DataHandler dataHandler;
        private ListarProdutos listarProdutos;
        private MainWindow main;
        public ListarEncomendas(DataHandler dataHandler, ListarProdutos listarProdutos, MainWindow main)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.listarProdutos = listarProdutos;
            this.main = main;
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
                this.refresh();
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
            if (encomendas.SelectedItems.Count == 1)
            {
                detalhesEncomenda.IsEnabled = true;
                if (((Encomenda)encomendas.SelectedItem).Estado.Equals("Aceite"))
                {
                    cancelarEncomenda.IsEnabled = true;
                    editarEncomenda.IsEnabled = true;
                    entregarEncomenda.IsEnabled = true;
                }
                else
                {
                    cancelarEncomenda.IsEnabled = false;
                    editarEncomenda.IsEnabled = false;
                    entregarEncomenda.IsEnabled = false;
                }
            }
        }
        private void editarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            EditarEncomenda page = new EditarEncomenda(dataHandler, (Encomenda)encomendas.SelectedItem);
            NavigationService.Navigate(page);
        }

        private void detalhesEncomenda_Click(object sender, RoutedEventArgs e)
        {
            DetalhesEncomenda window = new DetalhesEncomenda(dataHandler, ((Encomenda)encomendas.SelectedItem).NEncomenda);
            window.Show();
        }

        private void entregarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            String resultado = dataHandler.entregarEncomenda(((Encomenda)encomendas.SelectedItem).NEncomenda);
            Xceed.Wpf.Toolkit.MessageBox.Show(resultado, "Resultado" ,MessageBoxButton.OK, MessageBoxImage.Information);
            this.refresh();
            listarProdutos.refreshProdutosPersonalizados();
            main.refresh();
        }

        public void refresh()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pesquisaNENCOMENDA.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtInput.Text) && Regex.IsMatch(txtInput.Text, @"^\d+$"))
                {
                    ObservableCollection<Encomenda> items = new ObservableCollection<Encomenda>();
                    Encomenda enc = dataHandler.getEncomendaFromDB(Convert.ToInt32(txtInput.Text));
                    if(enc!=null)
                        items.Add(enc);
                    encomendas.ItemsSource = items;
                }
            }
            else if (pesquisaNOMECLIENTE.IsChecked == true)
            {
                encomendas.ItemsSource = dataHandler.getEncomendaDBcliente(txtInput.Text);
            }

            else if (pesquisaNCLIENTE.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtInput.Text) && Regex.IsMatch(txtInput.Text, @"^\d+$"))
                {
                    encomendas.ItemsSource = dataHandler.getEncomendaDB(Convert.ToInt32(txtInput.Text));
                }
            }
        }

        private void txtsearchEn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            Button_Click(sender, e);
            e.Handled = true;
        }

        private void encomendas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetalhesEncomenda window = new DetalhesEncomenda(dataHandler, ((Encomenda)encomendas.SelectedItem).NEncomenda);
            window.Show();
        }

        private void txtInput_NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void pesquisaNENCOMENDA_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Numero de Encomenda");
            txtInput.PreviewTextInput+= txtInput_NumberTextInput;
        }

        private void pesquisaNCLIENTE_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Numero de Cliente");
        }

        private void pesquisaNOMECLIENTE_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Nome do Cliente");
            txtInput.PreviewTextInput -= txtInput_NumberTextInput;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            refresh();
        }
    }
}
