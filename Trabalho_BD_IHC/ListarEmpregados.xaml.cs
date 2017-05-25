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
    /// Interaction logic for ListarEmpregados.xaml
    /// </summary>
    public partial class ListarEmpregados : Page
    {
        private DataHandler dataHandler;
        public ListarEmpregados(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            removerEmpregado.IsEnabled = false;
            editarEmpregado.IsEnabled = false;
            detalhesEmpregado.IsEnabled = false;
            empregados.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZADOR", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Utilizador> user = new ObservableCollection<Utilizador>();
                while (reader.Read())
                {
                    Utilizador u = new Utilizador();
                    u.Localizacao = new Localizacao();
                    u.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                    u.Nome = reader["NOME"].ToString();
                    u.Email = reader["EMAIL"].ToString();
                    u.Telemovel = reader["TELEFONE"].ToString();
                    u.Localizacao.Rua1 = reader["RUA"].ToString();
                    u.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                    user.Add(u);
                }

                empregados.ItemsSource = user;

                dataHandler.closeSGBDConnection();
            }
        }

        private void empregados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (empregados.SelectedItems.Count > 0)
            {
                editarEmpregado.IsEnabled = true;
                removerEmpregado.IsEnabled = true;
                detalhesEmpregado.IsEnabled = true;
            }
        }

        private void removerEmpregado_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = empregados.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende eliminar este empregado da base de dados?\n"
                +"Só deve eliminar um empregado caso este já não trabalhe mais nesta fábrica. Este processo é irreversivel! ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    RemoverEmpregado((Utilizador)empregados.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<Utilizador>)empregados.ItemsSource).RemoveAt(listViewIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void RemoverEmpregado(Utilizador empregado)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE Utilizador WHERE N_FUNCIONARIO=@empregadoN ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@empregadoN", empregado.NFuncionario);
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

        private void registarEmpregado_Click(object sender, RoutedEventArgs e)
        {
            RegistarEmpregado page = new RegistarEmpregado(dataHandler);
            this.NavigationService.Navigate(page);
        }
        private void verDetalhesEmpregado(object sender, RoutedEventArgs e)
        {
            Utilizador user = (Utilizador)empregados.SelectedItem;
            InformaçãoEmpregado page = new InformaçãoEmpregado(dataHandler, user);
            this.NavigationService.Navigate(page);
        }

        public void refresh() {

        }
    }
}
