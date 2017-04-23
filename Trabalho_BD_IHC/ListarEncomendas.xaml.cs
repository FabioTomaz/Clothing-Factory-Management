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
                SqlCommand cmd = new SqlCommand("SELECT N_ENCOMENDA, CLIENTE, CLIENTE.NOME, ESTADO, N_GESTOR_VENDA, UTILIZADOR.NOME as USERNAME FROM((ENCOMENDA JOIN CLIENTE ON CLIENTE = NIF) JOIN GESTOR_VENDA ON N_GESTOR_VENDA = N_FUNCIONARIO) JOIN UTILIZADOR ON GESTOR_VENDA.N_FUNCIONARIO = UTILIZADOR.N_FUNCIONARIO;", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Encomenda> enc = new ObservableCollection<Encomenda>();
                while (reader.Read())
                {
                    Encomenda Enc = new Encomenda();
                    Enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                    Enc.Cliente = new Cliente();
                    Enc.Cliente.Nif = reader["CLIENTE"].ToString();
                    Enc.Cliente.Nome = reader["NOME"].ToString();
                    Enc.Estado = reader["ESTADO"].ToString();
                    Enc.GestorVendas = new GestorVendas();
                    Enc.GestorVendas.NFuncionario_vendas = new Utilizador();
                    Enc.GestorVendas.NFuncionario_vendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                    Enc.GestorVendas.NFuncionario_vendas.Nome = reader["USERNAME"].ToString();
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
