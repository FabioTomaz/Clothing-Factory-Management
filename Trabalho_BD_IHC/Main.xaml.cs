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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        DataHandler dataHandler;
        public MainWindow()
        {
            InitializeComponent();
            dataHandler = new DataHandler();
            clientesFrame.Content = new ListarClientes(dataHandler);
            encomendasFrame.Content = new ListarEncomendas(dataHandler);
            produtosFrame.Content = new ListarProdutos(dataHandler);
            materiaisFrame.Content = new ListarMateriais(dataHandler);
            empregadosFrame.Content = new ListarEmpregados(dataHandler);
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            clientesFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}
