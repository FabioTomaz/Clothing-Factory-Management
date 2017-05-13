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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesEncomenda.xaml
    /// </summary>
    public partial class DetalhesEncomenda : Window
    {
        Encomenda encomenda;
        DataHandler dataHandler;
        public DetalhesEncomenda(DataHandler dataHandler, Encomenda encomenda)
        {
            this.encomenda = encomenda;
            this.dataHandler = dataHandler;
            InitializeComponent();
        }

        private void getDetalhes() {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT CLIENTE.NOME, CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO, TAMANHO_PRODUTO, quantidade ,COR_PRODUTO, PRECO "
                                + " FROM ENCOMENDA JOIN CONTEUDO_Encomenda ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=Encomenda.N_Encomenda"
                                + " JOIN CLIENTE ON CLIENTE.NCLIENTE=ENCOMENDA.CLIENTE"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_Encomenda.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " JOIN [PRODUTO-BASE] ON [PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA"
                                + " WHERE ENCOMENDA.N_ENCOMENDA=@ENCOMENDA;"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", encomenda.NEncomenda);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Produto> list = new ObservableCollection<Produto>();
            while (reader.Read())
            {
                Produto Prod = new Produto();
                Prod.Nome = reader["NOME"].ToString();
                Prod.Quantidade = Convert.ToInt32(reader["quantidade"].ToString());
                Prod.Referencia = Convert.ToInt32(reader["referencia_produto"].ToString());
                Prod.Tamanho = reader["tamanho_produto"].ToString();
                Prod.Cor = reader["cor_produto"].ToString();
                Prod.Preco = Convert.ToDouble(reader["preco"].ToString());
                list.Add(Prod);
            }
            produtos.ItemsSource = list;
            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();
            cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA, NOME, NCLIENTE ,DESCRIÇAO, DESCONTO, SUM(PRECO*QUANTIDADE) AS PRECOT, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, DATA_ENTREGA, LOCALENTREGA "
                    + " FROM ENCOMENDA JOIN CONTEUDO_Encomenda ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=Encomenda.N_Encomenda"
                    + " JOIN CLIENTE ON CLIENTE.NCLIENTE=ENCOMENDA.CLIENTE"
                    + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_Encomenda.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                    + " JOIN ESTADO ON ESTADO.ID = ENCOMENDA.ESTADO"
                    + " WHERE ENCOMENDA.N_ENCOMENDA=@ENCOMENDA"
                    + " GROUP BY ENCOMENDA.N_ENCOMENDA, NOME, NCLIENTE ,DESCRIÇAO, DESCONTO, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, DATA_ENTREGA, LOCALENTREGA;"
                    , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", encomenda.NEncomenda);
            reader = cmd.ExecuteReader();
            reader.Read();
            nomeCliente.Content = reader["NOME"].ToString(); 
            nCliente.Content = reader["NCLIENTE"].ToString();
            nEncomenda.Content = reader["N_ENCOMENDA"].ToString();
            estadoEncomenda.Content = reader["DESCRIÇAO"].ToString(); ;
            desconto.Content = reader["DESCONTO"].ToString(); ;
            preco.Content = reader["PRECOT"].ToString(); ;
            dataConfirmaçao.Content = reader["DATA_CONFIRMACAO"].ToString(); 
            dataPrevistaEntrega.Content = reader["DATA_ENTREGA_PREV"].ToString(); 
            dataEntrega.Content = reader["DATA_ENTREGA"].ToString(); 
            localEntrega.Content = reader["LOCALENTREGA"].ToString(); 
            dataHandler.closeSGBDConnection();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            getDetalhes();
        }
    }
}
