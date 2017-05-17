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
using System.IO;
using System.Drawing.Imaging;

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
            if (Utilizador.loggedUser.Imagem != null) { 
                var ms = new MemoryStream();
                Utilizador.loggedUser.Imagem.Save(ms, ImageFormat.Png);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                userImage.Source = bi;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que pretende terminar a sessão? Todas as alterações não guardadas serão perdidas.", "Erro de Inicio de Sessão", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Login login = new Login();
                this.Hide();
                Utilizador.loggedUser = null;
                login.ShowDialog();
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                String imgLoc = op.FileName.ToString();
                userImage.Source = new BitmapImage(new Uri(op.FileName));
                sendUserImageToDB(imgLoc);
                //Utilizador.loggedUser = dataHandler.getUtilizadorFromDB(Utilizador.loggedUser.NFuncionario);
            }

        }

        private void sendUserImageToDB(String imgLoc)
        {
            dataHandler.verifySGBDConnection();
            int rows = 0;
            byte[] images = null;
            FileStream stream = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(stream);
            images = br.ReadBytes((int)stream.Length);
            SqlCommand cmd = new SqlCommand("update utilizador set imagem = @imagem where utilizador.n_funcionario=@funcionario", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@imagem", images);
            cmd.Parameters.AddWithValue("@funcionario", Utilizador.loggedUser.NFuncionario);
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o contacto na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Atualização realizada com sucesso", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                   if (MessageBox.Show("Não foi possivel atualizar o perfil do cliente. Deseja Tentar Novamente?", "Erro", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    sendUserImageToDB(imgLoc);
                dataHandler.closeSGBDConnection();
            }
        }
    }
}
