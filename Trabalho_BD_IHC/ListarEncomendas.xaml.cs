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
            detalhesEncomenda.IsEnabled = false;
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
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA, SUM([PRODUTO-PERSONALIZADO].PRECO*CONTEUDO_ENCOMENDA.QUANTIDADE) AS PRECOTOTAL "
                                + " FROM ENCOMENDA JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE"
                                + " JOIN CONTEUDO_ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=ENCOMENDA.N_ENCOMENDA"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " JOIN ESTADO ON ENCOMENDA.ESTADO=ESTADO.ID"
                                + " GROUP BY ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA;"
                                , dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> enc = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda Enc = new Encomenda();
                Enc.Cliente = new Cliente();
                Enc.GestorVendas = new Utilizador();
                Enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                Enc.Estado = reader["DESCRIÇAO"].ToString();
                Enc.DataPrevistaEntrega = Convert.ToDateTime(reader["DATA_ENTREGA_PREV"]);
                Enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"]);
                Enc.Desconto = Convert.ToInt32(reader["DESCONTO"]);
                Enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                Enc.Cliente.Nome = reader["NOME"].ToString();
                Enc.Cliente.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                Enc.Preco = Convert.ToDouble(reader["PRECOTOTAL"].ToString());
                enc.Add(Enc);
            }

            encomendas.ItemsSource = enc;
            dataHandler.closeSGBDConnection();
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
            e.Handled = true;
            if (encomendas.SelectedItems.Count > 0)
            {
                detalhesEncomenda.IsEnabled = true;    
                cancelarEncomenda.IsEnabled = true;
                editarEncomenda.IsEnabled = true;
                entregarEncomenda.IsEnabled = true;
            }
        }

        private void editarEncomenda_Click(object sender, RoutedEventArgs e)
        {
            EditarEncomenda page = new EditarEncomenda(dataHandler, (Encomenda)encomendas.SelectedItem);
            NavigationService.Navigate(page);
        }

        private void detalhesEncomenda_Click(object sender, RoutedEventArgs e)
        {
            DetalhesEncomenda page = new DetalhesEncomenda(dataHandler, (Encomenda)encomendas.SelectedItem);
            page.Show();
        }
    }
}
