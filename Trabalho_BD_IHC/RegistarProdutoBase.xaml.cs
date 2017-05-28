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
            refProduto.Content = dataHandler.getLastIdentity("[PRODUTO-BASE]") + 1;
        }

        private void validar()
        {
            if (txtNomeModelo.Text.Equals(""))
                throw new Exception("Não foi escolhido um nome para o produto!");
            else if (txtInstruçoes.Text.Equals(""))
                throw new Exception("Não foram especificadas as instruções de produção deste produto!");
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try { 
                validar();
            }catch(Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ProdutoBase ProdutoBase = new ProdutoBase();
                ProdutoBase.Nome = txtNomeModelo.Text;
                ProdutoBase.InstrProd = txtInstruçoes.Text;
                ProdutoBase.IVA1 = txtIva.Value;
                ProdutoBase.GestorProducao = new Utilizador();
                ProdutoBase.GestorProducao.NFuncionario = 2; //---> suposto mais tarde colocar o nº do user
                if(imgPhoto.Source!=null)
                    ProdutoBase.Pic = BitMapToByte((BitmapImage)imgPhoto.Source);
            try
            {
                dataHandler.registarProdutoBase(ProdutoBase);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("A informação do produto foi registada com sucesso!", "SUCESSO", MessageBoxButton.OK, MessageBoxImage.Information);
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
