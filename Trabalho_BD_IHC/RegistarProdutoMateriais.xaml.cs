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
    /// Interaction logic for RegistarProdutoMateriais.xaml
    /// </summary>
    public partial class RegistarProdutoMateriais : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;
        private ObservableCollection<MaterialTextil> materiaisSelecionados;
        private ProdutoPersonalizado prod;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ListarProdutos lp = new ListarProdutos(dataHandler);
                ObservableCollection<MaterialTextil> mt = getMateriais(dataHandler);
                materiaisView.ItemsSource = mt;
                dataHandler.closeSGBDConnection();
            }

        }

        public RegistarProdutoMateriais(DataHandler dataHandler, ProdutoPersonalizado prod)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.prod = prod;
            materiaisSelecionados = new ObservableCollection<MaterialTextil>();
        }

        private bool EnviarProduto(ProdutoPersonalizado prod)
        {
            //registar etiqueta na base de dados primerio
            //inserir primeiro a nova etiqueta na base de dados
            dataHandler.insertEtiqueta(prod.Etiqueta.Normas, prod.Etiqueta.Composicao, prod.Etiqueta.PaisFabrico);
            //obter o numero da etiqueta adicionada, pois é necessario para o registo do produto
            prod.Etiqueta.Numero = dataHandler.getEtiqueta(prod.Etiqueta.Normas, prod.Etiqueta.Composicao, prod.Etiqueta.PaisFabrico);

            //produto
            if (!dataHandler.verifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO [PRODUTO-PERSONALIZADO] (REFERENCIA, TAMANHO, COR, PRECO, N_ETIQUETA) "
                + "VALUES (@refProdBase, @tamanho, @cor, @preco, @nEt);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@refProdBase", prod.ProdutoBase.Referencia);
            cmd.Parameters.AddWithValue("@tamanho", prod.Tamanho);
            cmd.Parameters.AddWithValue("@cor", prod.Cor);
            cmd.Parameters.AddWithValue("@preco", prod.Preco);
            cmd.Parameters.AddWithValue("@nEt", prod.Etiqueta.Numero);
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o Produto Personalizado na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
            //materiais do produto
            int ID = dataHandler.getLastIdentity("[PRODUTO-PERSONALIZADO]");

            for (int i = 0; i < materiaisSelecionados.Count; i++)
            {
                if (!dataHandler.verifySGBDConnection())
                    return false;
                Console.WriteLine("HEY");
                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [MATERIAIS-PRODUTO] (REFERENCIA, TAMANHO, COR, ID, REFERENCIA_FABRICA, QUANTIDADE) "
                    + "VALUES (@refProdBase, @tamanho, @cor, @id, @refFabrica, @qtd);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@refProdBase", prod.ProdutoBase.Referencia);
                cmd.Parameters.AddWithValue("@tamanho", prod.Tamanho);
                cmd.Parameters.AddWithValue("@cor", prod.Cor);
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@refFabrica", materiaisSelecionados.ElementAt(i).Referencia);
                cmd.Parameters.AddWithValue("@qtd", materiaisSelecionados.ElementAt(i).QuantidadeSelecionada);
                cmd.Connection = dataHandler.Cn;
                cmd.Connection = dataHandler.Cn;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao registar o Produto na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            return true;
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar o registo do produto? Perderá todos os dados que tenha introduzido",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                ListarProdutos page = new ListarProdutos(dataHandler);
                this.NavigationService.Navigate(page);
            }
        }

        private void voltar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            //o utilizador ja tem q ter selecionado materiais para o produto
            if (materiaisSelectedView.Items.Count <= 0)
            {
                MessageBox.Show("Por favor, selecione os materiais necessários para a " +
                    "posterior produção deste produto", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                prod.MateriaisTexteis = new ObservableCollection<MaterialTextil>();
                for (int i = 0; i < materiaisSelectedView.Items.Count; i++)
                {
                    prod.MateriaisTexteis.Add((MaterialTextil)materiaisSelectedView.Items[i]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                EnviarProduto(prod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Produto registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            ListarProdutos page = new ListarProdutos(dataHandler);
            this.NavigationService.Navigate(page);
        }

        private ObservableCollection<MaterialTextil> getMateriais(DataHandler dataHandler)
        {
            ObservableCollection<MaterialTextil> materiaisTexteis = new ObservableCollection<MaterialTextil>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS", dataHandler.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MaterialTextil Mt = new MaterialTextil();
                Mt.Fornecedor = new Fornecedor();
                Mt.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                Mt.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                Mt.Designacao = reader["DESIGNACAO"].ToString();
                Mt.Cor = reader["COR"].ToString();
                Mt.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                materiaisTexteis.Add(Mt);
            }
            reader.Close();
            return materiaisTexteis;
        }

        private void hideAll()
        {
            panoArea.Visibility = Visibility.Hidden;
            panoGramagem.Visibility = Visibility.Hidden;
            panoPreço.Visibility = Visibility.Hidden;
            panoTipo.Visibility = Visibility.Hidden;
            linhaGrossura.Visibility = Visibility.Hidden;
            linhaPreço.Visibility = Visibility.Hidden;
            linhaQuantidade.Visibility = Visibility.Hidden;
            molaDiametro.Visibility = Visibility.Hidden;
            molaPreço.Visibility = Visibility.Hidden;
            molaQuantidade.Visibility = Visibility.Hidden;
            fechoComprimento.Visibility = Visibility.Hidden;
            fechoTamanhoDente.Visibility = Visibility.Hidden;
            fechoPreço.Visibility = Visibility.Hidden;
            fechoQuantidade.Visibility = Visibility.Hidden;
            botaoDiametro.Visibility = Visibility.Hidden;
            botaoPreço.Visibility = Visibility.Hidden;
            botaoQuantidade.Visibility = Visibility.Hidden;
            elasticoComprimento.Visibility = Visibility.Hidden;
            elasticoLargura.Visibility = Visibility.Hidden;
            elasticoPreço.Visibility = Visibility.Hidden;
            elasticoQuantidade.Visibility = Visibility.Hidden;
            velcroComprimento.Visibility = Visibility.Hidden;
            velcroLargura.Visibility = Visibility.Hidden;
            velcroPreço.Visibility = Visibility.Hidden;
            velcroQuantidade.Visibility = Visibility.Hidden;
        }

        private void materiaisView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            /*ver o tipo de material do item selecionado, meter na grid de detalhes a informçao
             * desse material, tornando a visibilidade ativa dos textblocks especificos*/
            String tipoMaterial = "";
            if (materiaisView.SelectedItems.Count > 0)
            {
                hideAll();
                addMaterial.IsEnabled = true;
                MaterialTextil mt = new MaterialTextil();
                mt = (MaterialTextil)materiaisView.SelectedItem;

                tipoMaterial = dataHandler.getMaterialType(mt.Referencia);
                //consoante o material selecionado, tornar visivel ou a caixa de seleçao de
                //quantidade de material inteira ou a caixa de seleçao de quantidade de material decimal
                if (tipoMaterial.Equals("Pano", StringComparison.Ordinal) || tipoMaterial.Equals("Linha", StringComparison.Ordinal))
                {
                    txtQuantidadeDec.Visibility = Visibility.Visible;
                    txtQuantidadeInt.Visibility = Visibility.Hidden;
                }
                else
                {
                    txtQuantidadeInt.Visibility = Visibility.Visible;
                    txtQuantidadeDec.Visibility = Visibility.Hidden;
                }

                if (dataHandler.verifySGBDConnection())
                {
                    if (tipoMaterial.Equals("Pano", StringComparison.Ordinal))
                    {
                        Pano pano = dataHandler.getPano(mt.Referencia);
                        txt1.Text = pano.Tipo;
                        txt2.Text = pano.Gramagem.ToString() + " g/m^2";
                        txt3.Text = pano.AreaArmazem.ToString() + "m^2";
                        txt4.Text = pano.PrecoMetroQuadrado.ToString() + " €/m^2";
                        panoArea.Visibility = Visibility.Visible;
                        panoGramagem.Visibility = Visibility.Visible;
                        panoPreço.Visibility = Visibility.Visible;
                        panoTipo.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Linha", StringComparison.Ordinal))
                    {
                        Linha linha = dataHandler.getLinha(mt.Referencia);
                        txt1.Text = linha.Grossura.ToString() + " cm";
                        txt2.Text = linha.Preco100Metros.ToString() + " €/100m";
                        txt3.Text = linha.ComprimentoStock.ToString() + " cm";
                        txt4.Text = "";
                        linhaGrossura.Visibility = Visibility.Visible;
                        linhaPreço.Visibility = Visibility.Visible;
                        linhaQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Fecho", StringComparison.Ordinal))
                    {
                        Fecho fecho = dataHandler.getFecho(mt.Referencia);
                        txt1.Text = fecho.Comprimento.ToString() + " cm";
                        txt2.Text = fecho.TamanhoDente.ToString() + " cm";
                        txt3.Text = fecho.QuantidadeArmazem.ToString() + " un.";
                        txt4.Text = fecho.PrecoUnidade.ToString() + " €/un.";
                        fechoComprimento.Visibility = Visibility.Visible;
                        fechoTamanhoDente.Visibility = Visibility.Visible;
                        fechoPreço.Visibility = Visibility.Visible;
                        fechoQuantidade.Visibility = Visibility.Visible;

                    }
                    else if (tipoMaterial.Equals("Mola", StringComparison.Ordinal))
                    {
                        Mola mola = dataHandler.getMola(mt.Referencia);
                        txt1.Text = mola.Diametro.ToString() + " cm";
                        txt2.Text = mola.QuantidadeArmazem.ToString() + " un.";
                        txt3.Text = mola.PrecoUnidade.ToString() + " €/un.";
                        txt4.Text = "";
                        molaDiametro.Visibility = Visibility.Visible;
                        molaPreço.Visibility = Visibility.Visible;
                        molaQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Botão", StringComparison.Ordinal))
                    {
                        Botao botao = dataHandler.getBotao(mt.Referencia);
                        txt1.Text = botao.Diametro.ToString() + " cm";
                        txt2.Text = botao.QuantidadeArmazem.ToString() + " un.";
                        txt3.Text = botao.PrecoUnidade.ToString() + " €/un.";
                        txt4.Text = "";
                        botaoDiametro.Visibility = Visibility.Visible;
                        botaoPreço.Visibility = Visibility.Visible;
                        botaoQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Elástico", StringComparison.Ordinal))
                    {
                        Elastico el = dataHandler.getElastico(mt.Referencia);
                        txt1.Text = el.Comprimento.ToString() + " cm";
                        txt2.Text = el.Largura.ToString() + " cm";
                        txt3.Text = el.QuantidadeArmazem.ToString() + " un.";
                        txt4.Text = el.PrecoUnidade.ToString() + " €/un.";
                        elasticoComprimento.Visibility = Visibility.Visible;
                        elasticoLargura.Visibility = Visibility.Visible;
                        elasticoPreço.Visibility = Visibility.Visible;
                        elasticoQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Fita de Velcro", StringComparison.Ordinal))
                    {
                        FitaVelcro f = dataHandler.getFitaVelcro(mt.Referencia);
                        txt1.Text = f.Comprimento.ToString() + " cm";
                        txt2.Text = f.Largura.ToString() + " cm";
                        txt3.Text = f.QuantidadeArmazem.ToString() + " un.";
                        txt4.Text = f.PrecoUnidade.ToString() + " €/un.";
                        velcroComprimento.Visibility = Visibility.Visible;
                        velcroLargura.Visibility = Visibility.Visible;
                        velcroPreço.Visibility = Visibility.Visible;
                        velcroQuantidade.Visibility = Visibility.Visible;
                    }
                }
                dataHandler.closeSGBDConnection();
            }
        }



        private void addMaterial_Click(object sender, RoutedEventArgs e)
        {

            MaterialTextil mt = (MaterialTextil)materiaisView.SelectedItem;
            if (txtQuantidadeDec.Visibility == Visibility.Visible)
            {
                if (txtQuantidadeDec.Text == null)
                {
                    MessageBox.Show("Por favor, selecione primeiro a quantidade do material selecionado" +
                        "necessária para produzir este produto", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    mt.QuantidadeSelecionada = Convert.ToDouble(txtQuantidadeDec.Text.ToString());
                }

            }
            else if (txtQuantidadeInt.Visibility == Visibility.Visible)
            {
                if (txtQuantidadeInt.Text == null)
                {
                    MessageBox.Show("Por favor, selecione primeiro a quantidade do material selecionado " +
                        "necessária para produzir este produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    mt.QuantidadeSelecionada = Convert.ToDouble(txtQuantidadeInt.Text.ToString());
                }
            }
            materiaisSelecionados.Add(mt);
            materiaisSelectedView.ItemsSource = materiaisSelecionados;
        }

        public void removeMaterial_Click(object sender, RoutedEventArgs e)
        {
            MaterialTextil mt = (MaterialTextil)materiaisSelectedView.SelectedItem;
            materiaisSelecionados.Remove(mt);
        }

        private void materiaisSelectedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (materiaisSelectedView.SelectedItems.Count > 0)
                removeMaterial.IsEnabled = true;

        }



    }
}