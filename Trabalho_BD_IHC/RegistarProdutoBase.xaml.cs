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
using System.IO;
using System.Data;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarProdutoBase : Page
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


        public RegistarProdutoBase(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarProdutoBase(ProdutoBase ProdutoBase)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Produto (NOME, IVA, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) "
                + "SELECT @nome_Produto, @iva, @Data_alteracao, @instr, @nGestor, BulkColumn "
                + "FROM Openrowset (Bulk @imagem, Single_Blob) as Image";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome_Produto", ProdutoBase.Nome);
            cmd.Parameters.AddWithValue("@iva", ProdutoBase.IVA1);
             cmd.Parameters.AddWithValue("@Data_alteracao", DateTime.Today);
            cmd.Parameters.AddWithValue("@instr", ProdutoBase.InstrProd);
            cmd.Parameters.AddWithValue("@nGestor", ProdutoBase.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@imagem", ProdutoBase.Pic).SqlDbType = SqlDbType.Binary;
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o Produto Base na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
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
            ProdutoBase ProdutoBase = new ProdutoBase();
            try
            {
                ProdutoBase.Nome = txtNomeProdutoBase.Text;
                ProdutoBase.InstrProd = txtInstruçoes.Text;
                ProdutoBase.IVA1 = Convert.ToDouble(txtIva.Text);
                ProdutoBase.GestorProducao = new Utilizador();
                ProdutoBase.GestorProducao.NFuncionario = 2; //---> suposto mais tarde colocar o nº do user
                ProdutoBase.Pic = BitMapToByte((BitmapImage)imgPhoto.Source);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                EnviarProdutoBase(ProdutoBase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Produto Base registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public Byte[] BitMapToByte(BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            Byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }

            return buffer;
        }


    }
}
