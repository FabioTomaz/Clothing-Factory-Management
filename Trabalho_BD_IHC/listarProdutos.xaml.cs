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
    /// Interaction logic for ListarProdutos.xaml
    /// </summary>
    public partial class ListarProdutos : Page
    {
        DataHandler dataHandler;
        public ListarProdutos(DataHandler dataHadler)
        {
            InitializeComponent();
            this.dataHandler = dataHadler;
        }

        private void Page_Load(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = sender as TabItem;

            if (tabItem.Name.Equals("produtoBase", StringComparison.Ordinal))
            {//pagina desenhos base
                editarProdutoBase.IsEnabled = false;
                detalhesProdutoBase.IsEnabled = false;
                produtosBaseLista.Focus();
                ObservableCollection<ProdutoBase> produtoBase = dataHandler.getProdutosBaseFromDB();
                produtosBaseLista.ItemsSource = produtoBase;
            }
            else
            {//pagina desenhos personalizados
                detalhesProdutoPersonalizado.IsEnabled = false;
                produzirProduto.IsEnabled = false;
                produtosPersonalizadosLista.Focus();
                if (!dataHandler.verifySGBDConnection())
                {
                    MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    produtosPersonalizadosLista.ItemsSource = dataHandler.getProdutosPers();
            }
            dataHandler.closeSGBDConnection();
        }

        private void registarProdutoBase_click(object sender, RoutedEventArgs e)
        {
            RegistarProdutoBase page = new RegistarProdutoBase(dataHandler);
            NavigationService.Navigate(page);
        }
        private void EditarProdutoBase_click(object sender, RoutedEventArgs e)
        {
            EditarProdutoBase page = new EditarProdutoBase(dataHandler, (ProdutoBase)produtosBaseLista.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void registarProdutoPers_click(object sender, RoutedEventArgs e)
        {
            RegistarProduto page = new RegistarProduto(dataHandler);
            NavigationService.Navigate(page);
        }

        private void ProduzirProdutoPers_click(object sender, RoutedEventArgs e)
        {
            ProduzirProduto page = new ProduzirProduto(dataHandler, (ProdutoPersonalizado)produtosPersonalizadosLista.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void produtos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (produtosPersonalizadosLista.SelectedItems.Count > 0)
            {
                detalhesProdutoPersonalizado.IsEnabled = true;
                produzirProduto.IsEnabled = true;
            }
            if (produtosBaseLista.SelectedItems.Count > 0)
            {
                editarProdutoBase.IsEnabled = true;
                detalhesProdutoBase.IsEnabled = true;
            }
        }

        private void detalhesProdutoBase_Click(object sender, RoutedEventArgs e)
        {
            if (produtosBaseLista.SelectedItems.Count == 1)
            {
                Console.WriteLine(((ProdutoBase)produtosBaseLista.SelectedItem).Referencia);
                DetalhesProdutoBase detalhes = new DetalhesProdutoBase(dataHandler, (int)((ProdutoBase)produtosBaseLista.SelectedItem).Referencia);
                detalhes.Show();
            }
        }

        private void detalhesProdutoPersonalizado_Click(object sender, RoutedEventArgs e)
        {
            if (produtosPersonalizadosLista.SelectedItems.Count == 1)
            {
                ProdutoPersonalizado prod = (ProdutoPersonalizado)produtosPersonalizadosLista.SelectedItem;
                DetalhesProdutoPersonalizado detalhes = new DetalhesProdutoPersonalizado(dataHandler, (int)prod.ProdutoBase.Referencia, prod.Tamanho, prod.Cor, (int)prod.ID);
                detalhes.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                ObservableCollection<ProdutoBase> prod = new ObservableCollection<ProdutoBase>();
                prod.Add(dataHandler.getProdutoBaseFromDB(Convert.ToInt32(txtInput.Text)));
                produtosBaseLista.ItemsSource = prod;
            }
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, indique a referência do desenho que pretende pesquisar", "Informação", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void txtnomeCl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            produtosBaseLista.ItemsSource = dataHandler.searchClientesInDB(txtInput.Text);
            e.Handled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInputPers.Text) || Regex.IsMatch(txtInputPers.Text, @"^\d+$"))
                produtosPersonalizadosLista.ItemsSource = dataHandler.searchAndGetProdutosPersID(Convert.ToInt32(txtInputPers.Text));

            else
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, indique o nº do id do produto que pretende pesquisar", "Informação", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
    }
}
