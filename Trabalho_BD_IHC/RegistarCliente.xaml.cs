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
    /// Interaction logic for RegistarCliente.xaml
    /// </summary>
    public partial class RegistarCliente : Page
    {
        private DataHandler dataHandler;
        public RegistarCliente(DataHandler dh)
        {
            InitializeComponent();
            this.dataHandler = dh;
            txtNumeroCliente.Text = (dataHandler.getLastIdentity("CLIENTE")+1).ToString();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo de cliente? Perderá todos os dados que tenha introduzido.",
                 "Cancelar Registo de Cliente", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            try
            {
                validarInput();   
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            cliente.Nome = txtNome.Text;
            cliente.Nib = txtNIB.Text;
            cliente.Nif = txtNIF.Text;
            cliente.Telemovel = txtTelemovel.Text;
            cliente.Email = txtEmail.Text;
            cliente.CodigoPostal = txtcodigoPostal1.Text + "-" + txtcodigoPostal2.Text;
            cliente.Rua = txtRua.Text;
            cliente.NCasa = int.Parse(txtNumeroPorta.Text);
            try {
                dataHandler.registarCliente(cliente);
            }catch(Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Cliente Registado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void validarInput() {
            Regex regex = new Regex("[^a-bA-B]+");
            if (txtNome.Text.Trim().Length>50 || txtNome.Text.Trim().Length < 5)
                throw new Exception("O nome introduzido está incorreto. Deverá ter no minimo 5 caracteres e no maximo 50");
            if (txtNIF.Text.Trim().Length != 21 && txtNIB.Text.Trim().Length != 0)
                throw new Exception("O NIB introduzido está tem um nº de carateres errado");
            if (txtNIF.Text.Trim().Length != 9 && txtNIF.Text.Trim().Length != 0)
                throw new Exception("O NIF introduzido está tem um nº de carateres errado");
            try { 
                MailAddress m = new MailAddress(txtEmail.Text);
            }catch(Exception e) { 
                throw new Exception("O Email introduzido está incorreto.");
            }
            if (txtTelemovel.Text.Trim().Length != 9)
                throw new Exception("O Telemovel introduzido está incorreto.");
            if (txtcodigoPostal1.Text.Length!=4 || txtcodigoPostal2.Text.Length != 3)
                throw new Exception("O Código Postal introduzido está incorreto.");
            if (txtRua.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a rua do cliente");
            if (txtNumeroPorta.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a porta do cliente");
        }

       
    }
}
