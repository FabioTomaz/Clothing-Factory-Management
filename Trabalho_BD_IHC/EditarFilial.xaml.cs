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
    /// Interaction logic for EditarFilial.xaml
    /// </summary>
    public partial class EditarFilial : Page
    {
        DataHandler dataHandler;
        filial filial;
        public EditarFilial(DataHandler dataHandler, filial filial)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.filial = filial;
            txtEmail.Text = filial.Email;
            txtFax.Text = filial.Fax;
            txtTelemovel.Text = filial.Telefone;
            String[] split = filial.Localizacao.CodigoPostal.Split('-');
            txtcodigoPostal1.Text = split[0];
            txtcodigoPostal2.Text = split[1];
            txtNumeroPorta.Text = filial.Localizacao.Porta.ToString();
            txtRua.Text = filial.Localizacao.Rua1;
            txtNChefe.Text = filial.Chefe.NFuncionario.ToString();
            txtEmail.Focus();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filial.Telefone = txtTelemovel.Text;
                filial.Fax = txtFax.Text;
                filial.Email = txtEmail.Text;
                filial.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
                filial.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
                filial.Localizacao.Rua1 = txtRua.Text;
                filial.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
                filial.NFilial = dataHandler.getNfilialFromDB(filial.Email, filial.Telefone);
                filial.Chefe.NFuncionario = Convert.ToInt32(txtNChefe.Text);
                validarInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                dataHandler.AtualizarFilial(filial);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
        public void validarInput()
        {
            Regex regex = new Regex("[^a-bA-B]+");

            if (txtFax.Text.Trim().Length > 22)
                throw new Exception("O fax introduzido tem demasiados carateres.");
            if (!IsValidEmail(txtEmail.Text.Trim()))
                throw new Exception("O Email introduzido está escrito de forma incorreta.");
            if (txtEmail.Text.Trim().Length == 0 || txtEmail.Text.Trim().Length > 50)
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
            if (txtNChefe.Text.Trim().Length == 0)
                throw new Exception("Por favor, introduza um número válido do chefe da filial a registar.");
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
