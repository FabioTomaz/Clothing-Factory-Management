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
        ListarClientes listarClientes;
        ListarEncomendas listarEncomendas;
        ListarMateriais listarMateriais;
        ListarProdutos listarProdutos;
        ListarEmpregados listarEmpregados;
        ListarFornecedores listarFornecedores;
        ListarFiliais listarFiliais;
       
        public MainWindow(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;

            listarClientes = new ListarClientes(dataHandler);
            listarMateriais = new ListarMateriais(dataHandler);
            listarProdutos = new ListarProdutos(dataHandler, listarMateriais);
            listarEncomendas = new ListarEncomendas(dataHandler, listarProdutos, this);
            listarEmpregados = new ListarEmpregados(dataHandler, this);
            listarFornecedores = new ListarFornecedores(dataHandler);
            listarFiliais = new ListarFiliais(dataHandler);

            clientesFrame.Content = listarClientes;
            encomendasFrame.Content = listarEncomendas;
            produtosFrame.Content = listarProdutos;
            materiaisFrame.Content = listarMateriais;
            empregadosFrame.Content = listarEmpregados;
            fornecedoresFrame.Content = listarFornecedores;
            filiaisFrame.Content = listarFiliais;

            fillUserInfo();
            fillNotifications();
            limitFunctions();
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            clientesFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void limitFunctions()
        {
            List<String> tipos = Utilizador.loggedUser.TiposUser;
            if (!tipos.Contains("Gestor de Produção"))
            {
                tabControl.Items.Remove(producao);
                tabControl.Items.Remove(materiais);
            }
            if (!tipos.Contains("Gestor de Vendas"))
            {
                tabControl.Items.Remove(encomendas);
                tabControl.Items.Remove(clientes);
            }
            if (!tipos.Contains("Gestor da Empresa"))
            {
                tabControl.Items.Remove(empresa);
                tabControl.Items.Remove(Fornecedores);
            }
            if(!tipos.Contains<String>("Gestor de Recursos Humanos"))
            {
                tabControl.Items.Remove(empregados);     
            }
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
            if(Utilizador.loggedUser.Supervisor != null)
                supervisor.Content =  String.Format("{0} (Nome: {1})", Utilizador.loggedUser.Supervisor.NFuncionario, Utilizador.loggedUser.Supervisor.Nome);
            numFilial.Content = Utilizador.loggedUser.Filial.NFilial;
            emailFilial.Content = Utilizador.loggedUser.Filial.Email;
            faxFilial.Content = Utilizador.loggedUser.Filial.Fax;
            telefoneFilial.Content = Utilizador.loggedUser.Filial.Telefone;
            moradaFilial.Content = String.Format("{0} , {1}, {2}, porta nº{3}", Utilizador.loggedUser.Localizacao.Distrito, Utilizador.loggedUser.Localizacao.Localidade, Utilizador.loggedUser.Localizacao.Rua1, Utilizador.loggedUser.Localizacao.Porta);
            String str = "";
            for (int i=0; i<Utilizador.loggedUser.TiposUser.Count; i++)
            {
                str += " [" + Utilizador.loggedUser.TiposUser.ElementAt(i) + "] ";
            }
            funcoes.Content = str;
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
        public void refresh()
        {
            Utilizador.loggedUser = dataHandler.getUtilizadorFromDB(Utilizador.loggedUser.NFuncionario);
            fillUserInfo();
            fillNotifications();
        }

        public void fillNotifications()
        {
            double lucroMes = dataHandler.getSaldoDesteMes();
            double dinheiroGasto = dataHandler.getDinheiroGastoMes();
            double dinheiroGerado = dataHandler.getDinheiroGeradoMes();
            int encomendasMes = dataHandler.getEncomendasDesteMes();
            int nProdutos = dataHandler.getNProdutosVendidosAteHoje();
            int nProdutosDesteMes = dataHandler.getNProdutosVendidosMes();
            nEncomendasPrevistas.Content = "Existem " + encomendasMes + " encomendas para serem entregues este mês.";
            nProdutosMes.Content = "Neste mês foram vendidos até ao momento " + nProdutosDesteMes + " produtos";
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
        private void mudarInfo_Click(object sender, RoutedEventArgs e)
        {
            EditarInfPessoal window = new EditarInfPessoal(dataHandler, Utilizador.loggedUser, this);
            window.Show();
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
                    Xceed.Wpf.Toolkit.MessageBox.Show("Atualização realizada com sucesso", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                   if (Xceed.Wpf.Toolkit.MessageBox.Show("Não foi possivel atualizar o perfil do cliente. Deseja Tentar Novamente?", "Erro", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    sendUserImageToDB(imgLoc);
                dataHandler.closeSGBDConnection();
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if(e.Source is Dragablz.TabablzControl) {
            //    Dragablz.TabablzControl tabItem = (Dragablz.TabablzControl)e.Source;
            //    string nome = ((TabItem)tabItem.SelectedItem).Name;
            //    if(nome.Equals("conta"))
            //        this.refresh();
            //    else if(nome.Equals("materiais"))
            //        listarMateriais.refresh();
            //}
        }

        private void MudarPass_Click_2(object sender, RoutedEventArgs e)
        {
            MudarPasse window = new MudarPasse(dataHandler);
            window.Show();
        }

        private void userImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && userImage.Source!=null)
            {
                Imagem window = new Imagem((BitmapImage)userImage.Source);
                window.Show();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((userImage.Source) == null) { 
                Xceed.Wpf.Toolkit.MessageBox.Show("O utilizador não pussui qualquer imagem para ser expandida!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Imagem window = new Imagem((BitmapImage)userImage.Source);
            window.Show();
        }
    }
}
