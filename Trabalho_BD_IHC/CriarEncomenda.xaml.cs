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

namespace Trabalho_BD_IHC
{
    /// <summary>
    /// Interaction logic for RegistarCliente.xaml
    /// </summary>
    public partial class CriarEncomenda : Page
    {
        private DataHandler dataHandler;
        public CriarEncomenda(DataHandler dh)
        {
            InitializeComponent();
            this.dataHandler = dh;
        }           

        private void EnviarEncomenda(Encomenda enc)
        {

            if (!dataHandler.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

           /* cmd.CommandText = "INSERT INTO CLIENTE (NOME, NIB, NIF, EMAIL, TELEMOVEL, COD_POSTAL, RUA, N_PORTA) " +
                "VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @COD_POSTAL, @RUA, @N_PORTA);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", cl.Nome);
            cmd.Parameters.AddWithValue("@NIB", cl.Nib);
            cmd.Parameters.AddWithValue("@NIF", cl.Nif);
            cmd.Parameters.AddWithValue("@EMAIL", cl.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", cl.Telemovel);
            cmd.Parameters.AddWithValue("@COD_POSTAL", cl.CodigoPostal);
            cmd.Parameters.AddWithValue("@RUA", cl.Rua);
            cmd.Parameters.AddWithValue("@N_PORTA", cl.NCasa);
            cmd.Connection = dataHandler.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao criar encomenda na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                dataHandler.closeSGBDConnection();
            }*/
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmar_Click(object sender, RoutedEventArgs e)
        {
            Encomenda encomenda = new Encomenda();
            try
            {
               /* cliente.Nome = txtNome.Text;
                cliente.Nib = txtNIB.Text;
                cliente.Nif = txtNIF.Text;
                cliente.Telemovel = txtTelemovel.Text;
                cliente.Email = txtEmail.Text;
                cliente.CodigoPostal = txtcodigoPostal.Text;
                cliente.Rua = txtRua.Text;
                cliente.NCasa = int.Parse(txtNumeroPorta.Text);*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try {
                EnviarEncomenda(encomenda);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            this.NavigationService.GoBack();
        }
    }
}
