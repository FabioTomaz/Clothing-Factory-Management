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
    public partial class RegistarProdutoPersonalizado : Page
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


        public RegistarProdutoPersonalizado(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarProdutoPersonalizado(ProdutoPersonalizado ProdutoPersonalizado)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            /*
            cmd.CommandText = "INSERT INTO Produto (NOME, IVA, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) "
                + "SELECT @nome_Produto, @iva, @Data_alteracao, @instr, @nGestor, BulkColumn "
                + "FROM Openrowset (Bulk @imagem, Single_Blob) as Image";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome_Produto", ProdutoPersonalizado.Nome);
            cmd.Parameters.AddWithValue("@iva", ProdutoPersonalizado.IVA1);
             cmd.Parameters.AddWithValue("@Data_alteracao", DateTime.Today);
            cmd.Parameters.AddWithValue("@instr", ProdutoPersonalizado.InstrProd);
            cmd.Parameters.AddWithValue("@nGestor", ProdutoPersonalizado.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@imagem", ProdutoPersonalizado.Pic).SqlDbType = SqlDbType.Binary;
            cmd.Connection = dataHandler.Cn;*/
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o Produto Personalizado na Personalizado de dados. \n ERROR MESSAGE: \n" + ex.Message);
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
            ProdutoPersonalizado ProdutoPersonalizado = new ProdutoPersonalizado();
            try
            {
               /* ProdutoPersonalizado.Nome = txtNomeProdutoPersonalizado.Text;
                ProdutoPersonalizado.InstrProd = txtInstruçoes.Text;
                ProdutoPersonalizado.IVA1 = Convert.ToDouble(txtIva.Text);
                ProdutoPersonalizado.GestorProducao = new Utilizador();
                ProdutoPersonalizado.GestorProducao.NFuncionario = 2; //---> suposto mais tarde colocar o nº do user
                ProdutoPersonalizado.Pic = BitMapToByte((BitmapImage)imgPhoto.Source);*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                EnviarProdutoPersonalizado(ProdutoPersonalizado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Produto Personalizado registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }



    }
}
