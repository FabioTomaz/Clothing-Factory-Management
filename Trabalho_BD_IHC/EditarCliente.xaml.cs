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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarCliente.xaml
    /// </summary>
    public partial class EditarCliente : Page
    {
        DataHandler dataHandler;
        Cliente cliente;
        public EditarCliente(DataHandler dataHandler, Cliente cliente)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.cliente = cliente;
            txtNIF.Text = cliente.Nif;
            labelNome.Content = cliente.Nome;
            String[] split = cliente.CodigoPostal.Split('-');
            txtcodigoPostal1.Text = split[0];
            txtcodigoPostal2.Text = split[1];
            txtEmail.Text = cliente.Email;
            txtNIB.Text = cliente.Nib;
            txtNumeroPorta.Text = cliente.NCasa.ToString();
            txtRua.Text = cliente.Rua;
            txtTelemovel.Text = cliente.Telemovel;
            txtNumeroCliente.Text = cliente.NCliente.ToString();
            txtNIB.Focus();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo a edição do cliente? Perderá todos os dados que tenha atualizado.",
     "Cancelar Registo de Cliente", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void validarInput()
        {
            Regex regex = new Regex("[^a-bA-B]+");
            if (txtNIF.Text.Trim().Length != 21 && txtNIB.Text.Trim().Length != 0)
                throw new Exception("O NIB introduzido está tem um um nº de carateres errado.");
            if (txtNIF.Text.Trim().Length != 9 && txtNIF.Text.Trim().Length != 0)
                throw new Exception("O NIF introduzido está tem um nº de carateres errado.");
            try
            {
                MailAddress m = new MailAddress(txtEmail.Text);
            }
            catch (Exception e)
            {
                throw new Exception("O Email introduzido está incorreto.");
            }
            if (txtTelemovel.Text.Trim().Length != 9)
                throw new Exception("O Telemovel introduzido está incorreto.");
            if (txtcodigoPostal1.Text.Length != 4 || txtcodigoPostal2.Text.Length != 3)
                throw new Exception("O Código Postal introduzido está incorreto.");
            if (txtRua.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a rua do cliente");
            if (txtNumeroPorta.Text.Trim().Length == 0)
                throw new Exception("Por favor introduza a porta do cliente");
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

            try
            {
                cliente.Nif = txtNIF.Text.ToString();
                cliente.Nome = labelNome.Content.ToString();
                cliente.Nib = txtNIB.Text;
                cliente.Telemovel = txtTelemovel.Text;
                cliente.Email = txtEmail.Text;
                cliente.CodigoPostal = txtcodigoPostal1.Text + "-" + txtcodigoPostal2.Text;
                cliente.Rua = txtRua.Text;
                cliente.NCasa = int.Parse(txtNumeroPorta.Text);
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Os dados introduzidos não são validos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                dataHandler.editarCliente(cliente);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Cliente Atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }
    }
}
