using System;
using System.Collections.Generic;
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
        public DetalhesProdutoBase(DataHandler dataHandler, int referencia)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.produtoBase = dataHandler.getProdutoBaseFromDB(referencia);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (produtoBase.Pic != null)
            {
                var ms = new MemoryStream();
                Utilizador.loggedUser.Imagem.Save(ms, ImageFormat.Png);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                desenhoImagem.Source = bi;
            }
        }
    }
}
