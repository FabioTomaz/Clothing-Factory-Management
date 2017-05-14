using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarEncomenda.xaml
    /// </summary>
    public partial class EditarEncomenda : Page
    {
        DataHandler dataHandler;
        Encomenda Encomenda;
        public EditarEncomenda(DataHandler dataHandler, Encomenda Encomenda)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.Encomenda = Encomenda;
            txtNEncomenda.Content = Encomenda.NEncomenda;
            txtDesconto.Value = ((int?)Encomenda.Desconto);
            txtCliente.Content = ((Cliente)Encomenda.Cliente).NCliente;
            dataPrevista.SelectedDate = Encomenda.DataPrevistaEntrega;
            getProdutosFromEncomenda();
        }

        private void getProdutosFromEncomenda()
        {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT NOME, CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO, TAMANHO_PRODUTO, quantidade ,COR_PRODUTO, PRECO "
                                + " FROM ENCOMENDA JOIN CONTEUDO_Encomenda ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=Encomenda.N_Encomenda"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_Encomenda.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " JOIN [PRODUTO-BASE] ON [PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA"
                                + " WHERE ENCOMENDA.N_ENCOMENDA=@ENCOMENDA;"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", Encomenda.NEncomenda);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoBase> list = new ObservableCollection<ProdutoBase>();
            while (reader.Read())
            {
                ProdutoBase Prod = new ProdutoBase();
                Prod.Nome = reader["NOME"].ToString();
                /*Prod.Quantidade= Convert.ToInt32(reader["quantidade"].ToString());
                Prod.Referencia = Convert.ToInt32(reader["referencia_produto"].ToString());
                Prod.Tamanho = reader["tamanho_produto"].ToString();
                Prod.Cor = reader["cor_produto"].ToString();
                Prod.Preco = Convert.ToDouble(reader["preco"].ToString());*/
                list.Add(Prod);
            }
            produtos.ItemsSource = list;
            dataHandler.closeSGBDConnection();
        }

        private void updateEncomenda() {
            int rows = 0;
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("UPDATE ENCOMENDA SET DESCONTO=@DESCONTO, DATA_ENTREGA_PREV = @DATEP WHERE N_ENCOMENDA=@ENCOMENDA"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", Encomenda.NEncomenda);
            cmd.Parameters.AddWithValue("@DESCONTO", Encomenda.Desconto);
            cmd.Parameters.AddWithValue("@DATEP", Encomenda.DataPrevistaEntrega);

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o contacto na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                {
                    dataHandler.closeSGBDConnection();
                    MessageBox.Show("Edição da encomenda realizada com sucesso", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                   if (MessageBox.Show("Não foi possivel atualizar a encomenda na base de dados. Deseja tentar novamente?", "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                    updateEncomenda();
                   else
                    dataHandler.closeSGBDConnection();
            }
        } 

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Encomenda.Desconto = (double)txtDesconto.Value;
                Encomenda.DataPrevistaEntrega = dataPrevista.SelectedDate.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                updateEncomenda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
    }
}
