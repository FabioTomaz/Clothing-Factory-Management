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
using System.Windows.Controls.Primitives;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ProduzirProduto.xaml
    /// </summary>
    public partial class ProduzirProduto : Page
    {
        private DataHandler dataHandler;
        private ProdutoPersonalizado prodPers;
        private ObservableCollection<MaterialTextil> mtProd;
        private int invalidMaterials;
        public ProduzirProduto(DataHandler dataHandler, ProdutoPersonalizado prodPers)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.prodPers = prodPers;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                nomeProduto.Text = prodPers.ProdutoBase.Nome.ToString();
                //obter os materiais necessários para o produto
                int quantProd = Convert.ToInt32(quantidade.Text);
                mtProd = materiaisProduto((int)prodPers.ProdutoBase.Referencia, prodPers.Tamanho, prodPers.Cor, (int)prodPers.ID, quantProd);
                ObservableCollection<MaterialTextil> materiaisProd = new ObservableCollection<MaterialTextil>();
                String s = "";
                
                foreach (MaterialTextil mt in mtProd)
                {   //passar as funçoes pra sql
                    s = dataHandler.getMaterialType(mt.Referencia);
                    if (s.Equals("Pano", StringComparison.Ordinal))
                    {
                        Pano p = dataHandler.getPano(mt.Referencia);
                        mt.QuantidadeStock = p.AreaArmazem + " m^2";
                        mt.QuantidadeStockD = p.AreaArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        p.QuantidadeNecessaria = Convert.ToDouble(mt.QuantidadeSelecionada);
                        materiaisProd.Add(p);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " m^2";
                    }
                    else if (s.Equals("Linha", StringComparison.Ordinal))
                    {
                        Linha l = dataHandler.getLinha(mt.Referencia);
                        mt.QuantidadeStock = l.ComprimentoStock + " m";
                        mt.QuantidadeStockD = l.ComprimentoStock ;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        l.QuantidadeNecessaria = Convert.ToDouble(mt.QuantidadeSelecionada);
                        materiaisProd.Add(l);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " m";
                    }
                    else if (s.Equals("Fecho", StringComparison.Ordinal))
                    {
                        Fecho f = dataHandler.getFecho(mt.Referencia);
                        mt.QuantidadeStock = f.QuantidadeArmazem + " un.";
                        mt.QuantidadeStockD = f.QuantidadeArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        f.QuantidadeNecessaria = Convert.ToInt32(mt.QuantidadeSelecionada);
                        materiaisProd.Add(f);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " un.";
                    }
                    else if (s.Equals("Mola", StringComparison.Ordinal))
                    {
                        Mola m = dataHandler.getMola(mt.Referencia);
                        mt.QuantidadeStock = m.QuantidadeArmazem + " un.";
                        mt.QuantidadeStockD = m.QuantidadeArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        m.QuantidadeNecessaria = Convert.ToInt32(mt.QuantidadeSelecionada);
                        materiaisProd.Add(m);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " un.";
                    }
                    else if (s.Equals("Botão", StringComparison.Ordinal))
                    {
                        Botao b = dataHandler.getBotao(mt.Referencia);
                        mt.QuantidadeStock = b.QuantidadeArmazem + " un.";
                        mt.QuantidadeStockD = b.QuantidadeArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        b.QuantidadeNecessaria = Convert.ToInt32(mt.QuantidadeSelecionada);
                        materiaisProd.Add(b);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " un.";
                    }
                    else if (s.Equals("Elástico", StringComparison.Ordinal))
                    {
                        Elastico el = dataHandler.getElastico(mt.Referencia);
                        mt.QuantidadeStock = el.QuantidadeArmazem + " un.";
                        mt.QuantidadeStockD = el.QuantidadeArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        el.QuantidadeNecessaria = Convert.ToInt32(mt.QuantidadeSelecionada);
                        materiaisProd.Add(el);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " un.";
                    }
                    else if (s.Equals("Fita de Velcro", StringComparison.Ordinal))
                    {
                        FitaVelcro fv = dataHandler.getFitaVelcro(mt.Referencia);
                        mt.QuantidadeStock = fv.QuantidadeArmazem + " un.";
                        mt.QuantidadeStockD = fv.QuantidadeArmazem;
                        //passar para a observable collection que tem todos os detalhes do material o double da 
                        //quantidade necessária para o produzir
                        fv.QuantidadeNecessaria = Convert.ToInt32(mt.QuantidadeSelecionada);
                        materiaisProd.Add(fv);
                        //acrescentar as unidades na variavel que vai ser usada para o bind
                        mt.QuantidadeSelecionada = mt.QuantidadeSelecionada + " un.";
                    }
                }
                DGproduçao.ItemsSource = mtProd;
            }
        }


       

        //devolve todos os materiais necessários para a produção do produto selecionado
        private ObservableCollection<MaterialTextil> materiaisProduto(int referencia, String tamanho, String cor, int id, int qtProd)
        {
            ObservableCollection<MaterialTextil> mt = new ObservableCollection<MaterialTextil>();
            if (!dataHandler.verifySGBDConnection())
                return mt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.getProductMaterials(@ref, @tamanho, @cor, @id);", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            cmd.Parameters.AddWithValue("@tamanho", tamanho);
            cmd.Parameters.AddWithValue("@cor", cor);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MaterialTextil m = new MaterialTextil();
                m.Designacao = reader["DESIGNACAO"].ToString();
                m.Fornecedor = new Fornecedor();
                m.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                m.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                m.Cor = reader["COR"].ToString();
                m.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                m.QuantidadeSelecionada = (Convert.ToDouble(reader["QUANTIDADE"].ToString()) * qtProd).ToString();
                m.QuantidadeSelecionadaD = Convert.ToDouble(m.QuantidadeSelecionada)* qtProd;
                mt.Add(m);
            }
            reader.Close();
            dataHandler.closeSGBDConnection();
            return mt;
        }
        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(quantidade.Text))
                Xceed.Wpf.Toolkit.MessageBox.Show("Selecione a quantidade de produto que pretende produzir", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                if (dataHandler.produzirProduto(prodPers, Convert.ToInt32(quantidade.Text)))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("O produto foi produzido com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.NavigationService.GoBack();
                }
                else
                    Xceed.Wpf.Toolkit.MessageBox.Show("Não foi possível produzir o produto, pois não tem quantidade de material suficiente para a produção. Encomende o material em falta.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void quantidade_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(IsLoaded)
                Page_Loaded(sender, e);
        }
    }
}