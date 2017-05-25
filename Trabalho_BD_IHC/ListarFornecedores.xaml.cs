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
    /// Interaction logic for ListarFornecedores.xaml
    /// </summary>
    public partial class ListarFornecedores : Page
    {
        private DataHandler dataHandler;
        public ListarFornecedores(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            removerFornecedor.IsEnabled = false;
            editarFornecedor.IsEnabled = false;
            detalhesFornecedor.IsEnabled = false;
            Fornecedores.Focus();
            if (!dataHandler.verifySGBDConnection())
                return;
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM FORNECEDOR", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
                while (reader.Read())
                {
                    Fornecedor f = new Fornecedor();
                    f.NIF_Fornecedor = reader["NIF"].ToString();
                    f.Email = reader["EMAIL"].ToString();
                    f.Nome = reader["NOME"].ToString();
                    f.Fax = reader["FAX"].ToString();
                    f.Telefone = reader["TELEFONE"].ToString();
                    f.Designacao = reader["DESIGNACAO"].ToString();
                    f.CodigoPostal = reader["CODPOSTAL1"].ToString()+"-"+ reader["CODPOSTAL2"].ToString();
                    f.Rua = reader["RUA"].ToString();
                    f.NPorta  = Convert.ToInt32(reader["N_PORTA"].ToString());
                    fornecedores.Add(f);
                }
                dataHandler.closeSGBDConnection();
                Fornecedores.ItemsSource = fornecedores;
                               
            }
        }

        private void Fornecedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (Fornecedores.SelectedItems.Count > 0)
            {
                editarFornecedor.IsEnabled = true;
                removerFornecedor.IsEnabled = true;
                detalhesFornecedor.IsEnabled = true;
            }
        }

        private void removerFornecedor_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = Fornecedores.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende eliminar este Fornecedor da base de dados?\n"
                +"Só deve eliminar um Fornecedor caso o contrato de fornecimento tenha acabado, ou a empresa "
                +"fornecedora fechar. Este processo é irreversivel! ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    RemoverFornecedor((Utilizador)Fornecedores.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<Utilizador>)Fornecedores.ItemsSource).RemoveAt(listViewIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void RemoverFornecedor(Utilizador Fornecedor)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE FORNECEDORES WHERE NIF=@FornecedorN ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@FornecedorN", Fornecedor.NFuncionario);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }

        private void registarFornecedor_Click(object sender, RoutedEventArgs e)
        {
            RegistarFornecedor page = new RegistarFornecedor(dataHandler);
            this.NavigationService.Navigate(page);
        }
        private void verDetalhesFornecedor(object sender, RoutedEventArgs e)
        {
            Utilizador user = (Utilizador)Fornecedores.SelectedItem;
            /*InformaçãoFornecedor page = new InformaçãoFornecedor(dataHandler, user);
            this.NavigationService.Navigate(page);*/
        }
    }
}
