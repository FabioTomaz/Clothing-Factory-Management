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
using System.Windows.Markup;


namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarDesenhoBase : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;

        public int CurrentRow
        {
            get
            {
                return currentRow;
            }

            set
            {
                currentRow = value;
            }
        }

        public RegistarDesenhoBase(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarDesenhoBase(Desenho desenhoBase)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            /* cmd.CommandText = "INSERT INTO DESENHO (NOME_DESENHO, DATA_ALTERAÇAO, INSTRUÇÕES_PRODUÇÃO, N_GESTOR_PROD, IMAGEM_DESENHO) " +
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
                 throw new Exception("Falha ao criar encomenda na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
             }
             finally
             {
                 dataHandler.closeSGBDConnection();
             }*/
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Desenho desenhoBase = new Desenho();
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                EnviarDesenhoBase(desenhoBase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }

        private void btnAdicionarImagem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
