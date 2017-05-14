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
        private ImageSource pic;
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

        public ImageSource Pic
        {
            get
            {
                return pic;
            }

            set
            {
                pic = value;
            }
        }

        public RegistarDesenhoBase(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarDesenhoBase(DesenhoBase desenhoBase)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO DESENHO (NOME_DESENHO, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) "
                +  "SELECT (@nome_desenho, @Data_alteracao, @instr, @nGestor, @imagem);";
             cmd.Parameters.Clear();
             cmd.Parameters.AddWithValue("@nome_desenho", desenhoBase.Nome);
             cmd.Parameters.AddWithValue("@Data_alteracao", DateTime.Today);
             cmd.Parameters.AddWithValue("@instr", desenhoBase.InstrucoesProducao);
             cmd.Parameters.AddWithValue("@nGestor", desenhoBase.GestorProducao.NFuncionario);
             cmd.Parameters.AddWithValue("@imagem", desenhoBase.Pic);
             cmd.Connection = dataHandler.Cn;
             try
             {
                 cmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 throw new Exception("Falha ao registar o Desenho Base na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
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
            DesenhoBase desenhoBase = new DesenhoBase();
            try
            {
                desenhoBase.Nome = txtNomeDesenhoBase.Text;
                desenhoBase.InstrucoesProducao = txtInstruçoes.Text;
                desenhoBase.GestorProducao = new Utilizador();
                desenhoBase.GestorProducao.NFuncionario = 2; //---> suposto mais tarde colocar o nº do user
                desenhoBase.Pic = Pic;
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
            MessageBox.Show("Desenho Base registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
