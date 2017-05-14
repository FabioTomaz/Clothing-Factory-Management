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
    public partial class DetalhesCliente : Window
    {
        Cliente cliente;
        DataHandler dataHandler;
        public DetalhesCliente(DataHandler dataHandler, Cliente cliente)
        {
            this.cliente = cliente;
            this.dataHandler = dataHandler;
            InitializeComponent();
            nomeCliente.Content = cliente.Nome;
            nCliente.Content = cliente.NCliente;
            nif.Content = cliente.Nif;
            nib.Content = cliente.Nib;
            email.Content = cliente.Email;
            telemovel.Content = cliente.Telemovel;
            cdgPostal.Content = cliente.CodigoPostal;
            distrito.Content = cliente.Distrito;
            concelho.Content = cliente.Concelho;
            localidade.Content = cliente.Localidade;
            rua.Content = cliente.Rua;
            nporta.Content = cliente.NCasa;
        }

        private void getDetalhes() {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA, "
                    + "LOCALENTREGA, ESTADO, N_GESTOR_VENDA, UTILIZADOR.NOME FROM ENCOMENDA JOIN "
                    + "UTILIZADOR ON N_FUNCIONARIO = N_GESTOR_VENDA WHERE CLIENTE = @nCliente"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nCliente", cliente.NCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> list = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda enc = new Encomenda();
                enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"].ToString());
                if (!reader["DATA_ENTREGA"].ToString().Equals(null, StringComparison.Ordinal))
                {
                    enc.DataEntrega = Convert.ToDateTime(reader["DATA_ENTREGA"].ToString());
                }
                enc.LocalEntrega = reader["LOCALENTREGA"].ToString();
                enc.Estado = reader["ESTADO"].ToString();
                enc.GestorVendas = new Utilizador();
                enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                enc.GestorVendas.Nome = reader["GESTOR_NOME"].ToString();
                list.Add(enc);
            }
            produtos.ItemsSource = list;
            dataHandler.closeSGBDConnection();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            getDetalhes();
        }
    }
}
