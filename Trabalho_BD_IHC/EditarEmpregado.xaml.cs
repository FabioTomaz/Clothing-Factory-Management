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
    /// Interaction logic for EditarEmpregado.xaml
    /// </summary>
    public partial class EditarEmpregado : Page
    {
        private DataHandler dataHandler;
        private MainWindow main;
        Utilizador u;
        public EditarEmpregado(DataHandler dh, Utilizador u, MainWindow main)
        {
            InitializeComponent();
            this.dataHandler = dh;
            this.u = u;
            this.main = main;
            txtNome.Text = u.Nome;
            txtSalario.Text = u.Salario.ToString();
            txtnFilial.Text = u.Filial.NFilial.ToString();
            txtSuper.Text = u.Supervisor.NFuncionario.ToString();
            txtSaida.SelectedTime = DateTime.Parse(u.HoraSaida.ToString());
            txtEntrada.SelectedTime = DateTime.Parse(u.HoraEntrada.ToString());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(u.TiposUser.Contains("Gestor da Empresa"))
            {
                ckEmpr.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Produção"))
            {
                ckProd.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Vendas"))
            {
                ckVend.IsChecked = true;
            }
            if (u.TiposUser.Contains("Gestor de Recursos Humanos"))
            {
                ckRH.IsChecked = true;
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar a edição dos dados do empregado? Perderá todos as alterações que tenha feito.",
                "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validarInput();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            u.Nome = txtNome.Text;
            u.Salario = Convert.ToDouble(txtSalario.Text);
            u.Filial = new filial();
            u.Filial.NFilial = Convert.ToInt32(txtnFilial.Text);
            List<string> tiposUser = new List<string>();
            if (ckEmpr.IsChecked == true)
                tiposUser.Add(ckEmpr.Content.ToString());
            if (ckProd.IsChecked == true)
                tiposUser.Add(ckProd.Content.ToString());
            if (ckVend.IsChecked == true)
                tiposUser.Add(ckVend.Content.ToString());
            if (ckRH.IsChecked == true)
                tiposUser.Add(ckRH.Content.ToString());
            u.TiposUser = tiposUser;
            u.HoraEntrada = TimeSpan.Parse(txtEntrada.Text);
            u.HoraSaida = TimeSpan.Parse(txtSaida.Text);

            u.Supervisor = new Utilizador();
            u.Supervisor.NFuncionario = Convert.ToInt32(txtSuper.Text);

            try
            {
                dataHandler.atualizarEmpregado(u);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Empregado Editado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void validarInput()
        {
           
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    

               
    }
}
