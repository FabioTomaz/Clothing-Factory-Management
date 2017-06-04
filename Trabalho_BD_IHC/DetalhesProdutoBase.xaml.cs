using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Shapes;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesProdutoBase.xaml
    /// </summary>
    public partial class DetalhesProdutoBase : Window
    {
        private DataHandler dataHandler;
        private ProdutoBase produtoBase;
        private ObservableCollection<ProdutoPersonalizado> produtosPersonalizados; 
        public DetalhesProdutoBase(DataHandler dataHandler, int referencia)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.produtoBase = dataHandler.getProdutoBaseFromDBWithRef(referencia);
            nomeProduto.Text = produtoBase.Nome;
            ivaProduto.Text = produtoBase.IVA1.ToString();
            refProduto.Text = produtoBase.Referencia.ToString();
            gestorProduto.Text = produtoBase.GestorProducao.NFuncionario.ToString();
            instrucoesProduto.Text = produtoBase.InstrProd;
            dataProduto.Text = produtoBase.DataAlteraçao.ToString("dd/MM/yyyy");
            produtosPersonalizados=dataHandler.getProdutosPersonalizadosFromProdutoBaseDB(referencia);
            produtosPers.ItemsSource = produtosPersonalizados;
            imagemDesenho.Source = LoadImage(produtoBase.Pic);

        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ProdutoPersonalizado produtoSelecionado = ((ProdutoPersonalizado)produtosPers.SelectedItem);
            if (produtoSelecionado != null)
            {
                DetalhesProdutoPersonalizado window = new DetalhesProdutoPersonalizado(dataHandler, (int)produtoSelecionado.ProdutoBase.Referencia, produtoSelecionado.Tamanho, produtoSelecionado.Cor, (int)produtoSelecionado.ID);
                window.Show();
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2 && imagemDesenho.Source != null)
            {
                Imagem window = new Imagem((BitmapImage)imagemDesenho.Source);
                window.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Imagem window = new Imagem((BitmapImage)imagemDesenho.Source);
            window.Show();
        }
    }
}
