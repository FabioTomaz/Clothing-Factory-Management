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
            cancelarEncomenda.IsEnabled = false;
            editarEncomenda.IsEnabled = false;
            encomendas.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA AS NENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO,"
                                +" QUANTIDADE, ESTADO, CLIENTE.NOME as CLIENTENAME, [PRODUTO-BASE].NOME as PRODUCT_NAME"
                                +" FROM((CONTEUDO_ENCOMENDA JOIN ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA = ENCOMENDA.N_ENCOMENDA)"
                                +" JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE)"
                                +" JOIN[PRODUTO-BASE] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO = [PRODUTO-BASE].REFERENCIA", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<ConteudoEncomenda> enc = new ObservableCollection<ConteudoEncomenda>();
                while (reader.Read())
                {
                    ConteudoEncomenda Enc = new ConteudoEncomenda();
                    Enc.Encomenda = new Encomenda();
                    Enc.Encomenda.Cliente = new Cliente();
                    Enc.ProdutoPersonalizado = new ProdutoPersonalizado();
                    Enc.Encomenda.NEncomenda = Convert.ToInt32(reader["NENCOMENDA"].ToString());
                    Enc.ProdutoPersonalizado.ProdutoBase = new ProdutoBase();
                    Enc.ProdutoPersonalizado.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA_PRODUTO"].ToString());
                    Enc.ProdutoPersonalizado.ProdutoBase.Nome = reader["PRODUCT_NAME"].ToString();
                    Enc.ProdutoPersonalizado.Tamanho = reader["TAMANHO_PRODUTO"].ToString();
                    Enc.ProdutoPersonalizado.Cor = reader["COR_PRODUTO"].ToString();
                    Enc.Quantidade = Convert.ToInt32(reader["QUANTIDADE"].ToString());
                    Enc.Encomenda.Cliente.Nome = reader["CLIENTENAME"].ToString();
                    enc.Add(Enc);
                }

                encomendas.ItemsSource = enc;

                dataHandler.closeSGBDConnection();
            }
        }

        private void CancelarEncomenda(Encomenda encomenda)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE Encomenda WHERE N_ENCOMENDA=@EncomendaN ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EncomendaN", encomenda.NEncomenda);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao cancelar encomenda. \n ERROR MESSAGE: \n" + ex.Message);
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
                    ((ObservableCollection<Encomenda>)encomendas.ItemsSource).RemoveAt(listViewIndex);


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
            CriarEncomenda page = new CriarEncomenda(dataHandler);
            NavigationService.Navigate(page);
        }

        private void encomendas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (encomendas.SelectedItems.Count > 0)
            {
                cancelarEncomenda.IsEnabled = true;
                editarEncomenda.IsEnabled = true;
                entregarEncomenda.IsEnabled = true;
            }
        }
    }
}
