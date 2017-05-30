using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarInfPessoal.xaml
    /// </summary>
    public partial class EditarInfPessoal : Window
    {
        DataHandler dataHandler;
        Utilizador user;
        MainWindow m;
        public EditarInfPessoal(DataHandler dataHandler, Utilizador user, MainWindow m)
        {
            InitializeComponent();
            txtEmail.Text = user.Email;
            txtTelemovel.Text = user.Telemovel;
            this.dataHandler = dataHandler;
            this.user = user;
            this.m = m;
            String[] split = user.Localizacao.CodigoPostal.Split('-');
            txtcodigoPostal1.Text = split[0];
            txtcodigoPostal2.Text = split[1];
            txtRua.Text = user.Localizacao.Rua1;
            txtNumeroPorta.Text = user.Localizacao.Porta.ToString();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar a edição dos dados do empregado? Perderá todos as alterações que tenha feito.",
                "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
               this.Close();
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

            user.Telemovel = txtTelemovel.Text;
            user.Email = txtEmail.Text;
            user.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
            user.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
            user.Localizacao.Rua1 = txtRua.Text;
            user.Localizacao.Porta = Convert.ToInt32(txtNumeroPorta.Text);
            try
            {
                dataHandler.editarInfPessoal(user);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("As suas informações foram editadas com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
        }

        private void validarInput()
        {
            Regex regex = new Regex("[^a-bA-B]+");
          
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m.refresh();
        }
    }
}
