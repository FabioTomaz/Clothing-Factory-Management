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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesMaterial.xaml
    /// </summary>
    /// 

    public partial class DetalhesMaterial : Window
    {
        DataHandler dataHandler;
        MaterialTextil mat;
        public DetalhesMaterial(DataHandler dataHandler, MaterialTextil material)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.mat = material;
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fillInfo();
            produtos.ItemsSource = dataHandler.getProdutosContainingMaterial(mat.Referencia);
        }

        private void fillInfo()
        {
            design.Text = mat.Designacao;
            cor.Text = mat.Cor;
            mat.TipoMaterial1 = dataHandler.getMaterialType(mat.Referencia);
            if (mat.TipoMaterial1.Equals("Pano", StringComparison.Ordinal))
            {
                Pano pano = dataHandler.getPano(mat.Referencia);
                txt1.Text = pano.Tipo;
                txt2.Text = pano.Gramagem.ToString() + " g/m^2";
                txt3.Text = pano.AreaArmazem.ToString() + "m^2";
                txt4.Text = pano.PrecoMetroQuadrado.ToString() + " €/m^2";
                panoArea.Visibility = Visibility.Visible;
                panoGramagem.Visibility = Visibility.Visible;
                panoPreço.Visibility = Visibility.Visible;
                panoTipo.Visibility = Visibility.Visible;
            }
            else if (mat.TipoMaterial1.Equals("Linha", StringComparison.Ordinal))
            {
                Linha linha = dataHandler.getLinha(mat.Referencia);
                txt1.Text = linha.Grossura.ToString() + " cm";
                txt2.Text = linha.Preco100Metros.ToString() + " €/100m";
                txt3.Text = linha.ComprimentoStock.ToString() + " cm";
                txt4.Text = "";
                linhaGrossura.Visibility = Visibility.Visible;
                linhaPreço.Visibility = Visibility.Visible;
                linhaQuantidade.Visibility = Visibility.Visible;
            }
            else if (mat.TipoMaterial1.Equals("Fecho", StringComparison.Ordinal))
            {
                Fecho fecho = dataHandler.getFecho(mat.Referencia);
                txt1.Text = fecho.Comprimento.ToString() + " cm";
                txt2.Text = fecho.TamanhoDente.ToString() + " cm";
                txt3.Text = fecho.QuantidadeArmazem.ToString() + " un.";
                txt4.Text = fecho.PrecoUnidade.ToString() + " €/un.";
                fechoComprimento.Visibility = Visibility.Visible;
                fechoTamanhoDente.Visibility = Visibility.Visible;
                fechoPreço.Visibility = Visibility.Visible;
                fechoQuantidade.Visibility = Visibility.Visible;

            }
            else if (mat.TipoMaterial1.Equals("Mola", StringComparison.Ordinal))
            {
                Mola mola = dataHandler.getMola(mat.Referencia);
                txt1.Text = mola.Diametro.ToString() + " cm";
                txt2.Text = mola.QuantidadeArmazem.ToString() + " un.";
                txt3.Text = mola.PrecoUnidade.ToString() + " €/un.";
                txt4.Text = "";
                molaDiametro.Visibility = Visibility.Visible;
                molaPreço.Visibility = Visibility.Visible;
                molaQuantidade.Visibility = Visibility.Visible;
            }
            else if (mat.TipoMaterial1.Equals("Botão", StringComparison.Ordinal))
            {
                Botao botao = dataHandler.getBotao(mat.Referencia);
                txt1.Text = botao.Diametro.ToString() + " cm";
                txt2.Text = botao.QuantidadeArmazem.ToString() + " un.";
                txt3.Text = botao.PrecoUnidade.ToString() + " €/un.";
                txt4.Text = "";
                botaoDiametro.Visibility = Visibility.Visible;
                botaoPreço.Visibility = Visibility.Visible;
                botaoQuantidade.Visibility = Visibility.Visible;
            }
            else if (mat.TipoMaterial1.Equals("Elástico", StringComparison.Ordinal))
            {
                Elastico el = dataHandler.getElastico(mat.Referencia);
                txt1.Text = el.Comprimento.ToString() + " cm";
                txt2.Text = el.Largura.ToString() + " cm";
                txt3.Text = el.QuantidadeArmazem.ToString() + " un.";
                txt4.Text = el.PrecoUnidade.ToString() + " €/un.";
                elasticoComprimento.Visibility = Visibility.Visible;
                elasticoLargura.Visibility = Visibility.Visible;
                elasticoPreço.Visibility = Visibility.Visible;
                elasticoQuantidade.Visibility = Visibility.Visible;
            }
            else if (mat.TipoMaterial1.Equals("Fita de Velcro", StringComparison.Ordinal))
            {
                FitaVelcro f = dataHandler.getFitaVelcro(mat.Referencia);
                txt1.Text = f.Comprimento.ToString() + " cm";
                txt2.Text = f.Largura.ToString() + " cm";
                txt3.Text = f.QuantidadeArmazem.ToString() + " un.";
                txt4.Text = f.PrecoUnidade.ToString() + " €/un.";
                velcroComprimento.Visibility = Visibility.Visible;
                velcroLargura.Visibility = Visibility.Visible;
                velcroPreço.Visibility = Visibility.Visible;
                velcroQuantidade.Visibility = Visibility.Visible;
            }

            refFabr.Text = mat.Referencia.ToString();
            refForn.Text = mat.ReferenciaFornecedor;
            nifForn.Text = mat.Fornecedor.NIF_Fornecedor;
            nomeForn.Text = mat.Fornecedor.Nome;
        }

        private void produtos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (produtos.SelectedItems.Count == 1 && Utilizador.loggedUser.TiposUser.Contains("Gestor de Produção"))
            {
                ProdutoPersonalizado prod = (ProdutoPersonalizado)produtos.SelectedItem;
                DetalhesProdutoPersonalizado window = new DetalhesProdutoPersonalizado(dataHandler, (int)prod.ProdutoBase.Referencia, prod.Tamanho, (int)prod.ID);
                window.Show();
            }

        }
    }
}
