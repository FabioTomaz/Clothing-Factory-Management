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
    /// Interaction logic for RegistarProduto.xaml
    /// </summary>
    public partial class RegistarProduto : Page
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

        public RegistarProduto(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            materiaisSelecionados = new ObservableCollection<MaterialTextil>();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem a certeza que deseja cancelar o registo do produto? Perderá todos os dados que tenha introduzido",
                "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            //verificar se um tamanho foi selecionado
            if (cbTamanho.SelectedIndex <= -1)
            {
                MessageBox.Show("Por favor, selecione o tamanho a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //verificar se uma cor foi selecionada
            if (txtCor.SelectedColorText == "")
            {
                MessageBox.Show("Por favor, selecione uma cor a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPreço.Text == "")
            {
                MessageBox.Show("Por favor, indique o preço a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Convert.ToDouble(txtPreço.Text) <= 0)
            {
                MessageBox.Show("O preço do produto deve ser maior que 0!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (rdEtiquetaExis.IsChecked == false && rdEtiquetaNova.IsChecked == false)
            {
                MessageBox.Show("Por favor, adicione uma etiqueta existente ao produto, ou crie uma nova!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //validar os formularios de preenchimento de uma etiqueta nova
            if (rdEtiquetaNova.IsChecked == true)
            {
                if (txtNormas.Text.Length == 0 || txtComp.Text.Length == 0 || txtPais.Text.Length == 0)
                {
                    MessageBox.Show("Por favor, preencha todos os campos relativos à criação de nova etiqueta!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (txtNormas.Text.Length > 100)
                {
                    MessageBox.Show("A especificação das normas é demasiado longa! Indique apenas o essencial.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (txtComp.Text.Length > 100)
                {
                    MessageBox.Show("A especificação da composição da etiqueta é demasiado longa! Indique apenas o essencial.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (txtComp.Text.Length > 20)
                {
                    MessageBox.Show("O nome do País especificado é demasiado longo! Use acrónimos ou abreviações.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
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

                prodPers.Etiqueta = new Etiqueta();
                if (rdEtiquetaExis.IsChecked == true)
                {
                    prodPers.Etiqueta = (Etiqueta)cbEtiqueta.SelectedItem;

                }
                else if (rdEtiquetaNova.IsChecked == true)
                {
                    prodPers.Etiqueta.Normas = txtNormas.Text;
                    prodPers.Etiqueta.Composicao = txtComp.Text;
                    prodPers.Etiqueta.PaisFabrico = txtPais.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            RegistarProdutoMateriais page = new RegistarProdutoMateriais(dataHandler, prodPers);
            this.NavigationService.Navigate(page);
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

        private ObservableCollection<MaterialTextil> getMateriais()
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
    }
}