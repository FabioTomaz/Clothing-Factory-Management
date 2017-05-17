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
        }
        private void EnviarProdutoPersonalizado(ProdutoPersonalizado ProdutoPersonalizado)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            /*
            cmd.CommandText = "INSERT INTO Produto (NOME, IVA, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) "
                + "SELECT @nome_Produto, @iva, @Data_alteracao, @instr, @nGestor, BulkColumn "
                + "FROM Openrowset (Bulk @imagem, Single_Blob) as Image";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome_Produto", ProdutoPersonalizado.Nome);
            cmd.Parameters.AddWithValue("@iva", ProdutoPersonalizado.IVA1);
             cmd.Parameters.AddWithValue("@Data_alteracao", DateTime.Today);
            cmd.Parameters.AddWithValue("@instr", ProdutoPersonalizado.InstrProd);
            cmd.Parameters.AddWithValue("@nGestor", ProdutoPersonalizado.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@imagem", ProdutoPersonalizado.Pic).SqlDbType = SqlDbType.Binary;
            cmd.Connection = dataHandler.Cn;*/
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
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            ProdutoPersonalizado prodPers = new ProdutoPersonalizado();
            try
            {
                prodPers.Tamanho = cbTamanho.SelectedItem.ToString();
                prodPers.Cor = txtCor.SelectedColorText;
                Console.WriteLine(txtCor.SelectedColorText);
                prodPers.Preco = Convert.ToDouble(txtPreço.Text);
                prodPers.ProdutoBase = new ProdutoBase();

                StackPanel stck = (StackPanel)cbProdBase.SelectedItem;
                TextBlock t = (TextBlock)stck.Children[0];
                prodPers.ProdutoBase.Referencia = Convert.ToInt32(t.Text);
                Console.WriteLine(t.Text);
                /* ProdutoPersonalizado.GestorProducao.NFuncionario = 2; //---> suposto mais tarde colocar o nº do user
                ProdutoPersonalizado.Pic = BitMapToByte((BitmapImage)imgPhoto.Source); */
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
                dataHandler.closeSGBDConnection();
            }

        }

        private void etiquetaExistente_Checked(object sender, RoutedEventArgs e)
        {
            etiquetaNova.Visibility = Visibility.Hidden;
            etiquetaExisente.Visibility = Visibility.Visible;
            //fazer bind de todas as etiquetas existentas na base de dados para a combo box
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ObservableCollection<Etiqueta> et = getEtiquetas();
                cbEtiqueta.ItemsSource = et;
                if (et.Count > 0)
                {
                    Etiqueta firstDes = et.First();
                    cbEtiqueta.SelectedItem = firstDes;
                }

            }
            dataHandler.closeSGBDConnection();

        }

        private void etiquetaNova_Checked(object sender, RoutedEventArgs e)
        {
            etiquetaNova.Visibility = Visibility.Visible;
            etiquetaExisente.Visibility = Visibility.Hidden;
        }

        public ObservableCollection<Etiqueta> getEtiquetas()
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
            return etiquetas;
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
        private void addMaterial_Click(object sender, RoutedEventArgs e)
        {
            MaterialTextil mt = (MaterialTextil)materiaisView.SelectedItem;
            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            StackPanel stackMaterial = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Name = "material" + currentRow,
            };

            TextBlock txtref = new TextBlock { Text = mt.Referencia.ToString() };

            TextBlock txtcor = new TextBlock { Text = mt.Cor };

            TextBlock txtDesig = new TextBlock { Text = mt.Designacao };

            StackPanel removerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            removerPanel.Children.Add(new Image { Source = new BitmapImage(new Uri("delete.png", UriKind.Relative)), Width = 20 });
            removerPanel.Children.Add(new TextBlock { Text = "Remover Material", Margin = new Thickness(4, 0, 0, 0) });
            Button remover = new Button
            {
                Tag = currentRow,
                Width = 120,
                Content = removerPanel
            };
            remover.Click += new RoutedEventHandler(Remover_Click);


            stackMaterial.Children.Add(txtref);
            stackMaterial.Children.Add(txtcor);
            stackMaterial.Children.Add(txtDesig);
            stackMaterial.Children.Add(remover);

            stack.Children.Add(stackMaterial);

            currentRow++;
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            Button senderbtn = (Button)sender;
            int row = Convert.ToInt32(senderbtn.Tag);

            for (int i = row + 1; i < currentRow; i++)
            {
                Button produtoBtn = (Button)((StackPanel)stack.Children[i]).Children[0];
                produtoBtn.Tag = i - 1;
            }
            stack.Children.RemoveAt(row);
            currentRow--;
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM PANO WHERE REFERENCIA_FABRICA = @ref"
                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Pano p = new Pano();
            while (reader.Read())
            {
                pano.Referencia = referencia;
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



    }
}