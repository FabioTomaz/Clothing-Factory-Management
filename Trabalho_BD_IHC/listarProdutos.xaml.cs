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
        private DataHandler dataHandler;
        private ListarMateriais listarMateriais;
        public ListarProdutos(DataHandler dataHadler, ListarMateriais listarMateriais)
        {
            InitializeComponent();
            this.dataHandler = dataHadler;
            this.listarMateriais = listarMateriais;
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
            EditarProdutoBase page = new EditarProdutoBase(dataHandler, (int)((ProdutoBase)produtosBaseLista.SelectedItem).Referencia);
            this.NavigationService.Navigate(page);
        }

        private void registarProdutoPers_click(object sender, RoutedEventArgs e)
        {
            RegistarProduto page = new RegistarProduto(dataHandler, listarMateriais);
            NavigationService.Navigate(page);
        }

        private void ProduzirProdutoPers_click(object sender, RoutedEventArgs e)
        {
            ProduzirProduto page = new ProduzirProduto(dataHandler, (ProdutoPersonalizado)produtosPersonalizadosLista.SelectedItem, listarMateriais);
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
            if (pesquisaREFERENCIA.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtInput.Text) && Regex.IsMatch(txtInput.Text, @"^\d+$"))
                {
                    ObservableCollection<ProdutoBase> prod = new ObservableCollection<ProdutoBase>();
                    ProdutoBase prodBase = dataHandler.getProdutoBaseFromDBWithRef(Convert.ToInt32(txtInput.Text));
                    if(prodBase!=null)
                        prod.Add(prodBase);
                    produtosBaseLista.ItemsSource = prod;
                }
            }
            else if (pesquisaNOME.IsChecked == true)
            {
                produtosBaseLista.ItemsSource = dataHandler.getProdutosBaseFromDBNome(txtInput.Text);
            }
            else if (pesquisaGESTOR.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtInput.Text) && Regex.IsMatch(txtInput.Text, @"^\d+$"))
                {
                    produtosBaseLista.ItemsSource = dataHandler.getProdutoBaseFromDBNGestor(Convert.ToInt32(txtInput.Text));
                }

            }
        }

        private void txtsearchMo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            Button_Click(sender, e);
            e.Handled = true;
        }

        private void txtsearchPro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            Button_Click_1(sender, e);
            e.Handled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pesquisaRef.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtInputPers.Text) && Regex.IsMatch(txtInputPers.Text, @"^\d+$"))
                    produtosPersonalizadosLista.ItemsSource = dataHandler.getProdutosPersonalizadosFromDBRef(Convert.ToInt32(txtInputPers.Text));
            }
            else if (pesquisaCor.IsChecked == true)
            {
               produtosPersonalizadosLista.ItemsSource = dataHandler.getProdutosPersonalizadosFromDBCor(txtInputPers.Text);
            }
        }

        private void produtosPersonalizadosLista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            detalhesProdutoPersonalizado_Click(sender, e);
        }

        private void produtosBaseLista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            detalhesProdutoBase_Click(sender, e);
        }

        public void refreshProdutosBase()
        {
            ObservableCollection<ProdutoBase> produtoBase = dataHandler.getProdutosBaseFromDB();
            produtosBaseLista.ItemsSource = produtoBase;
        }

        public void refreshProdutosPersonalizados()
        {
            ObservableCollection<ProdutoPersonalizado> produtosPersonalizados = dataHandler.getProdutosPers();
            produtosPersonalizadosLista.ItemsSource = produtosPersonalizados;
        }

        private void pesquisaNOME_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Nome do Desenho de produto");
        }

        private void pesquisaGESTOR_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Gestor do Desenho de produto");
        }

        private void pesquisaREFERENCIA_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInput.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Referencia de Desenho de produto");
        }

        private void pesquisaRef_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInputPers.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por referência do desenho de produto associdado");
        }

        private void pesquisaCor_Checked(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            txtInputPers.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Cor do produto");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            txtInput.Text = "";
            refreshProdutosBase();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            txtInputPers.Text = "";
            refreshProdutosPersonalizados();
        }

        
    }
}
