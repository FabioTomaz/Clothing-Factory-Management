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
        public DetalhesMaterial(MaterialTextil material, DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.mat = material;
        }

        private MaterialTextil getDetalhesMaterial(MaterialTextil material)
        {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT * "
                                + " FROM [MATERIAIS_TÊXTEIS] JOIN FORNECEDOR ON FORNECEDOR.NIF=[MATERIAIS_TÊXTEIS].NIF_FORNECEDOR"
                                + " WHERE REFERENCIA_FABRICA=@REFMATERIAL;"
                                , dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFMATERIAL", material.Referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Console.WriteLine("getinfo");
            reader.Read();
            material.Fornecedor = new Fornecedor();
            material.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
            material.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
            material.Cor = reader["COR"].ToString();
            material.Designacao = reader["DESIGNACAO"].ToString();
            material.Fornecedor.Nome = reader["NOME"].ToString();
            material.Fornecedor.NIF_Fornecedor= reader["NIF"].ToString();
            dataHandler.closeSGBDConnection();
            return material;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            getDetalhesMaterial(mat);
            fillInfo();
        }

        private void fillInfo() {
            Console.WriteLine("fillinfo");
            designacao.Content = mat.Designacao;
            cor.Content = mat.Cor;
            referencia.Content = mat.Referencia;
            referenciaFornecedor.Content = mat.ReferenciaFornecedor;
            fornecedor.Content = mat.Fornecedor.Nome;

        }
    }
}
