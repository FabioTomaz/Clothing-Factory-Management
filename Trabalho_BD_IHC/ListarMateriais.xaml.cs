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
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarMateriais.xaml
    /// </summary>
    public partial class ListarMateriais : Page
    {
        private DataHandler dataHandler;
        public ListarMateriais(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            adicionarMaterial.IsEnabled = false;
            detalhesMaterial.IsEnabled = false;
            materiais.Focus();
            materiais.ItemsSource = dataHandler.getMateriaisFromDB();
        }

        private void materiais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (materiais.SelectedItems.Count > 0)
            {
                adicionarMaterial.IsEnabled = true;
                detalhesMaterial.IsEnabled = true;
            }
        }

        private void Encomenda_Click(object sender, RoutedEventArgs e)
        {
            RegistarMaterial page = new RegistarMaterial(dataHandler);
            NavigationService.Navigate(page);
        }


        private void detalhesMaterial_Click(object sender, RoutedEventArgs e)
        {
            DetalhesMaterial window = new DetalhesMaterial(dataHandler, (MaterialTextil)materiais.SelectedItem);
            window.Show();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //procurar materiais
        {
            if (txtRef.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(input.Text) && Regex.IsMatch(input.Text, @"^\d+$"))
                    materiais.ItemsSource = dataHandler.getMaterialFromDBRef(Convert.ToInt32(input.Text));
                else
                    materiais.ItemsSource = dataHandler.getMateriaisFromDB();
            }
            else if (txtCor.IsChecked == true)
            {
                materiais.ItemsSource = dataHandler.getMaterialFromDBCor(input.Text);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(dataHandler.adicionarMaterial(((MaterialTextil)materiais.SelectedItem).Referencia, Convert.ToDouble(quantidade.Text.Replace('.',','))), "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
            this.refresh();
        }

        public void refresh()
        {
            adicionarMaterial.IsEnabled = false;
            detalhesMaterial.IsEnabled = false;
            materiais.Focus();
            materiais.ItemsSource = dataHandler.getMateriaisFromDB();
        }

        private void materiais_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetalhesMaterial window = new DetalhesMaterial(dataHandler, (MaterialTextil)materiais.SelectedItem);
            window.Show();
        }

        private void txtRef_Checked(object sender, RoutedEventArgs e)
        {
            input.Text = "";
            input.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Referencia do Material");
        }

        private void txtCor_Checked(object sender, RoutedEventArgs e)
        {
            input.Text = "";
            input.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Cor do Material");
        }

        private void txtDes_Checked(object sender, RoutedEventArgs e)
        {
            input.Text = "";
            input.SetValue(MaterialDesignThemes.Wpf.HintAssist.HintProperty, "Pesquisar Por Descrição do Material");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            input.Text = "";
            this.refresh();
        }
    }


}

