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
using System.IO;
using System.Drawing.Imaging;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarProdutoBase.xaml
    /// </summary>
    public partial class EditarProdutoBase : Page
    {
        DataHandler dataHandler;
        ProdutoBase ProdutoBase;
        public EditarProdutoBase(DataHandler dataHandler, ProdutoBase ProdutoBase)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.ProdutoBase = ProdutoBase;
            refProduto.Content = ProdutoBase.Referencia;
            txtNomeModelo.Content = ProdutoBase.Nome;
            txtIva.Value = ProdutoBase.IVA1;
            txtInstruçoes.Text = ProdutoBase.InstrProd;
            if (ProdutoBase.Pic != null)
            {
                var ms = new MemoryStream();
                Utilizador.loggedUser.Imagem.Save(ms, ImageFormat.Png);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                imgPhoto.Source = bi;
            }
            txtInstruçoes.Focus();
        }

        private void validar()
        {
            if (txtInstruçoes.Text.Equals(""))
                throw new Exception("Não foram especificadas as instruções de produção deste produto!");
            else if (imgPhoto.Source == null)
                throw new Exception("Não foi introduzida uma foto do desenho do produto!");
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar a atualização do produto? Perderá todos os dados que tenha introduzido.",
     "Cancelar Atualização de Produto", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void AtualizarProdutoBase(ProdutoBase prod)
        {
            int rows = 0;

            if (!dataHandler.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE [PRODUTO-BASE] " + "SET  NOME = @NOME, " + "   DATA_ALTERACAO = @DataAlt, " +
                "IVA = @iva"+"  N_GESTOR_PROD = @N_GestorProd, " + " INSTRUCOES_PRODUCAO = @IntrProd " + "WHERE REFERENCIA = @Referencia";
            cmd.Parameters.Clear(); //falta inserir a imagem do produto!
            cmd.Parameters.AddWithValue("@NOME", prod.Nome);
            cmd.Parameters.AddWithValue("@iva", prod.IVA1);
            cmd.Parameters.AddWithValue("@N_GestorProd", 2); //--> do funcionario q editou
            cmd.Parameters.AddWithValue("@DataAlt", DateTime.Today);
            cmd.Parameters.AddWithValue("@IntrProd", prod.InstrProd);
            cmd.Parameters.AddWithValue("@Referencia", prod.Referencia);
            //cmd.Parameters.AddWithValue("@Imagem", des.);
            cmd.Connection = dataHandler.Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o Produto Base. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Produto atualizado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                   if (MessageBox.Show("Não foi possivel atualizar o Produto Base", "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.Cancel)
                    AtualizarProdutoBase(prod);
                dataHandler.closeSGBDConnection();
            }
        }

        

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            ProdutoBase prod = new ProdutoBase();
            prod.IVA1 = txtIva.Value;
            prod.InstrProd = txtInstruçoes.Text;
            if(imgPhoto.Source!=null)
                prod.Pic = ProdutoBase.Pic = getJPGFromImageControl((BitmapImage)imgPhoto.Source);
            try
            {
                AtualizarProdutoBase(prod);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Produto Base atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
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

        private void removerFoto_Click(object sender, RoutedEventArgs e)
        {
            imgPhoto.Source = null;
        }
    }
}
