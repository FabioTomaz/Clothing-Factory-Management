using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesFornecedor.xaml
    /// </summary>
    public partial class DetalhesFornecedor : Window
    {
        private DataHandler dataHandler;
        private Fornecedor fornecedor;

        public DetalhesFornecedor(DataHandler dataHandler, Fornecedor fornecedor)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.fornecedor = fornecedor;
            nif.Content = fornecedor.NIF_Fornecedor;
            nome.Content = fornecedor.Nome;
            email.Content = fornecedor.Email;
            telemovel.Content = fornecedor.Telefone;
            designacao.Text = fornecedor.Designacao;
            cdgPostal.Content = fornecedor.Localizacao.CodigoPostal;
            distrito.Content = fornecedor.Localizacao.Distrito;
            localidade.Content = fornecedor.Localizacao.Localidade;
            morada.Content = fornecedor.Localizacao.Rua1 + ", nº " + fornecedor.Localizacao.Porta;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            materiais.ItemsSource = dataHandler.getMateriaisFornecedorFromDB(Convert.ToInt32(fornecedor.NIF_Fornecedor));
        }

        private void materiais_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DetalhesMaterial window = new DetalhesMaterial(dataHandler, (MaterialTextil)materiais.SelectedItem);
            window.Show();
        }
    }
}
