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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarEncomenda : Page
    {
        private DataHandler dataHandler;
        private int currentRow=1;

        public int CurrentRow
        {
            get
            {
                return currentRow;
            }

            set
            {
                currentRow = value;
            }
        }

        public RegistarEncomenda(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarEncomenda(Encomenda enc)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            /* cmd.CommandText = "INSERT INTO CLIENTE (NOME, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) " +
                 "VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @COD_POSTAL, @RUA, @N_PORTA);";
             cmd.Parameters.Clear();
             cmd.Parameters.AddWithValue("@NOME", cl.Nome);
             cmd.Parameters.AddWithValue("@NIB", cl.Nib);
             cmd.Parameters.AddWithValue("@NIF", cl.Nif);
             cmd.Parameters.AddWithValue("@EMAIL", cl.Email);
             cmd.Parameters.AddWithValue("@TELEMOVEL", cl.Telemovel);
             cmd.Parameters.AddWithValue("@COD_POSTAL", cl.CodigoPostal);
             cmd.Parameters.AddWithValue("@RUA", cl.Rua);
             cmd.Parameters.AddWithValue("@N_PORTA", cl.NCasa);
             cmd.Connection = dataHandler.Cn;
             try
             {
                 cmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 throw new Exception("Falha ao criar encomenda na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
             }
             finally
             {
                 dataHandler.closeSGBDConnection();
             }*/
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Encomenda encomenda = new Encomenda();
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                EnviarEncomenda(encomenda);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            string referenciastr = String.Format("<WatermarkTextBox xmlns='clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit' Name='txtClienteNif'>\n <WatermarkTextBox.Watermark>\n <StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Orientation='Horizontal'>\n < Image Source = 'nCliente.png' Width = '20' />\n < TextBlock Text = 'Numero do Cliente' Margin = '4,0,0,0' />\n </StackPanel></WatermarkTextBox.Watermark>\n </WatermarkTextBox>\n");
            string searchstr = String.Format(@"<Button VerticalAlignment='Center'> <StackPanel Orientation='Horizontal'><Image Source='searchCliente.png' Width='20' /><TextBlock Text='Pesquisar Produto' Margin='4, 0, 0, 0' /></StackPanel></Button>");
            string removerstr = String.Format(@"<Button VerticalAlignment='Center'> <StackPanel Orientation='Horizontal'><Image Source='delete.png' Width='20' /><TextBlock Text='Remover' Margin='4, 0, 0, 0' /></StackPanel></Button>");
            UIElement referencia = (UIElement)XamlReader.Parse(referenciastr, context);
            UIElement pesquisar = (UIElement)XamlReader.Parse(searchstr, context);
            UIElement remover = (UIElement)XamlReader.Parse(removerstr, context);
            Grid.SetRow(referencia, currentRow);
            Grid.SetColumn(referencia, 1);
            Grid.SetRow(pesquisar, currentRow);
            Grid.SetColumn(pesquisar, 3);
            Grid.SetRow(remover, currentRow);
            Grid.SetColumn(remover, 5);

            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(referencia);
            grid.Children.Add(pesquisar);
            grid.Children.Add(remover);
            currentRow++;
        }
    }
}
