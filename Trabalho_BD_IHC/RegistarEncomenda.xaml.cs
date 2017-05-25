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
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarEncomenda : Page
    {
        private DataHandler dataHandler;

        public RegistarEncomenda(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            produtosEncomenda.ItemsSource = new ObservableCollection<ProdutoPersonalizado>();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo da encomenda? Perderá todos os dados que tenha introduzido.",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Encomenda encomenda = new Encomenda();
            encomenda.Cliente = new Cliente();
            encomenda.GestorVendas = new Utilizador();
            DateTime currentDate = DateTime.Now;
            encomenda.Cliente.NCliente = Convert.ToInt32(txtCliente.Text);
            encomenda.DataConfirmacao = currentDate;
            encomenda.Desconto =  Convert.ToInt32(txtDesconto.Value);
            encomenda.GestorVendas.NFuncionario= Utilizador.loggedUser.NFuncionario;
            encomenda.LocalEntrega = localEntrega.SelectedItem.ToString();
            encomenda.DataPrevistaEntrega = dataPrevista.SelectedDate.Value;
            
            try
            {
                dataHandler.EnviarEncomenda(encomenda, produtosEncomenda.Items.Cast<ProdutoPersonalizado>().ToList<ProdutoPersonalizado>());
                Xceed.Wpf.Toolkit.MessageBox.Show("Encomenda Registada Com Sucesso!", "Envio de Encomenda", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
        }
    }
}

