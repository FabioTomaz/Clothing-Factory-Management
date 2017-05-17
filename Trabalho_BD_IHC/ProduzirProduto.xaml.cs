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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ProduzirProduto.xaml
    /// </summary>
    public partial class ProduzirProduto : Page
    {
        private DataHandler dataHandler;
        private ProdutoPersonalizado prodPers;
        public ProduzirProduto(DataHandler dataHandler, ProdutoPersonalizado prodPers)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.prodPers = prodPers;
            nomeProduto.Text = prodPers.ProdutoBase.Nome.ToString();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
