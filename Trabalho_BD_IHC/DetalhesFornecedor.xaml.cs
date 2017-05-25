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
using System.Windows.Shapes;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesFornecedor.xaml
    /// </summary>
    public partial class DetalhesFornecedor : Window
    {
        private DataHandler dataHandler;
        private Fornecedor fornecedor;

        public DetalhesFornecedor(DataHandler dataHandler, Fornecedor fornecedor)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.fornecedor = fornecedor;
        }
    }
}
