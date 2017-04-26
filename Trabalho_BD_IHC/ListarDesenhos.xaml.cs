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
    /// Interaction logic for ListarDesenhos.xaml
    /// </summary>
    public partial class ListarDesenhos : Page
    {
        DataHandler dataHandler;
        public ListarDesenhos(DataHandler dataHadler)
        {
            InitializeComponent();
            this.dataHandler = dataHadler;
        }

        private void getDesenhosBase(object sender, RoutedEventArgs e)
        {
            editarDesenhoBase.IsEnabled = false;
            removerDesenhoBase.IsEnabled = false;
            desenhosBaseLista.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                /* SqlCommand cmd = new SqlCommand("SELECT N_DESENHO, NOME_DESENHO, DATA_ALTERACAO, "
                                 +"INTRUÇÕES_PRODUÇÃO, N_GESTOR_PROD, UTILIZADOR.NOME FROM DESENHO JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO", dataHandler.Cn);
                 SqlDataReader reader = cmd.ExecuteReader();
                 ObservableCollection<Desenho> desenhoBase = new ObservableCollection<Desenho>();
                 while (reader.Read())
                 {
                     Desenho Des = new Desenho();
                     Des.NDesenho = Convert.ToInt32(reader["N_DESENHO"].ToString());
                     Des.Nome = reader["NOME_DESENHO"].ToString();
                     Des.InstrucoesProducao = reader["INTRUÇÕES_PRODUÇÃO"].ToString();
                     Des.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                     Des.GestorProducao = new GestorProducao();
                     Des.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                     Des.GestorProducao.Nome = reader["NOME"].ToString();
                     desenhoBase.Add(Des);
                 }

                 desenhosBaseLista.ItemsSource = desenhoBase;*/

                dataHandler.closeSGBDConnection();
            }
        }

        private void getDesenhosPers(object sender, RoutedEventArgs e)
        {
            editarDesenhoPersonalizado.IsEnabled = false;
            removerDesenhoPersonalizado.IsEnabled = false;
            desenhosPersonalizadosLista.Focus();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT N_DESENHO, NOME_DESENHO, DATA_ALTERACAO, "
                                + "INTRUÇÕES_PRODUÇÃO, N_GESTOR_PROD, UTILIZADOR.NOME FROM DESENHO JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Desenho> desenhoBase = new ObservableCollection<Desenho>();
                while (reader.Read())
                {
                    Desenho Des = new Desenho();
                    Des.NDesenho = Convert.ToInt32(reader["N_DESENHO"].ToString());
                    Des.Nome = reader["NOME_DESENHO"].ToString();
                    Des.InstrucoesProducao = reader["INTRUÇÕES_PRODUÇÃO"].ToString();
                    Des.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                    Des.GestorProducao = new GestorProducao();
                    Des.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                    Des.GestorProducao.Nome = reader["NOME"].ToString();
                    desenhoBase.Add(Des);
                }

                desenhosBaseLista.ItemsSource = desenhoBase;

                dataHandler.closeSGBDConnection();
            }
        }
        private void registarDesenhoBase_click(object sender, RoutedEventArgs e)
        {
            RegistarDesenhoBase page = new RegistarDesenhoBase(dataHandler);
            NavigationService.Navigate(page);
        }

        private void desenhos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (desenhosPersonalizadosLista.SelectedItems.Count > 0)
            {
                editarDesenhoPersonalizado.IsEnabled = true;
                removerDesenhoPersonalizado.IsEnabled = true;
            }
            if (desenhosBaseLista.SelectedItems.Count > 0)
            {
                editarDesenhoBase.IsEnabled = true;
                removerDesenhoBase.IsEnabled = true;
            }
        }
    }
}
