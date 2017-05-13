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
        private int currentRow = 0;

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
        private void EnviarEncomenda()
        {
            
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            DateTime currentDate = DateTime.Now;

            cmd.Parameters.Clear();
            cmd.CommandText = "Insert into ENCOMENDA(ESTADO, DESCONTO, DATA_CONFIRMACAO, DATA_ENTREGA_PREV ,N_GESTOR_VENDA, CLIENTE) values(1, @DESCONTO, @DATEC, @DATEP ,@GESTOR, @CLIENTE);";
            cmd.Parameters.AddWithValue("@DATEC", currentDate.ToString("dd-MM-yyyy"));
            cmd.Parameters.AddWithValue("@DESCONTO", txtDesconto.Value);
            cmd.Parameters.AddWithValue("@GESTOR", 1);
            cmd.Parameters.AddWithValue("@CLIENTE", txtCliente.Text);
            cmd.Parameters.AddWithValue("@DATEP", dataPrevista.Text.ToString());
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
            }

            dataHandler.verifySGBDConnection();
            SqlCommand lastCMD = new SqlCommand();
            lastCMD.CommandText = "Select max(N_ENCOMENDA) from ENCOMENDA;";
            SqlDataReader reader = lastCMD.ExecuteReader();
            reader.Read();
            int encomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
            dataHandler.closeSGBDConnection();

            for (int i = 0; i < currentRow; i++) {
                dataHandler.verifySGBDConnection();
                SqlCommand values = new SqlCommand();
                StackPanel sck = (StackPanel)stack.Children[i];
                String referencia = ((Xceed.Wpf.Toolkit.WatermarkTextBox)sck.Children[0]).Text;
                String cor = ((Xceed.Wpf.Toolkit.WatermarkTextBox)sck.Children[1]).Text;
                String tamanho = ((Xceed.Wpf.Toolkit.WatermarkTextBox)sck.Children[2]).Text;
                int? quantidade = ((Xceed.Wpf.Toolkit.IntegerUpDown)sck.Children[3]).Value;
                values.Parameters.Clear();
                values.Parameters.AddWithValue("@ENCOMENDA", encomenda);
                values.Parameters.AddWithValue("@REFERENCIA", referencia);
                values.Parameters.AddWithValue("@TAMANHO", tamanho);
                values.Parameters.AddWithValue("@COR", cor);
                values.Parameters.AddWithValue("@QUANTIDADE", quantidade);
                values.CommandText = "INSERT INTO [CONTEUDO-ENCOMENDA](N_ENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, QUANTIDADE) VALUES(@ENCOMENDA, @REFERENCIA, @TAMANHO, @COR, @QUANTIDADE)";
                values.Connection = dataHandler.Cn;
                try
                {
                    values.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao associar produtos á encomenda na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
                finally
                {
                    dataHandler.closeSGBDConnection();
                }
            }

        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                EnviarEncomenda();
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

            StackPanel stackProduto = new StackPanel {
                Orientation = Orientation.Horizontal,
                Name="produto"+currentRow,
            };

            StackPanel refPanel = new StackPanel { Orientation=Orientation.Horizontal};
            refPanel.Children.Add(new Image {Source= new BitmapImage(new Uri("referencia.png", UriKind.Relative)), Width=20 });
            refPanel.Children.Add(new TextBlock { Text = "Referencia", Margin = new Thickness(4,0,0,0) });
            Xceed.Wpf.Toolkit.WatermarkTextBox referencia = new Xceed.Wpf.Toolkit.WatermarkTextBox {
                Name = "txtRef"+currentRow,
                Watermark = refPanel,
                Width=100
            };
            StackPanel corPanel = new StackPanel { Orientation = Orientation.Horizontal };
            corPanel.Children.Add(new Image { Source = new BitmapImage(new Uri("color.png", UriKind.Relative)), Width = 20 });
            corPanel.Children.Add(new TextBlock { Text = "Cor", Margin = new Thickness(4, 0, 0, 0) });
            Xceed.Wpf.Toolkit.WatermarkTextBox cor = new Xceed.Wpf.Toolkit.WatermarkTextBox
            {
                Name = "txtCor"+currentRow,
                Watermark = corPanel,
                Width = 100
            };
            StackPanel tamanhoPanel = new StackPanel { Orientation = Orientation.Horizontal };
            tamanhoPanel.Children.Add(new Image { Source = new BitmapImage(new Uri("size.png", UriKind.Relative)), Width = 20 });
            tamanhoPanel.Children.Add(new TextBlock { Text = "Tamanho", Margin = new Thickness(4, 0, 0, 0) });
            Xceed.Wpf.Toolkit.WatermarkTextBox tamanho = new Xceed.Wpf.Toolkit.WatermarkTextBox
            {
                Name = "txtTamanho"+currentRow,
                Watermark = tamanhoPanel,
                Width = 100
            };

            StackPanel quantidadePanel = new StackPanel { Orientation = Orientation.Horizontal };
            quantidadePanel.Children.Add(new Image { Source = new BitmapImage(new Uri("quantity.png", UriKind.Relative)), Width = 20 });
            quantidadePanel.Children.Add(new TextBlock { Text = "Quantidade", Margin = new Thickness(4, 0, 0, 0) });
            Xceed.Wpf.Toolkit.IntegerUpDown quantidade = new Xceed.Wpf.Toolkit.IntegerUpDown
            {
                Name = "txtQuantidade"+currentRow,
                Minimum=1,
                Watermark = quantidadePanel
            };

            StackPanel removerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            removerPanel.Children.Add(new Image { Source = new BitmapImage(new Uri("delete.png", UriKind.Relative)), Width = 20 });
            removerPanel.Children.Add(new TextBlock { Text = "Remover Item", Margin = new Thickness(4, 0, 0, 0) });
            Button remover = new Button {
                Tag=currentRow,
                Width=120,
                Content=removerPanel
            };
            remover.Click += new RoutedEventHandler(Remover_Click);

            
            stackProduto.Children.Add(referencia);
            stackProduto.Children.Add(cor);
            stackProduto.Children.Add(tamanho);
            stackProduto.Children.Add(quantidade);
            stackProduto.Children.Add(remover);

            stack.Children.Add(stackProduto);

            currentRow++;
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Button senderbtn = (Button)sender;
            int row = Convert.ToInt32(senderbtn.Tag);
            
            for (int i = row + 1; i < currentRow; i++)
            {
                Button produtoBtn= (Button)((StackPanel)stack.Children[i]).Children[2];
                produtoBtn.Tag = i - 1;
            }
            stack.Children.RemoveAt(row);
            currentRow--;
        }
    }
}

