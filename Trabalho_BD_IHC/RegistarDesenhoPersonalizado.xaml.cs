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
    public partial class RegistarDesenhoPersonalizado : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;
        private ObservableCollection<DesenhoBase> desenhoBase;
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

        public ObservableCollection<DesenhoBase> DesenhoBase
        {
            get
            {
                return desenhoBase;
            }

            set
            {
                desenhoBase = value;
            }
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {/*
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ListarDesenhos ld = new ListarDesenhos(dataHandler);
                desenhoBase = ld.getDesenhosBase();
                cbDesenhos.ItemsSource = desenhoBase;
                if (desenhoBase.Count > 0)
                {
                    DesenhoBase firstDes = desenhoBase.First();
                    cbDesenhos.SelectedItem = firstDes ;
                }
            }*/
        }

        public RegistarDesenhoPersonalizado(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }

        private void EnviarEtiqueta(Etiqueta et)
        {
            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO ETIQUETA (NORMAS, PAIS_FABRICO, COMPOSICAO) VALUES " +
                "(@Normas, @Pais, @Comp);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Normas", et.Normas);
            cmd.Parameters.AddWithValue("@Pais", et.PaisFabrico);
            cmd.Parameters.AddWithValue("@Comp", et.Composicao);
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar a etiqueta na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }
        }

        //funçao para se obter o numero da etiqueta que se acabou de registar
        private int getNetiqueta(String normas, String pais, String comp)
        {
            int n = 0;
            if (!dataHandler.verifySGBDConnection())
            {
                MessageBoxResult result = MessageBox.Show("A conexão à base de dados é instável ou inexistente. Por favor tente mais tarde", "Erro de Base de Dados", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT N_ETIQUETA FROM ETIQUETA "
                + " WHERE(NORMAS = @Normas AND PAIS_FABRICO = @Pais AND COMPOSICAO = @Comp);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Normas", normas);
                cmd.Parameters.AddWithValue("@Pais", pais);
                cmd.Parameters.AddWithValue("@Comp", comp);
                cmd.Connection = dataHandler.Cn;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    n = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                }
                dataHandler.closeSGBDConnection();
                
            }
            return n;
        }

        private void EnviarDesenhoPersonalizado(DesenhoPersonalizado desenhoPers)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

             cmd.CommandText = "INSERT INTO MODELO (N_DESENHO, N_ETIQUETA) VALUES " +
                 "(@N_DesenhoBase, @N_Etiqueta);";
             cmd.Parameters.Clear();
             cmd.Parameters.AddWithValue("@N_DesenhoBase", desenhoPers.Desenho.NDesenho);
             cmd.Parameters.AddWithValue("@N_Etiqueta", desenhoPers.Etiqueta.Numero);
             cmd.Connection = dataHandler.Cn;
             try
             {
                 cmd.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 throw new Exception("Falha ao registar o Desenho Personalizado na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
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
            DesenhoPersonalizado desPer = new DesenhoPersonalizado();
            desPer.Desenho = (DesenhoBase)cbDesenhos.SelectedItem;
            try
            {
                //se a visibilidade da grid que contém a combo box com a lista das etiquetas registadas
                //for Visible, significa q o utilizador usou uma etiqueta já registada
                if(etiquetaExisente.Visibility == Visibility.Visible)
                {
                    desPer.Etiqueta = (Etiqueta)cbEtiqueta.SelectedItem;
                }
                // caso a grid que contem o formulário para a criação de uma nova etiqueta seja visivel,
                //o utilizador deciciu criar uma nova etiqueta, registá-la na base de dados e usá-la para
                //o registo do desenho personalizado
                else if ( etiquetaNova.Visibility == Visibility.Visible)
                {
                    Etiqueta et = new Etiqueta();
                    et.Normas = txtNormas.Text;
                    et.Composicao = txtComp.Text;
                    et.PaisFabrico = txtPais.Text;
                    EnviarEtiqueta(et);
                    et.Numero = getNetiqueta(et.Normas, et.PaisFabrico, et.Composicao);
                    desPer.Etiqueta = et;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                EnviarDesenhoPersonalizado(desPer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Desenho Personalizado registado com sucesso!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
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
                ObservableCollection<Etiqueta> et =  getEtiquetas();
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
            return etiquetas;
        }
    }
}
