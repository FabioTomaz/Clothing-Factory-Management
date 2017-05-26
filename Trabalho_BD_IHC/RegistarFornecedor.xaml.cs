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
using System.Net.Mail;
using System.Globalization;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarFornecedor.xaml
    /// </summary>
    public partial class RegistarFornecedor : Page
    {
        private DataHandler dataHandler;
        public RegistarFornecedor(DataHandler dh)
        {
            InitializeComponent();
            this.dataHandler = dh;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo de Fornecedor? Perderá todos os dados que tenha introduzido.",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Fornecedor Fornecedor = new Fornecedor();
            try
            {
                validarInput();   
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                return;
            }

            Fornecedor.Nome = txtNome.Text;
            Fornecedor.Designacao = txtDesi.Text;
            Fornecedor.NIF_Fornecedor = txtNIF.Text;
            Fornecedor.Telefone = txtTelemovel.Text;
            if(txtFax.Text.Length > 0)
            {
                Fornecedor.Fax = txtFax.Text;
            }
            else
            {
                Fornecedor.Fax = null;
            }
            Fornecedor.Email = txtEmail.Text;
            Fornecedor.Localizacao = new Localizacao();
            Fornecedor.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
            Fornecedor.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
            Fornecedor.Localizacao.Rua1 = txtRua.Text;
            Fornecedor.Localizacao.Porta = Convert.ToInt32(txtNumeroPorta.Text);
            try {
                dataHandler.EnviarFornecedor(Fornecedor);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Fornecedor Registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.NavigationService.GoBack();
        }

        private void validarInput() {
            Regex regex = new Regex("[^a-bA-B]+");
            if (txtNome.Text.Trim().Length>50 || txtNome.Text.Trim().Length < 5)
                throw new Exception("O nome introduzido deverá ter no minimo 5 caracteres e no máximo 50.");
            if (txtDesi.Text.Trim().Length > 100)
                throw new Exception("A designação do fornecedor tem mais de 100 carateres! Seja mais breve ao preencher este campo.");
            if (txtNIF.Text.Trim().Length != 9)
                throw new Exception("O NIF não tem 9 carateres.");
            if (!IsValidEmail(txtEmail.Text.Trim()))
                throw new Exception("O Email introduzido está escrito de forma incorreta.");
            if (txtEmail.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza um email válido.");
            if (txtTelemovel.Text.Trim()[0] == '+')
            {
                if (txtTelemovel.Text.Trim().Length > 13)
                    throw new Exception("O número de telemóvel introduzido tem mais de 13 carateres.");
            }
            else if (Regex.IsMatch(txtTelemovel.Text.Trim()[0].ToString(), @"^\d+$"))
            {
                if (txtTelemovel.Text.Trim().Length > 9)
                    throw new Exception("O número de telemóvel introduzido tem mais de 9 carateres.");
            }
            if (txtFax.Text.Trim().Length > 22)
                throw new Exception("O Fax introduzido tem demasiados carateres.");
            if (txtcodigoPostal1.Text.Length!=4 || txtcodigoPostal2.Text.Length != 3)
                throw new Exception("O Código Postal introduzido está incorreto.");
            if (txtRua.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a rua do Fornecedor");
            if (txtNumeroPorta.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza o número de porta do Fornecedor");
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
