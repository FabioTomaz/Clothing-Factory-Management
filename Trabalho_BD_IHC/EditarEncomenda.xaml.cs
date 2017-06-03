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
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarEncomenda.xaml
    /// </summary>
    public partial class EditarEncomenda : Page
    {
        DataHandler dataHandler;
        Encomenda Encomenda;
        public EditarEncomenda(DataHandler dataHandler, Encomenda Encomenda)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.Encomenda = Encomenda;
            nEncomenda.Text = Encomenda.NEncomenda.ToString();
            txtDesconto.Value = Convert.ToInt32(Encomenda.Desconto);
            txtCliente.Text = ((Cliente)Encomenda.Cliente).NCliente.ToString();
            if (Encomenda.LocalEntrega.Equals("Entrega ao Domicilio"))
                localEntrega.SelectedIndex = 0;
            else
                localEntrega.SelectedIndex = 1;
            dataPrevista.SelectedDate = Encomenda.DataPrevistaEntrega;
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar a edição da encomenda? Perderá todos os dados que tenha introduzido.",
     "Cancelar Registo de Cliente", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        public void validar()
        {
            if (dataPrevista.SelectedDate == null)
                throw new Exception("Não foi selecionada nenhuma data de entrega prevista!");
            else if(localEntrega.SelectedItem==null)
                throw new Exception("Não foi selecionado nenhum local para entregar a encomenda!");
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
            Encomenda.Desconto = (double)txtDesconto.Value;
            Encomenda.DataPrevistaEntrega = dataPrevista.SelectedDate.Value;
            Encomenda.LocalEntrega = localEntrega.SelectedItem.ToString();
            try
            {
                dataHandler.atualizarEncomenda(Encomenda);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Encomenda Editada com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }
    }
}
