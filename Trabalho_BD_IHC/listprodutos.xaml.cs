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
    /// Interaction logic for listarProdutos.xaml
    /// </summary>
    public partial class listarProdutos : Page
    {
     /*   DataHandler dataHandler;
        public listarProdutos(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<ProdutoBase> produtos = new List<ProdutoBase>();
                while (reader.Read())
                {
                    ProdutoBase prod = new ProdutoBase();
                    prod.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                    prod.Nome = reader["Nome"].ToString();
                    prod.Cor = reader["COR"].ToString();
                    produtos.Add(prod);
                }

                produtosLista.ItemsSource = produtos;

                dataHandler.closeSGBDConnection();
            }
        }
        */
    }
}
