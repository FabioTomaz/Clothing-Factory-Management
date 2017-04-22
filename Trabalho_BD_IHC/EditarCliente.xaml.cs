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
            labelNIF.Content = cliente.Nif;
            labelNome.Content = cliente.Nome;
            txtcodigoPostal.Text = cliente.CodigoPostal;
            txtEmail.Text = cliente.Email;
            txtNIB.Text = cliente.Nib;
            txtNumeroPorta.Text = cliente.NCasa.ToString();
            txtRua.Text = cliente.Rua;
            txtTelemovel.Text = cliente.Telemovel;
            txtNIB.Focus();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void AtualizarCliente(Cliente C)
        {
            int rows = 0;

            if (!dataHandler.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE Cliente " + "SET NIF = @NIF, " + "    NOME = @NOME, " + "    NIB = @NIB, " + "    EMAIL = @EMAIL, " + "    TELEMOVEL = @TELEMOVEL, " + "    COD_POSTAL = @COD_POSTAL, " + "    RUA = @RUA, " + " N_PORTA = @N_PORTA " + "WHERE NIF = @NIF";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NIF", C.Nif);
            cmd.Parameters.AddWithValue("@NOME", C.Nome);
            cmd.Parameters.AddWithValue("@NIB", C.Nib);
            cmd.Parameters.AddWithValue("@EMAIL", C.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", C.Telemovel);
            cmd.Parameters.AddWithValue("@COD_POSTAL", C.CodigoPostal);
            cmd.Parameters.AddWithValue("@RUA", C.Rua);
            cmd.Parameters.AddWithValue("@N_PORTA", C.NCasa);
            cmd.Connection = dataHandler.Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update contact in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Atualização realizada com sucesso", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                   if (MessageBox.Show("Não foi possivel atualizar o perfil do cliente", "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
                    AtualizarCliente(C);
                dataHandler.closeSGBDConnection();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cliente.Nif = labelNIF.Content.ToString();
                cliente.Nome = labelNome.Content.ToString();
                cliente.Nib = txtNIB.Text;
                cliente.Telemovel = txtTelemovel.Text;
                cliente.Email = txtEmail.Text;
                cliente.CodigoPostal = txtcodigoPostal.Text;
                cliente.Rua = txtRua.Text;
                cliente.NCasa = int.Parse(txtNumeroPorta.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                AtualizarCliente(cliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
    }
}
