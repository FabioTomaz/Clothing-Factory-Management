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
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarEmpregado.xaml
    /// </summary>
    public partial class EditarEmpregado : Page
    {
        private DataHandler dataHandler;
        Utilizador u;
        public EditarEmpregado(DataHandler dh, Utilizador u)
        {
            InitializeComponent();
            this.dataHandler = dh;
            this.u = u;
            txtNome.Text = u.Nome;
            txtEmail.Text = u.Email;
            txtSalario.Text = u.Salario.ToString();
            txtnFilial.Text = u.Filial.NFilial.ToString();
            String[] split = u.Localizacao.CodigoPostal.Split('-');
            txtcodigoPostal1.Text = split[0];
            txtcodigoPostal2.Text = split[1];
            txtRua.Text = u.Localizacao.Rua1;
            txtNumeroPorta.Text = u.Localizacao.Porta.ToString();
            txtSuper.Text = u.Supervisor.NFuncionario.ToString();
            txtSaida.Value = DateTime.Parse(u.HoraSaida.ToString());
            txtEntrada.Value = DateTime.Parse(u.HoraEntrada.ToString());
            txtTelemovel.Text = u.Telemovel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(u.TiposUser.Contains("Gestor da Empresa"))
            {
                ckEmpr.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Produção"))
            {
                ckProd.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Vendas"))
            {
                ckVend.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Recursos Humanos"))
            {
                ckRH.IsChecked = true;
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar a edição dos dados do empregado? Perderá todos as alterações que tenha feito.",
                "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validarInput();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            u.Nome = txtNome.Text;
            u.Salario = Convert.ToDouble(txtSalario.Text);
            u.Telemovel = txtTelemovel.Text;
            u.Filial = new filial();

            u.Filial.NFilial = Convert.ToInt32(txtnFilial.Text);
            List<string> tiposUser = new List<string>();
            if (ckEmpr.IsChecked == true)
                tiposUser.Add(ckEmpr.Content.ToString());
            if (ckProd.IsChecked == true)
                tiposUser.Add(ckProd.Content.ToString());
            if (ckVend.IsChecked == true)
                tiposUser.Add(ckVend.Content.ToString());
            if (ckRH.IsChecked == true)
                tiposUser.Add(ckRH.Content.ToString());
            u.TiposUser = tiposUser;
            u.Email = txtEmail.Text;
            u.Localizacao = new Localizacao();
            u.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
            u.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
            u.Localizacao.Rua1 = txtRua.Text;
            u.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
            u.HoraEntrada = TimeSpan.Parse(txtEntrada.Text);
            u.HoraSaida = TimeSpan.Parse(txtSaida.Text);

            u.Supervisor = new Utilizador();
            u.Supervisor.NFuncionario = Convert.ToInt32(txtSuper.Text);

            try
            {
                dataHandler.atualizarEmpregado(u);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Empregado Editado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void validarInput()
        {
            Regex regex = new Regex("[^a-bA-B]+");
            if (ckEmpr.IsChecked == false && ckProd.IsChecked == false && ckVend.IsChecked == false && ckRH.IsChecked == false)
                throw new Exception("Tem que selecionar pelo menos uma função a atribuir a este empregado!");
            if (txtNome.Text.Trim().Length > 50 || txtNome.Text.Trim().Length < 5)
                throw new Exception("O nome introduzido está incorreto. Deverá ter no minimo 5 caracteres e no maximo 50.");
            if (string.IsNullOrEmpty(txtSalario.Text))
                throw new Exception("Por favor, indique o salário deste empregado.");
            if (!IsValidEmail(txtEmail.Text.Trim()))
                throw new Exception("O Email introduzido está escrito de forma incorreta.");
            if (txtEmail.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza um email válido.");
            if (txtTelemovel.Text.Trim().Length > 9)
                throw new Exception("O Telemovel introduzido está incorreto.");
            if (txtcodigoPostal1.Text.Length != 4 || txtcodigoPostal2.Text.Length != 3)
                throw new Exception("O Código Postal introduzido está incorreto.");
            if (txtRua.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza a rua do cliente");
            if (txtNumeroPorta.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza o nº porta do Empregado");
            if(TimeSpan.Parse(txtSaida.Text) <= TimeSpan.Parse(txtEntrada.Text))
                throw new Exception("A hora de saída não pode ser menor ou igual do que a hora de entreda!");
            if (txtnFilial.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza o nº da filial onde pretende Editar o empregado.");
            if (txtSuper.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza o nº funcionário que será responsável por ser o supervisor deste novo empregado.");
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnAdicionarImagem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));

            }
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
