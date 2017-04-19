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
using System.Windows.Navigation;
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
        public MainWindow()
        {
            InitializeComponent();
            clientesFrame.Content = new ListarClientes();
        }

        private void registarCliente_Click(object sender, RoutedEventArgs e)
        {
            clientesFrame.Content = new RegistarCliente();
        }

        private void myFrame_ContentRendered(object sender, EventArgs e)
        {
            clientesFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}
