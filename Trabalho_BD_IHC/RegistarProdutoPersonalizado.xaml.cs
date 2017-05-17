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
    public partial class RegistarProdutoPersonalizado : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;
        private ObservableCollection<MaterialTextil> materiaisSelecionados;
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

        public RegistarProdutoPersonalizado(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            materiaisSelecionados = new ObservableCollection<MaterialTextil>();
        }

        private bool EnviarProdutoPersonalizado(ProdutoPersonalizado prod)
        {

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
                    throw new Exception("Falha ao registar o Produto Personalizado na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }

            return true;
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
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

            ProdutoPersonalizado prodPers = new ProdutoPersonalizado();
            try
            {
                ComboBoxItem cbi = (ComboBoxItem)cbTamanho.SelectedItem;
                prodPers.Tamanho = cbi.Content.ToString();

                prodPers.Cor = txtCor.SelectedColorText;
                Console.WriteLine(cbi.Content.ToString());
                prodPers.Preco = Convert.ToDouble(txtPreço.Text);
                prodPers.ProdutoBase = new ProdutoBase();
                prodPers.ProdutoBase = (ProdutoBase)cbProdBase.SelectedItem;
                prodPers.MateriaisTexteis = new ObservableCollection<MaterialTextil>();
                for (int i = 0; i < materiaisSelectedView.Items.Count; i++)
                {
                    prodPers.MateriaisTexteis.Add((MaterialTextil)materiaisSelectedView.Items[0]);

                }
                prodPers.Etiqueta = new Etiqueta();
                if (rdEtiquetaExis.IsChecked == true)
                {
                    prodPers.Etiqueta = (Etiqueta)cbEtiqueta.SelectedItem;

                }
                else if (rdEtiquetaNova.IsChecked == true)
                {
                    //inserir primeiro a nova etiqueta na base de dados
                    dataHandler.insertEtiqueta(txtNormas.Text, txtComp.Text, txtPais.Text);
                    //obter o numero da etiqueta adicionada, pois é necessario para o registo do produto
                    prodPers.Etiqueta.Numero = dataHandler.getEtiqueta(txtNormas.Text, txtComp.Text, txtPais.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                EnviarProdutoPersonalizado(prodPers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Produto Personalizado registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                
                ListarProdutos lp = new ListarProdutos(dataHandler);
                ObservableCollection<MaterialTextil> mt = getMateriais(dataHandler);
                materiaisView.ItemsSource = mt;
                ObservableCollection<ProdutoBase> prodBase = lp.getProdutosBase();
                cbProdBase.ItemsSource = prodBase;
                if (prodBase.Count > 0)
                {
                    ProdutoBase firstProd = prodBase.First();
                    cbProdBase.SelectedItem = firstProd;
                }
                Console.WriteLine("loading..");
                dataHandler.closeSGBDConnection();
            }

        }

        private void etiquetaExistente_Checked(object sender, RoutedEventArgs e)
        {
            etiquetaNova.Visibility = Visibility.Hidden;
            etiquetaExisente.Visibility = Visibility.Visible;
            //fazer bind de todas as etiquetas existentas na base de dados para a combo box
            ObservableCollection<Etiqueta> et = getEtiquetas();
            cbEtiqueta.ItemsSource = et;
            if (et.Count > 0)
            {
                Etiqueta firstDes = et.First();
                cbEtiqueta.SelectedItem = firstDes;
            }
        }

        private void etiquetaNova_Checked(object sender, RoutedEventArgs e)
        {
            etiquetaNova.Visibility = Visibility.Visible;
            etiquetaExisente.Visibility = Visibility.Hidden;
        }

        public ObservableCollection<Etiqueta> getEtiquetas()
        {
            if (dataHandler.verifySGBDConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ETIQUETA", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<Etiqueta> etiquetas = new ObservableCollection<Etiqueta>();
                while (reader.Read())
                {
                    Etiqueta et = new Etiqueta();
                    et.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                    et.Normas = reader["NORMAS"].ToString();
                    et.Composicao = reader["COMPOSICAO"].ToString();
                    et.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                    etiquetas.Add(et);
                }
                reader.Close();
                dataHandler.closeSGBDConnection();
                return etiquetas;
            }
            return null;
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
            Console.Write("fds");
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
                        Pano pano = getPano(mt.Referencia);
                        txt1.Text = pano.Tipo;
                        txt2.Text = pano.Gramagem.ToString();
                        txt3.Text = pano.AreaArmazem.ToString();
                        txt4.Text = pano.PrecoMetroQuadrado.ToString();
                        panoArea.Visibility = Visibility.Visible;
                        panoGramagem.Visibility = Visibility.Visible;
                        panoPreço.Visibility = Visibility.Visible;
                        panoTipo.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Linha", StringComparison.Ordinal))
                    {
                        Linha linha = getLinha(mt.Referencia);
                        txt1.Text = linha.Grossura.ToString();
                        txt2.Text = linha.Preco100Metros.ToString();
                        txt3.Text = linha.ComprimentoStock.ToString();
                        txt4.Text = "";
                        linhaGrossura.Visibility = Visibility.Visible;
                        linhaPreço.Visibility = Visibility.Visible;
                        linhaQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Fecho", StringComparison.Ordinal))
                    {
                        Fecho fecho = getFecho(mt.Referencia);
                        txt1.Text = fecho.Comprimento.ToString();
                        txt2.Text = fecho.TamanhoDente.ToString();
                        txt3.Text = fecho.QuantidadeArmazem.ToString();
                        txt4.Text = fecho.PrecoUnidade.ToString();
                        fechoComprimento.Visibility = Visibility.Visible;
                        fechoTamanhoDente.Visibility = Visibility.Visible;
                        fechoPreço.Visibility = Visibility.Visible;
                        fechoQuantidade.Visibility = Visibility.Visible;

                    }
                    else if (tipoMaterial.Equals("Mola", StringComparison.Ordinal))
                    {
                        Mola mola = getMola(mt.Referencia);
                        txt1.Text = mola.Diametro.ToString();
                        txt2.Text = mola.QuantidadeArmazem.ToString();
                        txt3.Text = mola.PrecoUnidade.ToString();
                        txt4.Text = "";
                        molaDiametro.Visibility = Visibility.Visible;
                        molaPreço.Visibility = Visibility.Visible;
                        molaQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Botão", StringComparison.Ordinal))
                    {
                        Botao botao = getBotao(mt.Referencia);
                        txt1.Text = botao.Diametro.ToString();
                        txt2.Text = botao.QuantidadeArmazem.ToString();
                        txt3.Text = botao.PrecoUnidade.ToString();
                        txt4.Text = "";
                        botaoDiametro.Visibility = Visibility.Visible;
                        botaoPreço.Visibility = Visibility.Visible;
                        botaoQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Elástico", StringComparison.Ordinal))
                    {
                        Elastico el = getElastico(mt.Referencia);
                        txt1.Text = el.Comprimento.ToString();
                        txt2.Text = el.Largura.ToString();
                        txt3.Text = el.QuantidadeArmazem.ToString();
                        txt4.Text = el.PrecoUnidade.ToString();
                        elasticoComprimento.Visibility = Visibility.Visible;
                        elasticoLargura.Visibility = Visibility.Visible;
                        elasticoPreço.Visibility = Visibility.Visible;
                        elasticoQuantidade.Visibility = Visibility.Visible;
                    }
                    else if (tipoMaterial.Equals("Fita de Velcro", StringComparison.Ordinal))
                    {
                        FitaVelcro f = getFitaVelcro(mt.Referencia);
                        txt1.Text = f.Comprimento.ToString();
                        txt2.Text = f.Largura.ToString();
                        txt3.Text = f.QuantidadeArmazem.ToString();
                        txt4.Text = f.PrecoUnidade.ToString();
                        velcroComprimento.Visibility = Visibility.Visible;
                        velcroLargura.Visibility = Visibility.Visible;
                        velcroPreço.Visibility = Visibility.Visible;
                        velcroQuantidade.Visibility = Visibility.Visible;
                    }
                }
                dataHandler.closeSGBDConnection();
            }
        }

        public Pano getPano(int referencia)
        {
            Pano pano = new Pano();
            SqlCommand cmd = new SqlCommand("SELECT PANO.REFERENCIA_FABRICA AS refFabr, TIPO, GRAMAGEM, "
                + "AREA_ARMAZEM, PRECO_POR_M2, REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM PANO "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = PANO.REFERENCIA_FABRICA "
                + "WHERE PANO.REFERENCIA_FABRICA = @ref"
                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Pano p = new Pano();
            while (reader.Read())
            {
                pano.Referencia = referencia;
                pano.Cor = reader["COR"].ToString();
                pano.Fornecedor = new Fornecedor();
                pano.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                pano.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                pano.Designacao = reader["DESIGNACAO"].ToString();
                pano.Gramagem = Convert.ToInt32(reader["GRAMAGEM"].ToString());
                pano.Tipo = reader["TIPO"].ToString();
                pano.AreaArmazem = Convert.ToDouble(reader["AREA_ARMAZEM"].ToString());
                pano.PrecoMetroQuadrado = Convert.ToDouble(reader["PRECO_POR_M2"].ToString());

            }
            reader.Close();
            return pano;
        }


        public Linha getLinha(int referencia)
        {

            SqlCommand cmd = new SqlCommand("SELECT * FROM LINHA WHERE REFERENCIA_FABRICA = @ref", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Linha l = new Linha();
            while (reader.Read())
            {
                l.Referencia = referencia;
                l.Grossura = Convert.ToDouble(reader["GROSSURA"].ToString());
                l.ComprimentoStock = Convert.ToDouble(reader["COMPRIMENTO_ARMAZEM"].ToString());
                l.Preco100Metros = Convert.ToDouble(reader["PRECO_CEM_METROS"].ToString());

            }
            reader.Close();
            return l;
        }

        public Fecho getFecho(int referencia)
        {

            SqlCommand cmd = new SqlCommand("SELECT FECHO.REFERENCIA_FABRICA, COMPRIMENTO, "
                + "TAMANHO_DENTE, QUANTIDADE_ARMAZEM, PRECO_UNIDADE FROM FECHO JOIN "
                + "ACESSORIO ON FECHO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + " WHERE FECHO.REFERENCIA_FABRICA = @ref", dataHandler.Cn);

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            Fecho f = new Fecho();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                f.Referencia = referencia;
                f.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
                f.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                f.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                f.TamanhoDente = Convert.ToDouble(reader["TAMANHO_DENTE"].ToString());

            }
            reader.Close();
            return f;
        }

        public Mola getMola(int referencia)
        {

            SqlCommand cmd = new SqlCommand("SELECT MOLA.REFERENCIA_FABRICA, DIAMETRO, QUANTIDADE_ARMAZEM, "
                + "PRECO_UNIDADE  FROM MOLA JOIN ACESSORIO ON "
                    + "MOLA.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                    + "WHERE MOLA.REFERENCIA_FABRICA = @ref", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Mola m = new Mola();
            while (reader.Read())
            {
                m.Referencia = referencia;
                m.Diametro = Convert.ToDouble(reader["DIAMETRO"].ToString());
                m.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                m.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
            }
            reader.Close();
            return m;
        }

        public Botao getBotao(int referencia)
        {

            SqlCommand cmd = new SqlCommand("SELECT BOTAO.REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, "
                    + "PRECO_UNIDADE, DIAMETRO FROM BOTAO JOIN ACESSORIO ON "
                    + "BOTAO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                    + "WHERE BOTAO.REFERENCIA_FABRICA = @ref", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Botao b = new Botao();
            while (reader.Read())
            {
                b.Referencia = referencia;
                b.Diametro = Convert.ToDouble(reader["DIAMETRO"].ToString());
                b.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                b.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());

            }
            reader.Close();
            return b;
        }


        public Elastico getElastico(int referencia)
        {

            SqlCommand cmd = new SqlCommand("SELECT ELASTICO.REFERENCIA_FABRICA as ref, QUANTIDADE_ARMAZEM, "
                + "COR, PRECO_UNIDADE, LARGURA, REFERENCIA_FORN, COR, DESIGNACAO, COMPRIMENTO, NIF_FORNECEDOR FROM ELASTICO "
                + "JOIN ACESSORIO ON ELASTICO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "WHERE ELASTICO.REFERENCIA_FABRICA = @ref", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Elastico el = new Elastico();
            while (reader.Read())
            {
                el.Referencia = referencia;
                el.Cor = reader["COR"].ToString();
                el.Designacao = reader["DESIGNACAO"].ToString();
                el.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                el.Fornecedor = new Fornecedor();
                el.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                el.Largura = Convert.ToDouble(reader["LARGURA"].ToString());
                el.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                el.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                el.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
            }
            reader.Close();
            return el;
        }

        public FitaVelcro getFitaVelcro(int referencia)
        {
            SqlCommand cmd = new SqlCommand("SELECT [FITA-VELCRO].REFERENCIA_FABRICA, "
                    + "QUANTIDADE_ARMAZEM, PRECO_UNIDADE, LARGURA, COMPRIMENTO FROM [FITA-VELCRO] "
                    + "JOIN ACESSORIO ON[FITA-VELCRO].REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                    + "WHERE [FITA-VELCRO].REFERENCIA_FABRICA = @ref", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            FitaVelcro f = new FitaVelcro();
            while (reader.Read())
            {
                f.Referencia = referencia;
                f.Largura = Convert.ToDouble(reader["LARGURA"].ToString());
                f.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                f.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                f.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());

            }
            reader.Close();
            return f;
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
                    mt.QuantidadeSelecionada = txtQuantidadeDec.Text.ToString();
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
                    mt.QuantidadeSelecionada = txtQuantidadeInt.Text.ToString();
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
            if (materiaisSelectedView.SelectedItems.Count > 0)
                removeMaterial.IsEnabled = true;

        }


    }
}