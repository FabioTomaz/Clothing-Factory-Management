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

        private void updateEncomenda() {
            int rows = 0;
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("UPDATE ENCOMENDA SET DESCONTO=@DESCONTO, DATA_ENTREGA_PREV = @DATEP WHERE N_ENCOMENDA=@ENCOMENDA"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", Encomenda.NEncomenda);
            cmd.Parameters.AddWithValue("@DESCONTO", Encomenda.Desconto);
            cmd.Parameters.AddWithValue("@DATEP", Encomenda.DataPrevistaEntrega);

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o contacto na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                {
                    dataHandler.closeSGBDConnection();
                    MessageBox.Show("Edição da encomenda realizada com sucesso", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                   if (MessageBox.Show("Não foi possivel atualizar a encomenda na base de dados. Deseja tentar novamente?", "Erro", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
                    updateEncomenda();
                   else
                    dataHandler.closeSGBDConnection();
            }
        } 

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Encomenda.Desconto = (double)txtDesconto.Value;
                Encomenda.DataPrevistaEntrega = dataPrevista.SelectedDate.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                updateEncomenda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
    }
}
