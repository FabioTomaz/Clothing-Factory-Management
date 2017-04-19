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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarClientes.xaml
    /// </summary>
    public partial class ListarClientes : Page
    {
        private DataHandler dataHandler;
        public ListarClientes()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataHandler = new DataHandler();
            dataHandler.verifySGBDConnection();

            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE", dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Cliente> items = new List<Cliente>();
            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.Nome = reader["NOME"].ToString();
                C.Nif = Convert.ToInt32(reader["NIF"].ToString());
                C.Nib = reader["NIB"].ToString();
                C.Email = reader["EMAIL"].ToString();
                C.Telemovel = Convert.ToInt32(reader["TELEMOVEL"].ToString());
                C.CodigoPostal = reader["COD_POSTAL"].ToString();
                C.Rua = reader["RUA"].ToString();
                C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                items.Add(C);
            }

            clientes.ItemsSource = items;

            dataHandler.closeSGBDConnection();
        }
    }
}
