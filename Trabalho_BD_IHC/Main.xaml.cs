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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private SqlConnection cn;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cn = getSGBDConnection();
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            clientesListBox.Items.Clear();

            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.NIF1 = reader["CustomerID"].ToString();
                C.Nome = reader["CompanyName"].ToString();
                C.NIB1 = reader["ContactName"].ToString();
                C.Telefone = reader["Address"].ToString();
                C.Telemovel = reader["City"].ToString();
                C.País = reader["Region"].ToString();
                C.CodigoPostal = reader["PostalCode"].ToString();
                C.Localidade = reader["Country"].ToString();
                C.Rua = reader["Phone"].ToString();
                clientesListBox.Items.Add(C);
            }

            cn.Close();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=localhost;integrated security=true;initial catalog=GestorFabricaVestuarioLaboral");
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }
    }
}
