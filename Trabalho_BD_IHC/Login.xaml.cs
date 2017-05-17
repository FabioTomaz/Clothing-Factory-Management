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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DataHandler dataHandler;
        public Login()
        {
            InitializeComponent();
            this.dataHandler = new DataHandler();
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
                if (checkUser(nEmpregado.Text))
                {
                    if (checkLogin(nEmpregado.Text, pass)) {
                        Utilizador.loggedUser = dataHandler.getUtilizadorFromDB(Convert.ToInt32(nEmpregado.Text));
                        Utilizador.loggedUser.Supervisor = dataHandler.getUtilizadorFromDB(Utilizador.loggedUser.Supervisor.NFuncionario);
                        Console.WriteLine(Utilizador.loggedUser.NFuncionario);
                        Xceed.Wpf.Toolkit.MessageBox.Show("O inicio de sessão foi realizado com sucesso!", "Inicio de Sessão Concluido", MessageBoxButton.OK, MessageBoxImage.Information);
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


        private Boolean checkUser(String user)
        {
            dataHandler.verifySGBDConnection();
            int rows=0;
            SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZADOR WHERE N_FUNCIONARIO=@USER", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                rows += 1;
            }
            dataHandler.closeSGBDConnection();
            return (rows==1);
        }

        private Boolean checkLogin(String user, String password)
        {
            dataHandler.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, PASS FROM UTILIZADOR WHERE N_FUNCIONARIO=@USER", dataHandler.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            String bdPass = reader["PASS"].ToString();
            dataHandler.closeSGBDConnection();
            if (password.Equals(bdPass))
                return true;
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
