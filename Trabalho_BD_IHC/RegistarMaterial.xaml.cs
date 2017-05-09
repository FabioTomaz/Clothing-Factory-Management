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
        private void EnviarMaterial(MaterialTextil mat)
        {

            if (!dataHandler.verifySGBDConnection()) { 
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = dataHandler.Cn;
            cmd.Parameters.Clear();
            Console.WriteLine(mat.Cor);
            cmd.Parameters.AddWithValue("@COR", mat.Cor);
            cmd.Parameters.AddWithValue("@FORNECEDORREF", mat.ReferenciaFornecedor);
            cmd.Parameters.AddWithValue("@FORNECEDOR", mat.Fornecedor);
            cmd.Parameters.AddWithValue("@DESIGNACAO", mat.Designacao);
            cmd.CommandText = "INSERT INTO MATERIAIS_TÊXTEIS (REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO) " +
"VALUES (@FORNECEDORREF, @FORNECEDOR, @COR, @DESIGNACAO);";

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

            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SqlCommand getRef = new SqlCommand();
            getRef.Connection = dataHandler.Cn;
            getRef.Parameters.AddWithValue("REFERENCIA_FORN", mat.ReferenciaFornecedor);
            getRef.Parameters.AddWithValue("NIF_FORNECEDOR", mat.Fornecedor);
            getRef.CommandText = "SELECT REFERENCIA_FABRICA FROM MATERIAIS_TÊXTEIS WHERE REFERENCIA_FORN=@REFERENCIA_FORN AND NIF_FORNECEDOR=@NIF_FORNECEDOR";
            SqlDataReader reader = getRef.ExecuteReader();
            while(reader.Read())
                mat.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
            dataHandler.closeSGBDConnection();
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (mat.GetType() == typeof(Pano)) {
                Pano pano = (Pano)mat;
                SqlCommand panocmd = new SqlCommand();
                panocmd.Connection = dataHandler.Cn;
                panocmd.Parameters.Clear();
                panocmd.Parameters.AddWithValue("@REFERENCIA", pano.Referencia);
                panocmd.Parameters.AddWithValue("@GRAMAGEM", pano.Gramagem);
                panocmd.Parameters.AddWithValue("@PRECO", pano.PrecoMetroQuadrado);
                panocmd.Parameters.AddWithValue("@AREAARMAZEM", pano.AreaArmazem);
                panocmd.Parameters.AddWithValue("@TIPO", pano.Tipo);
                panocmd.CommandText = "INSERT INTO PANO (REFERENCIA_FABRICA, TIPO, GRAMAGEM, AREA_ARMAZEM, PRECO_POR_M2) " +
"VALUES (@REFERENCIA, @TIPO, @GRAMAGEM, @AREAARMAZEM, @PRECO);";
                try
                {
                    panocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    dataHandler.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            else if (mat.GetType() == typeof(Linha)) {
                Linha linha = (Linha)mat;
                SqlCommand linhacmd = new SqlCommand();
                linhacmd.Connection = dataHandler.Cn;
                linhacmd.Parameters.Clear();
                linhacmd.Parameters.AddWithValue("@REFERENCIA", linha.Referencia);
                linhacmd.Parameters.AddWithValue("@GROSSURA", linha.Grossura);
                linhacmd.Parameters.AddWithValue("@PRECO", linha.Preco100Metros);
                linhacmd.Parameters.AddWithValue("@COMPRIMENTO", linha.ComprimentoStock);
                linhacmd.CommandText = "INSERT INTO LINHA (, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) " +
"VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @COD_POSTAL, @RUA, @N_PORTA);";
                try
                {
                    linhacmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    dataHandler.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            else
            {
                AcessoriosCostura acessorio = (AcessoriosCostura)mat;
                SqlCommand acessoriocmd = new SqlCommand();
                acessoriocmd.Connection = dataHandler.Cn;
                acessoriocmd.Parameters.Clear();
                acessoriocmd.Parameters.AddWithValue("@REFERENCIA", acessorio.Referencia);
                acessoriocmd.Parameters.AddWithValue("@PRECO_UNIDADE", acessorio.PrecoUnidade);
                acessoriocmd.CommandText = "INSERT INTO ACESSORIOS_COSTURA (REFERENCIA_FABRICA, PRECO_UNIDADE) " +
"VALUES (@REFERENCIA, @PRECO_UNIDADE);";
                try
                {
                    acessoriocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    dataHandler.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }

                if (mat.GetType() == typeof(Mola))
                {
                    Mola mola = (Mola)mat;
                    SqlCommand molacmd = new SqlCommand();
                    molacmd.Connection = dataHandler.Cn;
                    molacmd.Parameters.Clear();
                    molacmd.Parameters.AddWithValue("@REFERENCIA", mola.Referencia);
                    molacmd.Parameters.AddWithValue("@DIAMETRO", mola.Diametro);
                    molacmd.CommandText = "INSERT INTO MOLA (REFERENCIA_FABRICA, DIAMETRO) " +
    "VALUES (@REFERENCIA, @DIAMETRO);";
                    try
                    {
                        molacmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        dataHandler.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
                else if (mat.GetType() == typeof(Botao))
                {
                    Botao botao = (Botao)mat;
                    SqlCommand botaocmd = new SqlCommand();
                    botaocmd.Connection = dataHandler.Cn;
                    botaocmd.Parameters.Clear();
                    botaocmd.Parameters.AddWithValue("@REFERENCIA", botao.Referencia);
                    botaocmd.Parameters.AddWithValue("@DIAMETRO", botao.Diametro);
                    botaocmd.CommandText = "INSERT INTO BOTAO (REFERENCIA_FABRICA, DIAMETRO) " +
    "VALUES (@REFERENCIA, @DIAMETRO);";
                    try
                    {
                        botaocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        dataHandler.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
                else if (mat.GetType() == typeof(FitaVelcro))
                {
                    FitaVelcro fita = (FitaVelcro)mat;
                    SqlCommand fitacmd = new SqlCommand();
                    fitacmd.Connection = dataHandler.Cn;
                    fitacmd.Parameters.Clear();
                    fitacmd.Parameters.AddWithValue("@REFERENCIA", fita.Referencia);
                    fitacmd.Parameters.AddWithValue("@LARGURA", fita.Largura);
                    fitacmd.Parameters.AddWithValue("@COMPRIMENTO", fita.Comprimento);
                    fitacmd.CommandText = "INSERT INTO [FITA-VELCRO] (REFERENCIA_FABRICA, COMPRIMENTO, LARGURA) " +
    "VALUES (@REFERENCIA, @COMPRIMENTO, @LARGURA);";
                    try
                    {
                        fitacmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        dataHandler.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }

                }
                else if (mat.GetType() == typeof(Fecho))
                {
                    Fecho fecho = (Fecho)mat;
                    SqlCommand fechocmd = new SqlCommand();
                    fechocmd.Connection = dataHandler.Cn;
                    fechocmd.Parameters.Clear();
                    fechocmd.Parameters.AddWithValue("@REFERENCIA", fecho.Referencia);
                    fechocmd.Parameters.AddWithValue("@TAMANHO_DENTE", fecho.TamanhoDente);
                    fechocmd.Parameters.AddWithValue("@COMPRIMENTO", fecho.Comprimento);
                    fechocmd.CommandText = "INSERT INTO FECHO (REFERENCIA_FABRICA, TAMANHO_DENTE ,COMPRIMENTO) " +
"VALUES (@REFERENCIA, @TAMANHO_DENTE, @COMPRIMENTO);";
                    try
                    {
                        fechocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        dataHandler.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }

                }
                else if (mat.GetType() == typeof(Elastico))
                {
                    Elastico elastico = (Elastico)mat;
                    SqlCommand elasticocmd = new SqlCommand();
                    elasticocmd.Connection = dataHandler.Cn;
                    elasticocmd.Parameters.Clear();
                    elasticocmd.Parameters.AddWithValue("@REFERENCIA", elastico.Referencia);
                    elasticocmd.Parameters.AddWithValue("@LARGURA", elastico.Largura);
                    elasticocmd.Parameters.AddWithValue("@COMPRIMENTO", elastico.Comprimento);
                    elasticocmd.CommandText = "INSERT INTO ELASTICO (REFERENCIA_FABRICA, COMPRIMENTO, LARGURA) " +
    "VALUES (@REFERENCIA, @COMPRIMENTO, @LARGURA);";
                    try
                    {
                        elasticocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        dataHandler.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
                else
                {

                }
            }
        }



        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            ComboBoxItem cbo = (ComboBoxItem)tipoMaterial.SelectedItem;

            hideAll();
            if (cbo.Name.Equals("Acessorios", StringComparison.Ordinal))
            {
                acessorios.Visibility = Visibility.Visible;
                acessoriosLabel.Visibility = Visibility.Visible;
                fecho.Visibility = Visibility.Visible;
            }
            else
            {
                acessorios.Visibility = Visibility.Hidden;
                acessoriosLabel.Visibility = Visibility.Hidden;
                if (cbo.Name.Equals("Pano", StringComparison.Ordinal))
                {
                    pano.Visibility = Visibility.Visible;
                }
                else if (cbo.Name.Equals("Linha", StringComparison.Ordinal)) {
                    linha.Visibility = Visibility.Visible;
                }
            }
        }
    


        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            MaterialTextil material=null;

            if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Pano"))
            {
                material = new Pano();
                material.Cor = corMaterial.SelectedColor.ToString();
                material.Designacao = "designacao";
                material.Fornecedor = txtFornecedorNif.Text;
                material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                ((Pano)material).Gramagem = (int)txtGramagem.Value;
                ((Pano)material).PrecoMetroQuadrado = (Double)txtPreçoM2.Value;
                ((Pano)material).Tipo = txtTipoPano.Text;
            }
            else if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Linha"))
            {
                material = new Linha();
                ((Linha)material).Preco100Metros = Convert.ToDouble(txtPreço100M.Text);
                ((Linha)material).Grossura = Convert.ToDouble(txtGrossura.Text);
            }
            else if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Acessorios"))
            {
                if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("fecho"))
                {
                    material = new Fecho();
                    ((Fecho)material).TamanhoDente = Convert.ToDouble(txtTamanhoDente.Text);
                    ((Fecho)material).Largura = Convert.ToDouble(txtLarguraFecho.Text);
                    ((Fecho)material).Comprimento = Convert.ToDouble(txtComprimentoFecho.Text);
                    ((Fecho)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeFecho.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("mola"))
                {
                    material = new Mola();
                    ((Mola)material).Diametro = Convert.ToDouble(txtDiametroMola.Text);
                    ((Mola)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeMola.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("botao"))
                {
                    material = new Botao();
                    ((Botao)material).Diametro = Convert.ToDouble(txtDiametroBotao.Text);
                    ((Botao)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeBotao.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("fitaVelcro"))
                {
                    material = new FitaVelcro();
                    ((FitaVelcro)material).Largura = Convert.ToDouble(txtLarguraFita.Text);
                    ((FitaVelcro)material).Comprimento = Convert.ToDouble(txtComprimentoFita.Text);
                    ((FitaVelcro)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeFita.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("elastico"))
                {
                    material = new FitaVelcro();
                    ((Elastico)material).Largura = Convert.ToDouble(txtLarguraFita.Text);
                    ((Elastico)material).Comprimento = Convert.ToDouble(txtComprimentoFita.Text);
                    ((Elastico)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeElastico.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Outro"))
                {
                    material = new AcessoriosCostura();
                }
            }

            try
            {
                EnviarMaterial(material);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show("Material adicionado com sucesso!");
            this.NavigationService.GoBack();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void acessorios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            hideAll();

            ComboBoxItem cbo = (ComboBoxItem)acessorios.SelectedItem;

            if (cbo.Name.Equals("Fecho", StringComparison.Ordinal))
            {
                fecho.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Elastico", StringComparison.Ordinal))
            {
                elastico.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("FitaVelcro", StringComparison.Ordinal))
            {
                fitaVelcro.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Mola", StringComparison.Ordinal))
            {
                mola.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Botao", StringComparison.Ordinal))
            {
                botao.Visibility = Visibility.Visible;
            }
        }

        private void hideAll() {
            pano.Visibility = Visibility.Hidden;
            linha.Visibility = Visibility.Hidden;
            mola.Visibility = Visibility.Hidden;
            fecho.Visibility = Visibility.Hidden;
            botao.Visibility = Visibility.Hidden;
            elastico.Visibility = Visibility.Hidden;
            fitaVelcro.Visibility = Visibility.Hidden;
        }
    }
}

