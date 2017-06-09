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
    /// Interaction logic for RegistarEmpregado.xaml
    /// </summary>
    public partial class RegistarEmpregado : Page
    {
        private DataHandler dataHandler;
        public RegistarEmpregado(DataHandler dh)
        {
            InitializeComponent();
            this.dataHandler = dh;
        }


        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo de empregado? Perderá todos os dados que tenha introduzido.",
                "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Utilizador user = new Utilizador();
            try
            {
                validarInput();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            user.Nome = txtNome.Text;
            user.Password = txtPass.Text;
            user.Salario = Convert.ToDouble(txtSalario.Text);
            user.Telemovel = txtTelemovel.Text;
            user.Filial = new filial();
            if ((Boolean)ckEmpr.IsChecked)
                user.TiposUser.Add("Gestor da Empresa");
            if ((Boolean)ckEmpr.IsChecked)
                user.TiposUser.Add("Gestor de Produção");
            if ((Boolean)ckEmpr.IsChecked)
                user.TiposUser.Add("Gestor de Vendas");
            if ((Boolean)ckEmpr.IsChecked)
                user.TiposUser.Add("Gestor de Recursos Humanos");
            user.Filial.NFilial = Convert.ToInt32(txtnFilial.Text);
            List<string> tiposUser = new List<string>();
            if (ckEmpr.IsChecked == true)
                tiposUser.Add(ckEmpr.Content.ToString());
            if (ckProd.IsChecked == true)
                tiposUser.Add(ckProd.Content.ToString());
            if (ckVend.IsChecked == true)
                tiposUser.Add(ckVend.Content.ToString());
            if (ckRH.IsChecked == true)
                tiposUser.Add(ckRH.Content.ToString());
            user.TiposUser = tiposUser;
            user.Email = txtEmail.Text;
            user.Localizacao = new Localizacao();
            user.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
            user.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
            user.Localizacao.Rua1 = txtRua.Text;
            user.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
            user.HoraEntrada = TimeSpan.Parse(txtEntrada.Text);
            user.HoraSaida = TimeSpan.Parse(txtSaida.Text);

            user.Supervisor = new Utilizador();
            user.Supervisor.NFuncionario = Convert.ToInt32(txtSuper.Text);

            try
            {
                dataHandler.EnviarEmpregado(user);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Empregado Registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (txtPass.Text.Trim().Length < 3 && txtPass.Text.Trim().Length > 30)
                throw new Exception("A pass deverá ter entre 3 a 30 carateres.");
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
            if (txtnFilial.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza o nº da filial onde pretende registar o empregado.");
            if (txtSuper.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza o nº funcionário que será responsável por ser o supervisor deste novo empregado.");
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
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
