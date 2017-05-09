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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for ListarMateriais.xaml
    /// </summary>
    public partial class ListarMateriais : Page
    {
        private DataHandler dataHandler;
        public ListarMateriais(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            removerMaterial.IsEnabled = false;
            editarMaterial.IsEnabled = false;
            detalhesMaterial.IsEnabled = false;
            materiais.Focus();
            if (!dataHandler.verifySGBDConnection()) {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            } else {
                ObservableCollection<MaterialTextil> materiaisTexteis = new ObservableCollection<MaterialTextil>();
                SqlCommand cmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS", dataHandler.Cn);
                SqlDataReader reader = cmd.ExecuteReader();     
                while (reader.Read())
                {
                    MaterialTextil Mt = new MaterialTextil();
                    Mt.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                    Mt.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                    Mt.Designacao = reader["DESIGNACAO"].ToString();
                    Mt.Cor = reader["COR"].ToString();
                    Mt.Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                    materiaisTexteis.Add(Mt);
                }
                dataHandler.closeSGBDConnection();

                setMaterialsTypes(materiaisTexteis);
               
                materiais.ItemsSource = materiaisTexteis;
            }
       }

        private void materiais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (materiais.SelectedItems.Count > 0)
            {
                removerMaterial.IsEnabled = true;
                editarMaterial.IsEnabled = true;
                detalhesMaterial.IsEnabled = true;
            }
        }

        private void Encomenda_Click(object sender, RoutedEventArgs e)
        {
            RegistarMaterial page = new RegistarMaterial(dataHandler);
            NavigationService.Navigate(page);
        }

        private void setMaterialsTypes(ObservableCollection<MaterialTextil> materiaisTexteis)
        {
            dataHandler.verifySGBDConnection();

            List<int> panos = new List<int>();
            SqlCommand panoscmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN PANO ON PANO.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader panoReader = panoscmd.ExecuteReader();
            while (panoReader.Read())
            {
                panos.Add(Convert.ToInt32(panoReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> linhas = new List<int>();
            SqlCommand linhascmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN LINHA ON LINHA.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader linhasReader = linhascmd.ExecuteReader();
            while (linhasReader.Read())
            {
                linhas.Add(Convert.ToInt32(linhasReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> acessorios = new List<int>();
            SqlCommand acessorioscmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN ACESSORIO ON ACESSORIO.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader acessoriosReader = acessorioscmd.ExecuteReader();
            while (acessoriosReader.Read())
            {
                acessorios.Add(Convert.ToInt32(acessoriosReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> fechos = new List<int>();
            SqlCommand fechoscmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN FECHO ON FECHO.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader fechosReader = fechoscmd.ExecuteReader();
            while (fechosReader.Read())
            {
                fechos.Add(Convert.ToInt32(fechosReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> botoes = new List<int>();
            SqlCommand botoescmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN BOTAO ON BOTAO.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader botoesReader = botoescmd.ExecuteReader();
            while (botoesReader.Read())
            {
                botoes.Add(Convert.ToInt32(botoesReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> molas = new List<int>();
            SqlCommand molascmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN MOLA ON MOLA.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader molasReader = molascmd.ExecuteReader();
            while (molasReader.Read())
            {
                molas.Add(Convert.ToInt32(molasReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> elasticos = new List<int>();
            SqlCommand elasticoscmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN ELASTICO ON ELASTICO.REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader elasticosReader = elasticoscmd.ExecuteReader();
            while (elasticosReader.Read())
            {
                elasticos.Add(Convert.ToInt32(elasticosReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();
            dataHandler.verifySGBDConnection();

            List<int> fitas = new List<int>();
            SqlCommand fitascmd = new SqlCommand("SELECT * FROM MATERIAIS_TÊXTEIS JOIN [FITA-VELCRO] ON [FITA-VELCRO].REFERENCIA_FABRICA= MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA", dataHandler.Cn);
            SqlDataReader fitasReader = fitascmd.ExecuteReader();
            while (fitasReader.Read())
            {
                fitas.Add(Convert.ToInt32(fitasReader["REFERENCIA_FABRICA"].ToString()));
            }

            dataHandler.closeSGBDConnection();

            for (int i = 0; i < materiaisTexteis.Count; i++)
            {
                if (panos.Contains(materiaisTexteis.ElementAt(i).Referencia))
                    materiaisTexteis.ElementAt(i).TipoMaterial1 = "Pano";
                else if (linhas.Contains(materiaisTexteis.ElementAt(i).Referencia))
                    materiaisTexteis.ElementAt(i).TipoMaterial1 = "Linha";
                else if (acessorios.Contains(materiaisTexteis.ElementAt(i).Referencia))
                {
                    materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio";
                    if (fechos.Contains(materiaisTexteis.ElementAt(i).Referencia))
                        materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio - Fecho";
                    else if (botoes.Contains(materiaisTexteis.ElementAt(i).Referencia))
                        materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio - Botao";
                    else if (molas.Contains(materiaisTexteis.ElementAt(i).Referencia))
                        materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio - Mola";
                    else if (elasticos.Contains(materiaisTexteis.ElementAt(i).Referencia))
                        materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio - Elastico";
                    else if (fitas.Contains(materiaisTexteis.ElementAt(i).Referencia))
                        materiaisTexteis.ElementAt(i).TipoMaterial1 = "Acessorio - Fita de Velcro";
                }
            }
        }

        private void removerMaterial_Click(object sender, RoutedEventArgs e)
        {
            int listViewIndex = materiais.SelectedIndex;

            if (MessageBox.Show("Tem a certeza que pretende eliminar este Material?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                try
                {
                    RemoverMaterial((MaterialTextil)materiais.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    ((ObservableCollection<MaterialTextil>)materiais.ItemsSource).RemoveAt(listViewIndex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void RemoverMaterial(MaterialTextil material) {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE MATERIAIS_TÊXTEIS WHERE REFERENCIA_FABRICA=@REFERENCIA ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFERENCIA", material.Referencia);
            cmd.Connection = dataHandler.Cn;

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Material Apagado com sucesso!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }
    }

        
}

