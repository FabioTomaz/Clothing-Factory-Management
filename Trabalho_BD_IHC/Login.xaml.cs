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
using System.Text.RegularExpressions;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DataHandler dataHandler;
        public Login()
        {
            InitializeComponent();
            this.dataHandler = new DataHandler();
            nEmpregado.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pass = null;
            if (mostrarPass.IsChecked == true)
            {
                pass = passwordText.Text;
            }
            else {
                pass = password.Password;
            }

            if (nEmpregado.Text=="" || password.Password=="") {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Por favor preencha os campos de incio de sessão e tente novamente.", "Erro de Inicio de Sessão", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                if (dataHandler.checkUser(nEmpregado.Text))
                {
                    if (dataHandler.checkLogin(nEmpregado.Text, pass)) {
                        Utilizador.loggedUser = dataHandler.getUtilizadorFromDB(Convert.ToInt32(nEmpregado.Text));
                        Utilizador.loggedUser.Supervisor = dataHandler.getUtilizadorFromDB(Utilizador.loggedUser.Supervisor.NFuncionario);
                        this.Hide();
                        MainWindow main = new MainWindow(dataHandler);
                        main.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("A password introduzida está incorreta.", "Erro de Inicio de Sessão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else {
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("O numero de funcionário introduzido não está registado no sistema. Por favor reeintroduza parametros de inicio de sessão corretos e tente novamente.", "Erro de Inicio de Sessão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                passwordText.Text = password.Password;
                passwordText.Visibility = Visibility.Visible;
                password.Visibility = Visibility.Hidden;
            }
            else {
                password.Password = passwordText.Text;
                passwordText.Visibility = Visibility.Hidden;
                password.Visibility = Visibility.Visible;
            }
             
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Ajuda window = new Ajuda();
            window.ShowDialog();
        }

        private void nEmpregado_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
