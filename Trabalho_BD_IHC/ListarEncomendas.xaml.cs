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
            entregarEncomenda.IsEnabled = false;
            encomendas.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                getEncomendas();
                dataHandler.closeSGBDConnection();
            }
        }
        private void getEncomendas()
        {
            SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA AS NENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO,"
                                + " QUANTIDADE, ESTADO, CLIENTE.NOME as CLIENTENAME, [PRODUTO-BASE].NOME as PRODUCT_NAME, DATA_ENTREGA_PREV"
                                + " FROM((CONTEUDO_ENCOMENDA JOIN ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA = ENCOMENDA.N_ENCOMENDA)"
                                + " JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE)"
                                + " JOIN[PRODUTO-BASE] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO = [PRODUTO-BASE].REFERENCIA", dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> enc = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda Enc = new Encomenda();
                Enc.Cliente = new Cliente();
                Enc.Produto = new Produto();
                Enc.NEncomenda = Convert.ToInt32(reader["NENCOMENDA"].ToString());
                Enc.Produto.Referencia = Convert.ToInt32(reader["REFERENCIA_PRODUTO"].ToString());
                Enc.Produto.Nome = reader["PRODUCT_NAME"].ToString();
                Enc.Produto.Tamanho = reader["TAMANHO_PRODUTO"].ToString();
                Enc.Produto.Cor = reader["COR_PRODUTO"].ToString();
                Enc.Quantidade = Convert.ToInt32(reader["QUANTIDADE"].ToString());
                Enc.DataPrevistaEntrega = Convert.ToDateTime(reader["DATA_ENTREGA_PREV"]);
                Enc.Cliente.Nome = reader["CLIENTENAME"].ToString();
                enc.Add(Enc);
            }

            encomendas.ItemsSource = enc;
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
            RegistarEncomenda page = new RegistarEncomenda(dataHandler);
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
