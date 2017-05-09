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
    /// Interaction logic for ListarMateriais.xaml
    /// </summary>
    public partial class ListarMateriais : Page
    {
        private DataHandler dataHandler;
        public ListarMateriais(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            removerMaterial.IsEnabled = false;
            editarMaterial.IsEnabled = false;
            detalhesMaterial.IsEnabled = false;
            materiais.Focus();
            if (!dataHandler.verifySGBDConnection()) {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                List<MaterialTextil> materiaisTexteis = new List<MaterialTextil>();
                while (reader.Read())
                {
                    MaterialTextil Mt = new MaterialTextil();
                    Mt.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                    Mt.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                    Mt.Designacao = reader["DESIGNACAO"].ToString();
                    Mt.Cor = reader["COR"].ToString();
                    Mt.Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                    materiaisTexteis.Add(Mt);
                }
            
                materiais.ItemsSource = materiaisTexteis;

             

                dataHandler.closeSGBDConnection();
            }
       }

        private void materiais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (materiais.SelectedItems.Count > 0)
            {
                removerMaterial.IsEnabled = true;
                editarMaterial.IsEnabled = true;
                detalhesMaterial.IsEnabled = true;
            }
        }

        private void Encomenda_Click(object sender, RoutedEventArgs e)
        {
            RegistarMaterial page = new RegistarMaterial(dataHandler);
            NavigationService.Navigate(page);
        }
    }

        
}

