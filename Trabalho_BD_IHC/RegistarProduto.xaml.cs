﻿using System;
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
        private ListarMateriais listaMateriais;
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

        public RegistarProduto(DataHandler dataHandler, ListarMateriais listaMateriais)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            this.listaMateriais = listaMateriais;
            materiaisSelecionados = new ObservableCollection<MaterialTextil>();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo do produto? Perderá todos os dados que tenha introduzido",
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
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, selecione o tamanho a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //verificar se uma cor foi selecionada
            if (txtCor.SelectedColorText == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, selecione uma cor a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPreço.Text == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, indique o preço a atribuir ao produto!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Convert.ToDouble(txtPreço.Text) <= 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("O preço do produto deve ser maior que 0!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (rdEtiquetaExis.IsChecked == false && rdEtiquetaNova.IsChecked == false)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, adicione uma etiqueta existente ao produto, ou crie uma nova!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //validar os formularios de preenchimento de uma etiqueta nova
            if (rdEtiquetaNova.IsChecked == true)
            {
                if (txtNormas.Text.Length == 0 || txtComp.Text.Length == 0 || txtPais.Text.Length == 0)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Por favor, preencha todos os campos relativos à criação de nova etiqueta!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtNormas.Text.Length > 100)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("A especificação das normas é demasiado longa! Indique apenas o essencial.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtComp.Text.Length > 100)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("A especificação da composição da etiqueta é demasiado longa! Indique apenas o essencial.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (txtPais.Text.Length > 20)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("O nome do País especificado é demasiado longo! Use acrónimos ou abreviações.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            ProdutoPersonalizado prodPers = new ProdutoPersonalizado();
            Boolean inserirEtiqueta=true;
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
                    inserirEtiqueta = false;
                }
                else if (rdEtiquetaNova.IsChecked == true)
                {
                    prodPers.Etiqueta.Normas = txtNormas.Text;
                    prodPers.Etiqueta.Composicao = txtComp.Text;
                    prodPers.Etiqueta.PaisFabrico = txtPais.Text;
                    inserirEtiqueta = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            RegistarProdutoMateriais page = new RegistarProdutoMateriais(dataHandler, prodPers, listaMateriais, inserirEtiqueta);
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
                ListarProdutos lp = new ListarProdutos(dataHandler, listaMateriais);
                ObservableCollection<ProdutoBase> prodBase = dataHandler.getProdutosBaseFromDB();
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
            ObservableCollection<Etiqueta> et = dataHandler.getEtiquetas();
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

       

    }
}