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
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesFilial.xaml
    /// </summary>
    public partial class DetalhesFilial : Window
    {
        filial filial;
        DataHandler dataHandler;
        public DetalhesFilial(DataHandler dataHandler, filial filial)
        {
            this.filial = filial;
            this.dataHandler = dataHandler;
            InitializeComponent();
            nFil.Text = (dataHandler.getNfilialFromDB(filial.Email, filial.Telefone)).ToString();
            email.Text = filial.Email;
            fax.Text = filial.Fax;
            telefone.Text = filial.Telefone;
            distrito.Text = filial.Localizacao.Distrito;
            localidade.Text = filial.Localizacao.Localidade;
            cdgPostal.Text = filial.Localizacao.CodigoPostal;
            rua.Text = filial.Localizacao.Rua1+ ", nº "+ filial.Localizacao.Porta.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Utilizador> users = dataHandler.getUtilizadoresFromFilial(filial.NFilial);
            if (users != null)
                utilizadores.ItemsSource = users;
        }

        private void utilizadores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (utilizadores.SelectedItems.Count == 1) { 
                //DetalhesUtilizador window = new DetalhesUtilizador(dataHandler, (Utilizador)utilizadores.SelectedItem);
                //window.Show();
            }
        }
    }
}
