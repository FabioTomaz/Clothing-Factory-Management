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
    public partial class EditarDesenhoBase : Page
    {
        DataHandler dataHandler;
        DesenhoBase desenhoBase;
        public EditarDesenhoBase(DataHandler dataHandler, DesenhoBase desenhoBase)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.desenhoBase = desenhoBase;
            labelNDesenho.Content = desenhoBase.NDesenho;
            txtNomeDesenho.Text = desenhoBase.Nome;
            txtNGestProd.Text = desenhoBase.GestorProducao.NFuncionario.ToString();
            txtInstrProd.Text = desenhoBase.InstrucoesProducao;
            imgPhoto.Source = desenhoBase.Pic;
            txtNomeDesenho.Focus();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void AtualizarDesenhoBase(DesenhoBase des)
        {
            int rows = 0;

            if (!dataHandler.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE DESENHO " + "SET  NOME_DESENHO = @NOME, " + "   DATA_ALTERACAO = @DataAlt, " +
                "  N_GESTOR_PROD = @N_GestorProd, " + " INSTRUCOES_PRODUCAO = @IntrProd " + "WHERE N_DESENHO = @NDesenho";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", des.Nome);
            cmd.Parameters.AddWithValue("@N_GestorProd", des.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@DataAlt", DateTime.Today);
            cmd.Parameters.AddWithValue("@IntrProd", des.InstrucoesProducao);
            cmd.Parameters.AddWithValue("@NDesenho", des.NDesenho);
            //cmd.Parameters.AddWithValue("@Imagem", des.);
            cmd.Connection = dataHandler.Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o Desenho Base. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Desenho atualizado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                   if (MessageBox.Show("Não foi possivel atualizar o Desenho Base", "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
                    AtualizarDesenhoBase(des);
                dataHandler.closeSGBDConnection();
            }
        }

        

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                desenhoBase.Nome = txtNomeDesenho.Text;
                desenhoBase.GestorProducao.NFuncionario = int.Parse(txtNGestProd.Text);
                desenhoBase.InstrucoesProducao = txtInstrProd.Text;
                desenhoBase.NDesenho = int.Parse(labelNDesenho.Content.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                AtualizarDesenhoBase(desenhoBase);
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
