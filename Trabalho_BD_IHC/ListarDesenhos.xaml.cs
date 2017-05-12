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

        private void Page_Load(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = sender as TabItem;

            if (tabItem.Name.Equals("desenhoBase", StringComparison.Ordinal))
            {//pagina desenhos base
                editarDesenhoBase.IsEnabled = false;
                removerDesenhoBase.IsEnabled = false;
                detalhesDesenhoBase.IsEnabled = false;
                desenhosBaseLista.Focus();
                if (!dataHandler.verifySGBDConnection())
                {
                    MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    ObservableCollection<DesenhoBase> desenhoBase = getDesenhosBase();
                    desenhosBaseLista.ItemsSource = desenhoBase;

                }
            }
            else
            {//pagina desenhos personalizados
                editarDesenhoPersonalizado.IsEnabled = false;
                removerDesenhoPersonalizado.IsEnabled = false;
                detalhesDesenhoPersonalizado.IsEnabled = false;
                desenhosPersonalizadosLista.Focus();
                if (!dataHandler.verifySGBDConnection())
                {
                    MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    ObservableCollection<DesenhoPersonalizado> desenhoPers = getDesenhosPers();
                    desenhosPersonalizadosLista.ItemsSource = desenhoPers;
                    
                }
            }
            dataHandler.closeSGBDConnection();
        }
        public ObservableCollection<DesenhoBase> getDesenhosBase()
        {
            SqlCommand cmd = new SqlCommand("SELECT N_DESENHO, NOME_DESENHO, DATA_ALTERACAO, "
                            + "INSTRUCOES_PRODUCAO, N_GESTOR_PROD, UTILIZADOR.NOME FROM DESENHO JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO", dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<DesenhoBase> desenhoBase = new ObservableCollection<DesenhoBase>();
            while (reader.Read())
            {
                DesenhoBase Des = new DesenhoBase();
                Des.NDesenho = Convert.ToInt32(reader["N_DESENHO"].ToString());
                Des.Nome = reader["NOME_DESENHO"].ToString();
                Des.InstrucoesProducao = reader["INSTRUCOES_PRODUCAO"].ToString();
                Des.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                Des.GestorProducao = new GestorProducao();
                Des.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                Des.GestorProducao.Nome = reader["NOME"].ToString();
                desenhoBase.Add(Des);
            }

            dataHandler.closeSGBDConnection();
            return desenhoBase;
        }


        private ObservableCollection<DesenhoPersonalizado> getDesenhosPers()
        {
            SqlCommand cmd = new SqlCommand("SELECT N_MODELO, MODELO.N_DESENHO as Ndesenho, ETIQUETA.N_ETIQUETA as Netiqueta, NORMAS, PAIS_FABRICO, COMPOSICAO, "
                    +" NOME_DESENHO, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, NOME FROM MODELO JOIN ETIQUETA ON ETIQUETA.N_ETIQUETA = MODELO.N_ETIQUETA "
                    +" JOIN DESENHO ON DESENHO.N_DESENHO = MODELO.N_DESENHO JOIN UTILIZADOR  ON N_GESTOR_PROD = N_FUNCIONARIO", dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<DesenhoPersonalizado> desenhoPers = new ObservableCollection<DesenhoPersonalizado>();
            while (reader.Read())
            {
                DesenhoPersonalizado Des = new DesenhoPersonalizado();
                Des.NDesPers = Convert.ToInt32(reader["N_MODELO"].ToString());
                Des.Etiqueta = new Etiqueta();
                Des.Etiqueta.Numero = Convert.ToInt32(reader["Netiqueta"].ToString());
                Des.Etiqueta.Normas = reader["NORMAS"].ToString();
                Des.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                Des.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                Des.Desenho = new DesenhoBase();
                Des.Desenho.NDesenho = Convert.ToInt32(reader["Ndesenho"].ToString());
                Des.Desenho.Nome = reader["NOME_DESENHO"].ToString();
                Des.Desenho.InstrucoesProducao = reader["INSTRUCOES_PRODUCAO"].ToString();
                Des.Desenho.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                Des.Desenho.GestorProducao = new GestorProducao();
                Des.Desenho.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                Des.Desenho.GestorProducao.Nome = reader["NOME"].ToString();
                desenhoPers.Add(Des);
                Console.WriteLine(Des.Etiqueta.Normas.ToString());
            }
            dataHandler.closeSGBDConnection();
            return desenhoPers;
        }

        private void registarDesenhoBase_click(object sender, RoutedEventArgs e)
        {
            RegistarDesenhoBase page = new RegistarDesenhoBase(dataHandler);
            NavigationService.Navigate(page);
        }
        private void EditarDesenhoBase_click(object sender, RoutedEventArgs e)
        {
            EditarDesenhoBase page = new EditarDesenhoBase(dataHandler, (DesenhoBase)desenhosBaseLista.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void registarDesenhoPers_click(object sender, RoutedEventArgs e)
        {
            RegistarDesenhoPersonalizado page = new RegistarDesenhoPersonalizado(dataHandler);
            NavigationService.Navigate(page);
        }

        private void EditarDesenhoPers_click(object sender, RoutedEventArgs e)
        {
            EditarDesenhoPersonalizado page = new EditarDesenhoPersonalizado(dataHandler, (DesenhoPersonalizado)desenhosPersonalizadosLista.SelectedItem);
            this.NavigationService.Navigate(page);
        }

        private void desenhos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (desenhosPersonalizadosLista.SelectedItems.Count > 0)
            {
                editarDesenhoPersonalizado.IsEnabled = true;
                removerDesenhoPersonalizado.IsEnabled = true;
                detalhesDesenhoPersonalizado.IsEnabled = true;
            }
            if (desenhosBaseLista.SelectedItems.Count > 0)
            {
                editarDesenhoBase.IsEnabled = true;
                removerDesenhoBase.IsEnabled = true;
                detalhesDesenhoBase.IsEnabled = true;
            }
        }
    }
}
