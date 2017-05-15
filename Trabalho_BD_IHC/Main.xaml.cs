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
        DataHandler dataHandler;
        public MainWindow(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler; 
            clientesFrame.Content = new ListarClientes(dataHandler);
            encomendasFrame.Content = new ListarEncomendas(dataHandler);
            produtosFrame.Content = new ListarProdutos(dataHandler);
            materiaisFrame.Content = new ListarMateriais(dataHandler);
            empregadosFrame.Content = new ListarEmpregados(dataHandler);
            fillUserInfo();
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            clientesFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void login() {
            Login loginWindow = new Login();
            loginWindow.ShowDialog();
            if (Utilizador.loggedUser == null)
            {
                Application.Current.Shutdown();
            }
        }

        public void fillUserInfo() {
            nomeUtilizador.Content = Utilizador.loggedUser.Nome;
            horaEntrada.Content = Utilizador.loggedUser.HoraEntrada.ToString();
            horaSaida.Content = Utilizador.loggedUser.HoraSaida.ToString();
            emailUtilizador.Content = Utilizador.loggedUser.Email;
            telefoneUtilizador.Content = Utilizador.loggedUser.Telemovel;
            salario.Content = Utilizador.loggedUser.Salario;
            nFuncionario.Content = Utilizador.loggedUser.NFuncionario;
            supervisor.Content =  String.Format("{0} (Nome: {1})", Utilizador.loggedUser.Supervisor.NFuncionario, Utilizador.loggedUser.Supervisor.Nome);
            numFilial.Content = Utilizador.loggedUser.Filial.NFilial;
            emailFilial.Content = Utilizador.loggedUser.Filial.Email;
            faxFilial.Content = Utilizador.loggedUser.Filial.Fax;
            telefoneFilial.Content = Utilizador.loggedUser.Filial.Telefone;
            moradaFilial.Content = String.Format("Distrito de {0} , concelho de {1}, localidade de {2}, rua {3}, porta {4}", Utilizador.loggedUser.Localizacao.Distrito, Utilizador.loggedUser.Localizacao.Concelho, Utilizador.loggedUser.Localizacao.Localidade, Utilizador.loggedUser.Localizacao.Rua1, Utilizador.loggedUser.Localizacao.Porta);
        }
    }
}
