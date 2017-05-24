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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ProduzirProduto.xaml
    /// </summary>
    public partial class ProduzirProduto : Page
    {
        private DataHandler dataHandler;
        private ProdutoPersonalizado prodPers;
        private ObservableCollection<MaterialTextil> materiaisProd;
        public ProduzirProduto(DataHandler dataHandler, ProdutoPersonalizado prodPers)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.prodPers = prodPers;

            nomeProduto.Text = prodPers.ProdutoBase.Nome.ToString();
            //obter os materiais necessários para o produto
            ObservableCollection<MaterialTextil> mtProd = materiaisProduto(prodPers.ProdutoBase.Referencia, prodPers.Tamanho, prodPers.Cor, prodPers.ID);
            ObservableCollection<MaterialTextil> materiaisProd = new ObservableCollection<MaterialTextil>();
            String s = "";
            foreach (MaterialTextil mt in mtProd)
            {   //passar as funçoes pra sql
                s = dataHandler.getMaterialType(mt.Referencia);
                if (s.Equals("Pano", StringComparison.Ordinal))
                {
                    Pano p = dataHandler.getPano(mt.Referencia);
                    //p.Add(materiaisProd);
                }
            }
            DGproduçao.ItemsSource = mtProd;

        }


        //devolve todos os materiais necessários para a produção do produto selecionado
        private ObservableCollection<MaterialTextil> materiaisProduto(int referencia, String tamanho, String cor, int id)
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
                m.QuantidadeSelecionada = Convert.ToDouble(reader["QUANTIDADE"].ToString());
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

        }

        private void DGproduçao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}