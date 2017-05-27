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
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for EditarFilial.xaml
    /// </summary>
    public partial class EditarFilial : Page
    {
        DataHandler dataHandler;
        filial filial;
        public EditarFilial(DataHandler dataHandler, filial filial)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.filial = filial;
            txtEmail.Text = filial.Email;
            txtFax.Text = filial.Fax;
            txtTelemovel.Text = filial.Telefone;
            String[] split = filial.Localizacao.CodigoPostal.Split('-');
            txtcodigoPostal1.Text = split[0];
            txtcodigoPostal2.Text = split[1];
            txtNumeroPorta.Text = filial.Localizacao.Porta.ToString();
            txtRua.Text = filial.Localizacao.Rua1;
            txtNChefe.Text = filial.Chefe.NFuncionario.ToString();
            txtEmail.Focus();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }
        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filial.Telefone = txtTelemovel.Text;
                filial.Fax = txtFax.Text;
                filial.Email = txtEmail.Text;
                filial.Localizacao.CodigoPostal1 = Convert.ToInt32(txtcodigoPostal1.Text);
                filial.Localizacao.CodigoPostal2 = Convert.ToInt32(txtcodigoPostal2.Text);
                filial.Localizacao.Rua1 = txtRua.Text;
                filial.Localizacao.Porta = int.Parse(txtNumeroPorta.Text);
                filial.NFilial = dataHandler.getNfilialFromDB(filial.Email, filial.Telefone);
                filial.Chefe.NFuncionario = Convert.ToInt32(txtNChefe.Text);
                RegistarFilial r = new RegistarFilial(dataHandler);
                r.validarInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                dataHandler.AtualizarFilial(filial);
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
