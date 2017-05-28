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
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EnviarCliente(Cliente cl)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO CLIENTE (NOME, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) " +
                "VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @COD_POSTAL, @RUA, @N_PORTA);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", cl.Nome);
            cmd.Parameters.AddWithValue("@NIB", cl.Nib);
            cmd.Parameters.AddWithValue("@NIF", cl.Nif);
            cmd.Parameters.AddWithValue("@EMAIL", cl.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", cl.Telemovel);
            cmd.Parameters.AddWithValue("@COD_POSTAL", cl.CodigoPostal);
            cmd.Parameters.AddWithValue("@RUA", cl.Rua);
            cmd.Parameters.AddWithValue("@N_PORTA", cl.NCasa);
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao adicionar cliente na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar o registo de cliente? Perderá todos os dados que tenha introduzido.",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                ListarProdutos page = new ListarProdutos(dataHandler);
                this.NavigationService.Navigate(page);
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
                EnviarCliente(cliente);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Cliente Registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.NavigationService.GoBack();
        }

        private void validarInput() {
            Regex regex = new Regex("[^a-bA-B]+");
            if (txtNome.Text.Trim().Length>50 || txtNome.Text.Trim().Length < 5)
                throw new Exception("O nome introduzido está incorreto. Deverá ter no minimo 5 caracteres e no maximo 50");
            if (txtNIB.Text.Trim().Length!=0 && txtNIB.Text.Trim().Length != 21)
                throw new Exception("O NIB introduzido está incorreto.");
            if (txtNIB.Text.Trim().Length != 0 && txtNIB.Text.Trim().Length != 21)
                throw new Exception("O NIB introduzido está incorreto.");
            if (txtNIF.Text.Trim().Length !=9)
                throw new Exception("O NIF introduzido está incorreto.");
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

        public bool NibIsValid(string nib)
        {
            Regex nibRegex = new Regex(@"^\d{21}$", RegexOptions.Compiled);
            int result = 0;
            for (int nibIndex = 0; nibIndex < 19; nibIndex++)
            {
                result += Convert.ToInt32(nib[nibIndex].ToString(CultureInfo.InvariantCulture));
                result *= 10;
                result %= 94;
            }
            result = 94 - ((result * 10) % 97);
            return nib.Substring(19).Equals(result.ToString("00"));
        }
    }
}
