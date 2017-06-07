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
using System.Windows.Markup;
using System.Collections.ObjectModel;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarEncomenda : Page
    {
        private DataHandler dataHandler;
        private ObservableCollection<ProdutoPersonalizado> lista;

        public RegistarEncomenda(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            lista = new ObservableCollection<ProdutoPersonalizado>();
            produtosEncomenda.ItemsSource = lista;
            nEncomenda.Text = dataHandler.getLastIdentity("ENCOMENDA").ToString();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo da encomenda? Perderá todos os dados que tenha introduzido.",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void validarDados() {
            List<ProdutoPersonalizado> lista = ((IEnumerable<ProdutoPersonalizado>)this.produtosEncomenda.ItemsSource).ToList();
            if (txtCliente.Text.Equals("") || localEntrega.SelectedIndex==-1 || dataPrevista.SelectedDate == null)
            {
                throw new Exception("Por favor preencha todos os campos relativos á encomenda antes de avançar.");
            }else if (produtosEncomenda.Items.Count==0)
            {
                throw new Exception("Uma encomenda tem de ter pelo menos um produto!");
            }
            for (int i = 0; i < lista.Count; i++)
            {
                ProdutoPersonalizado prod = lista.ElementAt(i);
                if (prod.ProdutoBase.Referencia == null || prod.Tamanho == null || prod.ID == null || prod.Quantidade == null)
                    throw new Exception("Por favor preencha todos os dados referentes aos produtos da encomenda");
            }
            for (int i=0; i<produtosEncomenda.Items.Count; i++)
            {
                ProdutoPersonalizado prod = lista.ElementAt(i);
                if (!dataHandler.checkIfProdutoPersonalizadoExists(prod))
                {
                    throw new Exception("O produto especificado na linha "+i+" não existe está registado na base de dados. \nSe pretende registar esse produto por favor dirija-se á tab de produção.");
                }
            }
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try { 
                validarDados();
            }catch(Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Encomenda encomenda = new Encomenda();
            encomenda.Cliente = new Cliente();
            encomenda.GestorVendas = new Utilizador();
            encomenda.GestorVendas.NFuncionario = Utilizador.loggedUser.NFuncionario;
            DateTime currentDate = DateTime.Now;
            encomenda.Cliente.NCliente = Convert.ToInt32(txtCliente.Text);
            encomenda.DataConfirmacao = currentDate;
            encomenda.Desconto =  Convert.ToInt32(txtDesconto.Value);
            encomenda.GestorVendas.NFuncionario= Utilizador.loggedUser.NFuncionario;
            encomenda.LocalEntrega = localEntrega.SelectedItem.ToString();
            encomenda.DataPrevistaEntrega = dataPrevista.SelectedDate.Value;
            List<ProdutoPersonalizado> lista = ((IEnumerable<ProdutoPersonalizado>)this.produtosEncomenda.ItemsSource).ToList();


            try
            {
                dataHandler.EnviarEncomenda(encomenda, lista);
                Xceed.Wpf.Toolkit.MessageBox.Show("Encomenda Registada Com Sucesso!", "Envio de Encomenda", MessageBoxButton.OK, MessageBoxImage.Information);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProdutoPersonalizado prod = new ProdutoPersonalizado();
            prod.Quantidade = 1;
            List<ProdutoPersonalizado> someVar = ((IEnumerable<ProdutoPersonalizado>)this.produtosEncomenda.ItemsSource).ToList();
            someVar.Add(prod);
            produtosEncomenda.ItemsSource = someVar;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<ProdutoPersonalizado> someVar = ((IEnumerable<ProdutoPersonalizado>)this.produtosEncomenda.ItemsSource).ToList();
            if (someVar.Count == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Não existe mais nenhum produto a remover", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else { 
                someVar.RemoveAt(produtosEncomenda.SelectedIndex);
                produtosEncomenda.ItemsSource = someVar;
            }
        }

        private void produtosEncomenda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (produtosEncomenda.SelectedItems.Count > 0)
                remover.IsEnabled = true;
        }
    }
}

