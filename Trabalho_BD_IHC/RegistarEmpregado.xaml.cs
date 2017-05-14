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

        private void EnviarEmpregado(Utilizador user)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Utilizidador (NOME, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) " +
                "VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @COD_POSTAL, @RUA, @N_PORTA);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", user.Nome);
            cmd.Parameters.AddWithValue("@EMAIL", user.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", user.Telemovel);
            cmd.Parameters.AddWithValue("@COD_POSTAL", user.Localizacao.CodigoPostal);
            cmd.Parameters.AddWithValue("@RUA", user.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@N_PORTA", user.Localizacao.Porta);
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao adicionar empregado na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Utilizador user = new Utilizador();
            try
            {
                user.Nome = txtNome.Text;
                user.Telemovel = txtTelemovel.Text;
                user.Email = txtEmail.Text;
                user.Localizacao.CodigoPostal = txtcodigoPostal.Text;
                user.Localizacao.Rua1 = txtRua.Text;
                user.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try {
                EnviarEmpregado(user);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
    }
}
