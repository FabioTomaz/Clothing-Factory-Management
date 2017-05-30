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
    /// Interaction logic for RegistarMaterial.xaml
    /// </summary>
    public partial class RegistarMaterial : Page
    {
        private DataHandler dataHandler;


        public RegistarMaterial(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
        }


        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {

            if (!IsLoaded) return;

            ComboBoxItem cbo = (ComboBoxItem)tipoMaterial.SelectedItem;

            hideAll();
            if (cbo.Name.Equals("Acessorios", StringComparison.Ordinal))
            {
                acessorios.Visibility = Visibility.Visible;
                acessoriosLabel.Visibility = Visibility.Visible;
                fecho.Visibility = Visibility.Visible;
            }
            else
            {
                acessorios.Visibility = Visibility.Hidden;
                acessoriosLabel.Visibility = Visibility.Hidden;
                if (cbo.Name.Equals("Pano", StringComparison.Ordinal))
                {
                    pano.Visibility = Visibility.Visible;
                }
                else if (cbo.Name.Equals("Linha", StringComparison.Ordinal)) {
                    linha.Visibility = Visibility.Visible;
                }
            }
        }
    


        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            MaterialTextil material=null;

            if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Pano"))
            {
                material = new Pano();
                material.Fornecedor = new Fornecedor();
                material.Cor = corMaterial.SelectedColor.ToString();
                material.Designacao = txtDescriçãoMaterial.Text;
                material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                ((Pano)material).Gramagem = (int)txtGramagem.Value;
                ((Pano)material).PrecoMetroQuadrado = (Double)txtPreçoM2.Value;
                ((Pano)material).Tipo = txtTipoPano.Text;
            }
            else if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Linha"))
            {
                material = new Linha();
                material.Fornecedor = new Fornecedor();
                material.Cor = corMaterial.SelectedColor.ToString();
                material.Designacao = txtDescriçãoMaterial.Text;
                material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                ((Linha)material).Preco100Metros = Convert.ToDouble(txtPreço100M.Text);
                ((Linha)material).Grossura = Convert.ToDouble(txtGrossura.Text);
            }
            else if (((ComboBoxItem)tipoMaterial.SelectedItem).Name.Equals("Acessorios"))
            {
                if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Fecho"))
                {
                    material = new Fecho();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((Fecho)material).TamanhoDente = Convert.ToDouble(txtTamanhoDente.Text);
                    ((Fecho)material).Largura = Convert.ToDouble(txtLarguraFecho.Text);
                    ((Fecho)material).Comprimento = Convert.ToDouble(txtComprimentoFecho.Text);
                    ((Fecho)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeFecho.Text);
                    Console.WriteLine(((Fecho)material).PrecoUnidade);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Mola"))
                {
                    material = new Mola();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((Mola)material).Diametro = Convert.ToDouble(txtDiametroMola.Text);
                    ((Mola)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeMola.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Botao"))
                {
                    material = new Botao();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((Botao)material).Diametro = Convert.ToDouble(txtDiametroBotao.Text);
                    ((Botao)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeBotao.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("FitaVelcro"))
                {
                    material = new FitaVelcro();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((FitaVelcro)material).Largura = Convert.ToDouble(txtLarguraFita.Text);
                    ((FitaVelcro)material).Comprimento = Convert.ToDouble(txtComprimentoFita.Text);
                    ((FitaVelcro)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeFita.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Elastico"))
                {
                    material = new Elastico();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((Elastico)material).Largura = Convert.ToDouble(txtLarguraFita.Text);
                    ((Elastico)material).Comprimento = Convert.ToDouble(txtComprimentoFita.Text);
                    ((Elastico)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidadeElastico.Text);
                }
                else if (((ComboBoxItem)acessorios.SelectedItem).Name.Equals("Outro"))
                {
                    material = new AcessoriosCostura();
                    material.Fornecedor = new Fornecedor();
                    material.Cor = corMaterial.SelectedColor.ToString();
                    material.Designacao = txtDescriçãoMaterial.Text;
                    material.Fornecedor.NIF_Fornecedor = txtFornecedorNif.Text;
                    material.ReferenciaFornecedor = txtReferenciaFornecedor.Text;
                    ((AcessoriosCostura)material).PrecoUnidade = Convert.ToDouble(txtPrecoUnidade.Text);
                }
            }

            try
            {
                dataHandler.inserirMaterial(material);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Material adicionado á base de dados.", "Operação Concluida", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Tem a certeza que deseja cancelar o registo do material? Perderá todos os dados que tenha introduzido",
                 "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {//sim
                this.NavigationService.GoBack();
            }
        }

        private void acessorios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            hideAll();

            ComboBoxItem cbo = (ComboBoxItem)acessorios.SelectedItem;

            if (cbo.Name.Equals("Fecho", StringComparison.Ordinal))
            {
                fecho.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Elastico", StringComparison.Ordinal))
            {
                elastico.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("FitaVelcro", StringComparison.Ordinal))
            {
                fitaVelcro.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Mola", StringComparison.Ordinal))
            {
                mola.Visibility = Visibility.Visible;
            }
            else if (cbo.Name.Equals("Botao", StringComparison.Ordinal))
            {
                botao.Visibility = Visibility.Visible;
            }
            else
            {
                outroTipoAcessorio.Visibility = Visibility.Visible;
            }
        }

        private void hideAll() {
            pano.Visibility = Visibility.Hidden;
            linha.Visibility = Visibility.Hidden;
            mola.Visibility = Visibility.Hidden;
            fecho.Visibility = Visibility.Hidden;
            botao.Visibility = Visibility.Hidden;
            elastico.Visibility = Visibility.Hidden;
            fitaVelcro.Visibility = Visibility.Hidden;
            outroTipoAcessorio.Visibility = Visibility.Hidden;
        }
    }
}

