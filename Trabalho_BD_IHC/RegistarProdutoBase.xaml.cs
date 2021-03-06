﻿using System;
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
using System.IO;
using System.Data;

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarEncomenda.xaml
    /// </summary>
    public partial class RegistarProdutoBase : Page
    {
        private DataHandler dataHandler;
        private int currentRow = 1;
        String imgLoc;
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


        public RegistarProdutoBase(DataHandler dataHandler)
        {
            InitializeComponent();
            this.dataHandler = dataHandler;
            refProduto.Content = dataHandler.getLastIdentity("[PRODUTO-BASE]") + 1;
        }

        private void validar()
        {
            if (txtNomeModelo.Text.Equals(""))
                throw new Exception("Não foi escolhido um nome para o produto!");
            else if (txtInstruçoes.Text.Equals(""))
                throw new Exception("Não foram especificadas as instruções de produção deste produto!");
            else if (imgPhoto.Source == null)
                throw new Exception("Não foi introduzida uma foto do desenho do produto!");
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                validar();
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ProdutoBase ProdutoBase = new ProdutoBase();
            ProdutoBase.Nome = txtNomeModelo.Text;
            ProdutoBase.InstrProd = txtInstruçoes.Text;
            ProdutoBase.IVA1 = txtIva.Value;
            ProdutoBase.GestorProducao = new Utilizador();
            ProdutoBase.GestorProducao.NFuncionario = Utilizador.loggedUser.NFuncionario; //---> suposto mais tarde colocar o nº do user
            byte[] images = null;
            FileStream stream = new FileStream(imgLoc, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(stream);
            images = br.ReadBytes((int)stream.Length);
            ProdutoBase.Pic = images;
            try
            {
                dataHandler.registarProdutoBase(ProdutoBase);
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message, "ERRO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Xceed.Wpf.Toolkit.MessageBox.Show("A informação do desenho do produto foi registada com sucesso!", "SUCESSO", MessageBoxButton.OK, MessageBoxImage.Information);
            this.NavigationService.GoBack();
        }

        private void btnAdicionarImagem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                imgLoc = op.FileName.ToString();
            }
        }

        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        private void removerFoto_Click(object sender, RoutedEventArgs e)
        {
            imgPhoto.Source = null;
        }
    }
}
