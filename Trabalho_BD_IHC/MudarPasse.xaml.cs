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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for MudarPasse.xaml
    /// </summary>
    public partial class MudarPasse : Window
    {
        DataHandler dataHandler;
        public MudarPasse(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string pass = null;
            string oldPass = null;
            if (mostrarPass.IsChecked == true)
            {
                pass = passwordText.Text;
                oldPass = atuaPassText.Text;
            }
            else
            {
                pass = password.Password;
                oldPass = atualPass.Password;
            }

            if (atualPass.Password == "" || password.Password == "")
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Por favor indique a sua password antiga e a password para a qual pretende mudar e tente novamente", "Erro mudança de pass", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (dataHandler.changePass(Convert.ToInt32(Utilizador.loggedUser.NFuncionario), oldPass, pass))
                {
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("A password foi atualizada com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Close();
                }
                else
                {
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("A password introduzida está incorreta.", "Erro de Inicio de Sessão", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                atuaPassText.Text = atualPass.Password;
                atuaPassText.Visibility = Visibility.Visible;
                atualPass.Visibility = Visibility.Hidden;
            }
            else
            {
                password.Password = passwordText.Text;
                passwordText.Visibility = Visibility.Hidden;
                password.Visibility = Visibility.Visible;
                atualPass.Password = atuaPassText.Text;
                atuaPassText.Visibility = Visibility.Hidden;
                atualPass.Visibility = Visibility.Visible;
            }

        }
    }
}

