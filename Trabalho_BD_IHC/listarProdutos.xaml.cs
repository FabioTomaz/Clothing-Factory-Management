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
                {
                    ObservableCollection<ProdutoPersonalizado> produtosPers = getProdutosPers();
                    produtosPersonalizadosLista.ItemsSource = produtosPers;

                }
            }
            dataHandler.closeSGBDConnection();
        }

        private ObservableCollection<ProdutoPersonalizado> getProdutosPers()
        {
            SqlCommand cmd = new SqlCommand("SELECT TAMANHO, COR, ID, PRECO, UNIDADES_ARMAZEM, "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-BASE].NOME as nomeBase, DATA_ALTERACAO, "
                + "INSTRUCOES_PRODUCAO, IVA, PAIS_FABRICO, [PRODUTO-PERSONALIZADO].N_ETIQUETA, NORMAS, "
                + "PAIS_FABRICO, COMPOSICAO FROM[PRODUTO-PERSONALIZADO] JOIN[PRODUTO-BASE] ON "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA JOIN "
                + "ETIQUETA ON ETIQUETA.N_ETIQUETA = [PRODUTO-PERSONALIZADO].N_ETIQUETA; "
                , dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtoPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.ProdutoBase.Nome = reader["nomeBase"].ToString();
                prod.ProdutoBase.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.ProdutoBase.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.ProdutoBase.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.Etiqueta = new Etiqueta();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                prod.Etiqueta.Normas = reader["NORMAS"].ToString();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                produtoPers.Add(prod);
            }
            dataHandler.closeSGBDConnection();
            return produtoPers;
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
            if (produtosBaseLista.SelectedItems.Count == 1) {
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
    }
}
