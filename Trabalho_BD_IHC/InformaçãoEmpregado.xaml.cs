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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for InformaçãoEmpregado.xaml
    /// </summary>
   
    public partial class InformaçãoEmpregado : Page
    {
        DataHandler dataHandler;
        Utilizador user;
        public InformaçãoEmpregado(DataHandler dh, Utilizador u)
        {
            InitializeComponent();
            this.dataHandler = dh;
            this.user = u;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

       
    }
}
