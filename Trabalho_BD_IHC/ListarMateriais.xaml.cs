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

        private void removerMaterial_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = materiais.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende eliminar este Material?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    RemoverMaterial((MaterialTextil)materiais.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<MaterialTextil>)materiais.ItemsSource).RemoveAt(listViewIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void RemoverMaterial(MaterialTextil material)
        {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE MATERIAIS_TÊXTEIS WHERE REFERENCIA_FABRICA=@REFERENCIA ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFERENCIA", material.Referencia);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Material Apagado com sucesso!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(input.Text) || Regex.IsMatch(input.Text, @"^\d+$"))
                materiais.ItemsSource = dataHandler.getMateriais();
            else
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, indique o número da referência de fábrica do material a pesquisar", "Informação", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Xceed.Wpf.Toolkit.MessageBox.Show(dataHandler.adicionarMaterial(((MaterialTextil)materiais.SelectedItem).Referencia, Convert.ToInt32(quantidade.Text)), "Resultado", MessageBoxButton.OK, MessageBoxImage.Information);
            this.refresh();
        }

        public void refresh()
        {
            adicionarMaterial.IsEnabled = false;
            detalhesMaterial.IsEnabled = false;
            materiais.Focus();
            materiais.ItemsSource = dataHandler.getMateriaisFromDB();
        }
    }


}

