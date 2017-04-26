﻿using System;
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
    public partial class RegistarDesenhoBase : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;

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

        public RegistarDesenhoBase(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void EnviarDesenhoBase(Desenho desenhoBase)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            /* cmd.CommandText = "INSERT INTO DESENHO (NOME_DESENHO, DATA_ALTERAÇAO, INSTRUÇÕES_PRODUÇÃO, N_GESTOR_PROD, IMAGEM_DESENHO) " +
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
            Desenho desenhoBase = new Desenho();
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
                EnviarDesenhoBase(desenhoBase);
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

            Grid desenhoBaseGrid = new Grid
            {
                Name = "desenho" + currentRow,
            };
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition
            {
                Width = new GridLength(0.05, GridUnitType.Star)
            };
            ColumnDefinition gridCol3 = new ColumnDefinition();
            ColumnDefinition gridCol4 = new ColumnDefinition
            {
                Width = new GridLength(0.05, GridUnitType.Star)
            };
            ColumnDefinition gridCol5 = new ColumnDefinition();
            desenhoBaseGrid.ColumnDefinitions.Add(gridCol1);
            desenhoBaseGrid.ColumnDefinitions.Add(gridCol2);
            desenhoBaseGrid.ColumnDefinitions.Add(gridCol3);
            desenhoBaseGrid.ColumnDefinitions.Add(gridCol4);
            desenhoBaseGrid.ColumnDefinitions.Add(gridCol5);

            string referenciastr = String.Format("<WatermarkTextBox xmlns='clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit' Name='txtClienteNif'> <WatermarkTextBox.Watermark> Referencia </WatermarkTextBox.Watermark> </WatermarkTextBox>\n");
            string searchstr = String.Format(@"<Button VerticalAlignment='Center'> <StackPanel Orientation='Horizontal'><Image Source='searchCliente.png' Width='20' /><TextBlock Text='Pesquisar Produto' Margin='4, 0, 0, 0' /></StackPanel></Button>");
            string removerstr = String.Format(@"<Button Tag='" + currentRow + "'  VerticalAlignment='Center'> <StackPanel Orientation='Horizontal'><Image Source='delete.png' Width='20' /><TextBlock Text='Remover' Margin='4, 0, 0, 0' /></StackPanel></Button>");
            UIElement referencia = (UIElement)XamlReader.Parse(referenciastr, context);
            Button pesquisar = (Button)XamlReader.Parse(searchstr, context);
            Button remover = (Button)XamlReader.Parse(removerstr, context);
            remover.Click += new RoutedEventHandler(Remover_Click);

            Grid.SetRow(desenhoBaseGrid, currentRow);
            Grid.SetColumn(desenhoBaseGrid, 1);

            Grid.SetColumn(referencia, 0);
            Grid.SetColumn(pesquisar, 2);
            Grid.SetColumn(remover, 4);

            desenhoBaseGrid.Children.Add(referencia);
            desenhoBaseGrid.Children.Add(remover);

            grid.RowDefinitions.Add(new RowDefinition());
            grid.Children.Add(desenhoBaseGrid);

            currentRow++;
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Button senderbtn = (Button)sender;
            int row = Convert.ToInt32(senderbtn.Tag);
            Console.WriteLine("Row: " + row);
            Console.WriteLine("Current Row: " + currentRow);
            for (int i = row + 1; i < currentRow; i++)
            {
                grid.Children.Cast<Grid>().ElementAt(i).Tag = (i - 1).ToString();
                Grid.SetRow(grid.Children.Cast<Grid>().ElementAt(i), i - 1);
            }
            grid.RowDefinitions.RemoveAt(row);
            currentRow--;
            
        }
    }
}
