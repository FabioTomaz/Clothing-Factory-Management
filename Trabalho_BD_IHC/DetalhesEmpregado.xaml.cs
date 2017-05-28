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
using System.IO;
using System.Drawing.Imaging;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for DetalhesEncomenda.xaml
    /// </summary>
    public partial class DetalhesEmpregado : Window
    {
        Utilizador emp;
        DataHandler dataHandler;
        public DetalhesEmpregado(DataHandler dataHandler, Utilizador emp)
        {
            this.emp = emp;
            this.dataHandler = dataHandler;
            InitializeComponent();
            nomeEmpregado.Text = emp.Nome;
            nEmpregado.Text = emp.NFuncionario.ToString();
            nFilial.Text = emp.Filial.NFilial.ToString();
            salario.Text = emp.Salario.ToString();
            entrada.Text = emp.HoraEntrada.ToString();
            saida.Text = emp.HoraSaida.ToString(); ;
            email.Text = emp.Email;
            telemovel.Text = emp.Telemovel;
            cdgPostal.Text = emp.Localizacao.CodigoPostal;
            distrito.Text = emp.Localizacao.Distrito;
            localidade.Text = emp.Localizacao.Localidade;
            string userTypes = "";
            string lastItem = emp.TiposUser[emp.TiposUser.Count - 1];
            foreach (string s in emp.TiposUser)
            {
                if(s.Equals(lastItem, StringComparison.Ordinal))
                    userTypes += s + ".";
                else
                    userTypes += s + ", ";
            }
            funcao.Text = userTypes;
            morada.Text = emp.Localizacao.Rua1 + ", nº " + emp.Localizacao.Porta;
            if (emp.Imagem != null)
            {
                var ms = new MemoryStream();
                Utilizador.loggedUser.Imagem.Save(ms, ImageFormat.Png);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                userImage.Source = bi;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Utilizador u = dataHandler.getSupervisor(emp.Supervisor.NFuncionario);
            nome.Text = u.Nome;
            nFunc.Text = u.NFuncionario.ToString();
            emailSup.Text = u.Email;
            telSup.Text = u.Telemovel;
            supEntrada.Text = u.HoraEntrada.ToString();
            supSaida.Text = u.HoraSaida.ToString();
            if (u.Imagem != null)
            {
                var ms = new MemoryStream();
                Utilizador.loggedUser.Imagem.Save(ms, ImageFormat.Png);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                SupImage.Source = bi;
            }
        }

    }
}
