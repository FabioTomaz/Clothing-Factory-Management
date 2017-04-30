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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Markup;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarMaterial : Page
    {
        private DataHandler dataHandler;


        public RegistarMaterial(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void SubmeterRegistoMaterial(MaterialTextil mat)
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



        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {

            if (IsLoaded)
            {
                ComboBox caller = (ComboBox)sender;
                if (caller.Name.ToString().Equals("tipoMaterial", StringComparison.Ordinal))
                {
                    ComboBoxItem cbo = (ComboBoxItem)tipoMaterial.SelectedItem;
                    StackPanel stack = (StackPanel)cbo.Content;
                   
                    if ((((TextBlock)stack.Children[1]).Text).Equals("Acessórios Costura", StringComparison.Ordinal))
                    {
                        acessoriosLabel.Visibility = Visibility.Visible;
                        acessorios.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        acessoriosLabel.Visibility = Visibility.Hidden;
                        acessorios.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stack.Children[1]).Text).Equals("Pano", StringComparison.Ordinal))
                    {
                        pano.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        pano.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stack.Children[1]).Text).Equals("Linha", StringComparison.Ordinal))
                    {
                        linha.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        linha.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stack.Children[1]).Text).Equals("Acessórios Costura", StringComparison.Ordinal))
                    {
                        fecho.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        fecho.Visibility = Visibility.Hidden;
                    }
                }
                else if (caller.Name.ToString().Equals("acessorios", StringComparison.Ordinal))
                {
                    ComboBoxItem cboAc = (ComboBoxItem)acessorios.SelectedItem;
                    StackPanel stackAc = (StackPanel)cboAc.Content;
                    if ((((TextBlock)stackAc.Children[1]).Text).Equals("Fecho", StringComparison.Ordinal))
                    {
                        fecho.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        fecho.Visibility = Visibility.Hidden;
                    }

                    if ((((TextBlock)stackAc.Children[1]).Text).Equals("Mola", StringComparison.Ordinal))
                    {
                        mola.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        mola.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stackAc.Children[1]).Text).Equals("Botão", StringComparison.Ordinal))
                    {
                        botao.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        botao.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stackAc.Children[1]).Text).Equals("Elástico", StringComparison.Ordinal))
                    {
                        elastico.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        elastico.Visibility = Visibility.Hidden;
                    }
                    if ((((TextBlock)stackAc.Children[1]).Text).Equals("Fita de velcro", StringComparison.Ordinal))
                    {
                        fitaVelcro.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        fitaVelcro.Visibility = Visibility.Hidden;
                    }
                }
            }
        }


        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            MaterialTextil material = new MaterialTextil();
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
                SubmeterRegistoMaterial(material);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

    }
}

