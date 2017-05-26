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
    /// Interaction logic for RegistarFilial.xaml
    /// </summary>
    public partial class RegistarFilial : Page
    {
        private DataHandler dataHandler;
        public RegistarFilial(DataHandler dh)
        {
            InitializeComponent();
            this.dataHandler = dh;
            txtEmail.Focus();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }



        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar o registo de Filial? Perderá todos os dados que tenha introduzido.",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                ListarProdutos page = new ListarProdutos(dataHandler);
                this.NavigationService.Navigate(page);
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            filial fl = new filial();
            try
            {
                validarInput();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                return;
            }

            fl.Fax = txtFax.Text;
            fl.Telefone = txtTelemovel.Text;
            fl.Email = txtEmail.Text;
            fl.Localizacao = new Localizacao();
            fl.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
            fl.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
            fl.Localizacao.Rua1 = txtRua.Text;
            fl.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
            try
            {
                dataHandler.EnviarFilial(fl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Fábrica Filial registada com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.NavigationService.GoBack();
        }

        private void validarInput()
        {
            Regex regex = new Regex("[^a-bA-B]+");

            if (txtFax.Text.Trim().Length > 22)
                throw new Exception("O fax introduzido tem demasiados carateres.");
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
                if (txtTelemovel.Text.Trim().Length > 22)
                    throw new Exception("O número de telemóvel introduzido tem mais de 9 carateres.");
            }
            if (txtFax.Text.Trim().Length > 22)
                throw new Exception("O Fax introduzido tem demasiados carateres.");
            if (txtcodigoPostal1.Text.Length != 4 || txtcodigoPostal2.Text.Length != 3)
                throw new Exception("O Código Postal introduzido está incorreto.");
            if (txtRua.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a rua da Fábrica Filial.");
            if (txtNumeroPorta.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza o número de porta da Fábrica filial.");
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
