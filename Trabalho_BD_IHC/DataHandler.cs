using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;

namespace Trabalho_BD_IHC
{
    public class DataHandler
    {
        private SqlConnection cn;

        public SqlConnection Cn
        {
            get
            {
                return cn;
            }

            set
            {
                cn = value;
            }
        }

        public DataHandler()
        {
            Cn = getSGBDConnection();
        }
        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=tcp: 193.136.175.33\\SQLSERVER2012,8293; initial catalog=p4g3;"
                + " User ID=p4g3; Password=fabiobruno;");
        }
        /*db-->> data source=tcp: 193.136.175.33\\SQLSERVER2012,8293; initial catalog=p4g3;"
                + " User ID=p4g3; Password=fabiobruno;*/
        /*localhost --> "data source=localhost;integrated security=true;initial catalog=GESTAO-FABRICA-VESTUARIO-LABORAL"
         */
        public bool verifySGBDConnection()
        {
            if (Cn == null)
                Cn = getSGBDConnection();

            if (Cn.State != ConnectionState.Open)
                try
                {
                    Cn.Open();
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    return false;
                }


            return Cn.State == ConnectionState.Open;
        }


        public bool closeSGBDConnection()
        {
            if (Cn.State != ConnectionState.Closed)
                Cn.Close();

            return Cn.State == ConnectionState.Closed;
        }

        public Boolean checkUser(String user)
        {
            if (!verifySGBDConnection())
            {
                throw new Exception("Não foi possivel inicar sessão na base de dados");
            }
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZADOR WHERE N_FUNCIONARIO=@USER", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows += 1;
            }
            closeSGBDConnection();
            return (rows == 1);
        }


        public Boolean checkLogin(String user, String password)
        {
            if (!verifySGBDConnection())
            {
                throw new Exception("Não foi possivel inicar sessão na base de dados");
            }
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, PASS FROM UTILIZADOR WHERE N_FUNCIONARIO=@USER", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            String bdPass = reader["PASS"].ToString();
            closeSGBDConnection();
            if (password.Equals(bdPass))
                return true;
            return false;
        }

        public void registarCliente(Cliente cl)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO CLIENTE (NOME, NIB, NIF, EMAIL, TELEMOVEL, CODPOSTAL1, CODPOSTAL2, RUA, N_PORTA) " +
                "VALUES (@NOME, @NIB, @NIF, @EMAIL, @TELEMOVEL, @CODPOSTAL1, @CODPOSTAL2, @RUA, @N_PORTA);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", cl.Nome);
            if (string.IsNullOrEmpty(cl.Nib))
                cmd.Parameters.AddWithValue("@NIB", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@NIB", cl.Nib);
            if (string.IsNullOrEmpty(cl.Nib))
                cmd.Parameters.AddWithValue("@NIF", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@NIF", cl.Nif);

            cmd.Parameters.AddWithValue("@EMAIL", cl.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", cl.Telemovel);
            cmd.Parameters.AddWithValue("@CODPOSTAL1", Convert.ToInt32(cl.CodigoPostal.Split('-')[0]));
            cmd.Parameters.AddWithValue("@CODPOSTAL2", Convert.ToInt32(cl.CodigoPostal.Split('-')[1]));
            cmd.Parameters.AddWithValue("@RUA", cl.Rua);
            cmd.Parameters.AddWithValue("@N_PORTA", cl.NCasa);
            cmd.Connection = Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:
                        throw new Exception("O código postal que indicou não existe. Por favor indique um código postal válido.");
                    default:
                        throw new Exception("Ocorreu um erro inesperado. Por favor contacte o seu administrador da base de dados.\n Erro:\n" + ex); ;
                }
            }
            finally
            {
                closeSGBDConnection();
            }
        }

        public void editarCliente(Cliente cl)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE CLIENTE SET NIF = @NIF, NIB=@NIB, EMAIL=@EMAIL, TELEMOVEL=@TELEMOVEL, CODPOSTAL1=@CODPOSTAL1, CODPOSTAL2=@CODPOSTAL2, RUA=@RUA, N_PORTA=@N_PORTA " +
                "WHERE CLIENTE.NCLIENTE=@NCLIENTE;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NIB", cl.Nib);
            cmd.Parameters.AddWithValue("@NIF", cl.Nif);
            cmd.Parameters.AddWithValue("@EMAIL", cl.Email);
            cmd.Parameters.AddWithValue("@TELEMOVEL", cl.Telemovel);
            cmd.Parameters.AddWithValue("@CODPOSTAL1", Convert.ToInt32(cl.CodigoPostal.Split('-')[0]));
            cmd.Parameters.AddWithValue("@CODPOSTAL2", Convert.ToInt32(cl.CodigoPostal.Split('-')[1]));
            cmd.Parameters.AddWithValue("@RUA", cl.Rua);
            cmd.Parameters.AddWithValue("@N_PORTA", cl.NCasa);
            cmd.Parameters.AddWithValue("@NCLIENTE", cl.NCliente);
            cmd.Connection = Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 547:
                        throw new Exception("O código postal que indicou não existe. Por favor indique um código postal válido.");
                    default:
                        throw new Exception("Ocorreu um erro inesperado. Por favor contacte o seu administrador da base de dados.\n Erro:\n" + ex); ;
                }

            }
            finally
            {
                closeSGBDConnection();
            }
        }

        public ObservableCollection<MaterialTextil> getMateriaisFromProdutoPersonalizado(ProdutoPersonalizado prod)
        {
            verifySGBDConnection();
            ObservableCollection<MaterialTextil> result = new ObservableCollection<MaterialTextil>();
            SqlCommand cmd = new SqlCommand("select * from dbo.getMateriaisFromProdutoPersonalizado(@ref, @tam, @cor, @id);", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", prod.ProdutoBase.Referencia);
            cmd.Parameters.AddWithValue("@tam", prod.Tamanho);
            cmd.Parameters.AddWithValue("@cor", prod.Cor);
            cmd.Parameters.AddWithValue("@id", prod.ID);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MaterialTextil mat = new MaterialTextil();
                mat.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                mat.Cor = reader["COR"].ToString();
                mat.Designacao = reader["DESIGNACAO"].ToString();
                mat.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                mat.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                mat.QuantidadeSelecionadaD = Convert.ToInt32(reader["QUANTIDADE"].ToString());
                //mat.Preco =;
                result.Add(mat);
            }
            closeSGBDConnection();
            return result;
        }

        public int getEncomendasDesteMes()
        {
            verifySGBDConnection();
            int result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getEncomendasMes()", cn);
            result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            closeSGBDConnection();
            return result;
        }

        public Double getSaldoDesteMes()
        {
            verifySGBDConnection();
            Double result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getLucroGeradoMes()", cn);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToDouble(strResult);
            closeSGBDConnection();
            return result;
        }

        public String cancelarEncomenda(int nEncomenda)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("dbo.cancelarEncomenda", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            // set up the parameters
            cmd.Parameters.Add("@nEncomenda", SqlDbType.Int);
            cmd.Parameters.Add("@out", SqlDbType.VarChar, 70).Direction = ParameterDirection.Output;

            // set parameter values
            cmd.Parameters["@nEncomenda"].Value = nEncomenda;

            // execute stored procedure
            cmd.ExecuteNonQuery();

            // read output value from @NewId
            String strResult = cmd.Parameters["@out"].Value.ToString();
            return strResult;
        }

        public Double getDinheiroGastoMes()
        {
            verifySGBDConnection();
            Double result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getDinheiroGastoMes()", cn);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToDouble(strResult);
            closeSGBDConnection();
            return result;
        }

        public Double getDinheiroGeradoMes()
        {
            verifySGBDConnection();
            Double result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getDinheiroGeradoMes()", cn);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToDouble(strResult);
            closeSGBDConnection();
            return result;
        }

        public int getNProdutosVendidosAteHoje()
        {
            verifySGBDConnection();
            int result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getTotalProdutosVendidos()", cn);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToInt32(strResult);
            closeSGBDConnection();
            return result;
        }

        public int getNProdutosVendidosMes()
        {
            verifySGBDConnection();
            int result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getTotalProdutosVendidosMes()", cn);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToInt32(strResult);
            closeSGBDConnection();
            return result;
        }

        public ObservableCollection<Cliente> getClientesFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Cliente> items = new ObservableCollection<Cliente>();
            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.Nome = reader["NOME"].ToString();
                C.Nif = reader["NIF"].ToString();
                C.Nib = reader["NIB"].ToString();
                C.Email = reader["EMAIL"].ToString();
                C.Telemovel = reader["TELEMOVEL"].ToString();
                C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                C.Rua = reader["RUA"].ToString();
                C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                C.Distrito = reader["DISTRITO"].ToString();
                C.Localidade = reader["LOCALIDADE"].ToString();
                items.Add(C);
            }
            closeSGBDConnection();
            return items;
        }

        public double getQuantidadeMaterial(int referencia)
        {
            verifySGBDConnection();
            double result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getQuantidadeMaterial(@referencia)", cn);
            cmd.Parameters.AddWithValue("@referencia", referencia);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToDouble(strResult);
            closeSGBDConnection();
            return result;
        }

        public double getPrecoMaterial(int referencia)
        {
            verifySGBDConnection();
            double result = 0;
            SqlCommand cmd = new SqlCommand("select dbo.getPrecoMaterial(@referencia)", cn);
            cmd.Parameters.AddWithValue("@referencia", referencia);
            String strResult = cmd.ExecuteScalar().ToString();
            if (!strResult.Equals(""))
                result = Convert.ToDouble(strResult);
            closeSGBDConnection();
            return result;
        }

        public ObservableCollection<Cliente> getClientesInDBFromName(string nome)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2 "
                + "WHERE NOME LIKE @nome", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Cliente> items = new ObservableCollection<Cliente>();
            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.Nome = reader["NOME"].ToString();
                C.Nif = reader["NIF"].ToString();
                C.Nib = reader["NIB"].ToString();
                C.Email = reader["EMAIL"].ToString();
                C.Telemovel = reader["TELEMOVEL"].ToString();
                C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                C.Rua = reader["RUA"].ToString();
                C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                C.Distrito = reader["DISTRITO"].ToString();
                C.Localidade = reader["LOCALIDADE"].ToString();
                items.Add(C);
            }
            closeSGBDConnection();
            return items;
        }

        public ObservableCollection<Cliente> getClientesInDBFromEmail(string email)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2 "
                + "WHERE EMAIL LIKE @email", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@email", "%" + email + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Cliente> items = new ObservableCollection<Cliente>();
            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.Nome = reader["NOME"].ToString();
                C.Nif = reader["NIF"].ToString();
                C.Nib = reader["NIB"].ToString();
                C.Email = reader["EMAIL"].ToString();
                C.Telemovel = reader["TELEMOVEL"].ToString();
                C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                C.Rua = reader["RUA"].ToString();
                C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                C.Distrito = reader["DISTRITO"].ToString();
                C.Localidade = reader["LOCALIDADE"].ToString();
                items.Add(C);
            }
            closeSGBDConnection();
            return items;
        }

        public Cliente getClientesInDBnCliente(int nCliente)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2 "
                + "WHERE NCLIENTE = @nCliente", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nCliente", nCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            Cliente C = new Cliente();
            reader.Read();


            C.Nome = reader["NOME"].ToString();
            C.Nif = reader["NIF"].ToString();
            C.Nib = reader["NIB"].ToString();
            C.Email = reader["EMAIL"].ToString();
            C.Telemovel = reader["TELEMOVEL"].ToString();
            C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
            C.Rua = reader["RUA"].ToString();
            C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
            C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
            C.Distrito = reader["DISTRITO"].ToString();
            C.Localidade = reader["LOCALIDADE"].ToString();

            closeSGBDConnection();
            return C;
        }

        public ObservableCollection<Cliente> getClientesInDBNIF(string NIF)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2 "
                + "WHERE NIF LIKE @NIF", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NIF", "%" + NIF + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Cliente> items = new ObservableCollection<Cliente>();
            while (reader.Read())
            {
                Cliente C = new Cliente();
                C.Nome = reader["NOME"].ToString();
                C.Nif = reader["NIF"].ToString();
                C.Nib = reader["NIB"].ToString();
                C.Email = reader["EMAIL"].ToString();
                C.Telemovel = reader["TELEMOVEL"].ToString();
                C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                C.Rua = reader["RUA"].ToString();
                C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
                C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                C.Distrito = reader["DISTRITO"].ToString();
                C.Localidade = reader["LOCALIDADE"].ToString();
                items.Add(C);
            }
            reader.Close();

            closeSGBDConnection();
            return items;
        }

        public Cliente getClienteFromDB(int nCliente)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE JOIN ZONA ON CLIENTE.CODPOSTAL1=ZONA.CODPOSTAL1 AND CLIENTE.CODPOSTAL2=ZONA.CODPOSTAL2 WHERE NCLIENTE=@cliente", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@cliente", nCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            Cliente C = new Cliente();
            reader.Read();
            C.Nome = reader["NOME"].ToString();
            C.Nif = reader["NIF"].ToString();
            C.Nib = reader["NIB"].ToString();
            C.Email = reader["EMAIL"].ToString();
            C.Telemovel = reader["TELEMOVEL"].ToString();
            C.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
            C.Rua = reader["RUA"].ToString();
            C.NCasa = Convert.ToInt32(reader["N_PORTA"].ToString());
            C.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
            C.Distrito = reader["DISTRITO"].ToString();
            C.Localidade = reader["LOCALIDADE"].ToString();
            closeSGBDConnection();
            return C;
        }

        public ObservableCollection<Encomenda> getEncomendasFromCliente(int nCliente)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA, "
                    + "LOCALENTREGA, ESTADO, N_GESTOR_VENDA, UTILIZADOR.NOME AS GESTOR_NOME FROM ENCOMENDA JOIN "
                    + "UTILIZADOR ON N_FUNCIONARIO = N_GESTOR_VENDA WHERE CLIENTE = @nCliente"
                    , Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nCliente", nCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> list = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda enc = new Encomenda();
                enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"].ToString());
                if (reader["DATA_ENTREGA"].ToString() == null || reader["DATA_ENTREGA"].ToString().Equals("", StringComparison.Ordinal))
                {
                    enc.DataEntrega = null;
                }
                else
                {
                    enc.DataEntrega = Convert.ToDateTime(reader["DATA_ENTREGA"].ToString());
                }
                enc.LocalEntrega = reader["LOCALENTREGA"].ToString();
                enc.Estado = reader["ESTADO"].ToString();
                enc.GestorVendas = new Utilizador();
                enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                enc.GestorVendas.Nome = reader["GESTOR_NOME"].ToString();
                list.Add(enc);
            }
            closeSGBDConnection();
            return list;
        }

        public void atualizarEncomenda(Encomenda encomenda)
        {
            int rows = 0;
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("UPDATE ENCOMENDA SET DESCONTO=@DESCONTO, DATA_ENTREGA_PREV = @DATEP WHERE N_ENCOMENDA=@ENCOMENDA"
                                , Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ENCOMENDA", encomenda.NEncomenda);
            cmd.Parameters.AddWithValue("@DESCONTO", encomenda.Desconto);
            cmd.Parameters.AddWithValue("@DATEP", encomenda.DataPrevistaEntrega);

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o contacto na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows != 1)
                    throw new Exception("Algo de errado ocorreu durante a atualização da encomenda na base de dados.");
                closeSGBDConnection();
            }
        }

        public ObservableCollection<Encomenda> getEncomendasFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA,[LOCAIS-ENTREGA-ENCOMENDA].DESCRICAO AS LOCALENTREGA ,DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA, SUM([PRODUTO-PERSONALIZADO].PRECO*CONTEUDO_ENCOMENDA.QUANTIDADE)*(1-(DESCONTO/100)) AS PRECOTOTAL "
                                + " FROM ENCOMENDA JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE"
                                + " JOIN CONTEUDO_ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=ENCOMENDA.N_ENCOMENDA"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " AND CONTEUDO_ENCOMENDA.TAMANHO_PRODUTO = [PRODUTO-PERSONALIZADO].TAMANHO "
                                + " AND CONTEUDO_ENCOMENDA.COR_PRODUTO = [PRODUTO-PERSONALIZADO].COR"
                                + " AND CONTEUDO_ENCOMENDA.ID_PRODUTO = [PRODUTO-PERSONALIZADO].ID"
                                + " JOIN ESTADO ON ENCOMENDA.ESTADO=ESTADO.ID"
                                + " JOIN [LOCAIS-ENTREGA-ENCOMENDA] ON ENCOMENDA.LOCALENTREGA=[LOCAIS-ENTREGA-ENCOMENDA].ID"
                                + " GROUP BY [LOCAIS-ENTREGA-ENCOMENDA].DESCRICAO, ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA;"
                                , cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> enc = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda Enc = new Encomenda();
                Enc.Cliente = new Cliente();
                Enc.GestorVendas = new Utilizador();
                Enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                Enc.Estado = reader["DESCRIÇAO"].ToString();
                Enc.DataPrevistaEntrega = Convert.ToDateTime(reader["DATA_ENTREGA_PREV"]);
                Enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"]);
                Enc.Desconto = Convert.ToInt32(reader["DESCONTO"]);
                Enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                Enc.Cliente.Nome = reader["NOME"].ToString();
                Enc.Cliente.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                Enc.LocalEntrega = reader["LOCALENTREGA"].ToString();
                Enc.Preco = Convert.ToDouble(reader["PRECOTOTAL"].ToString());
                enc.Add(Enc);
            }
            closeSGBDConnection();
            return enc;
        }


        public Encomenda getEncomendaFromDB(int nEncomenda)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME AS CNOME, N_GESTOR_VENDA, SUM([PRODUTO-PERSONALIZADO].PRECO*CONTEUDO_ENCOMENDA.QUANTIDADE) AS PRECOTOTAL "
                                + " FROM ENCOMENDA JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE"
                                + " JOIN CONTEUDO_ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=ENCOMENDA.N_ENCOMENDA"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " AND CONTEUDO_ENCOMENDA.TAMANHO_PRODUTO = [PRODUTO-PERSONALIZADO].TAMANHO "
                                + " AND CONTEUDO_ENCOMENDA.COR_PRODUTO = [PRODUTO-PERSONALIZADO].COR"
                                + " AND CONTEUDO_ENCOMENDA.ID_PRODUTO = [PRODUTO-PERSONALIZADO].ID"
                                + " JOIN ESTADO ON ENCOMENDA.ESTADO=ESTADO.ID"
                                + " JOIN UTILIZADOR ON UTILIZADOR.N_FUNCIONARIO=N_GESTOR_VENDA"
                                + " WHERE ENCOMENDA.N_ENCOMENDA=@encomenda"
                                + " GROUP BY ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA;"
                                , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@encomenda", nEncomenda);
            SqlDataReader reader = cmd.ExecuteReader();
            Encomenda Enc = null;
            if (reader.Read())
            {
                Enc = new Encomenda();
                Enc.Cliente = new Cliente();
                Enc.GestorVendas = new Utilizador();
                Enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                Enc.Estado = reader["DESCRIÇAO"].ToString();
                Enc.DataPrevistaEntrega = Convert.ToDateTime(reader["DATA_ENTREGA_PREV"]);
                Enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"]);
                Enc.Desconto = Convert.ToInt32(reader["DESCONTO"]);
                Enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                Enc.Cliente.Nome = reader["CNOME"].ToString();
                Enc.Cliente.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                Enc.Preco = Convert.ToDouble(reader["PRECOTOTAL"].ToString());
            }
            closeSGBDConnection();
            return Enc;
        }

        public ObservableCollection<Encomenda> getEncomendaDB(int nCliente)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME AS CNOME, N_GESTOR_VENDA, SUM([PRODUTO-PERSONALIZADO].PRECO*CONTEUDO_ENCOMENDA.QUANTIDADE) AS PRECOTOTAL "
                                + " FROM ENCOMENDA JOIN CLIENTE ON CLIENTE.NCLIENTE = ENCOMENDA.CLIENTE"
                                + " JOIN CONTEUDO_ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=ENCOMENDA.N_ENCOMENDA"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " AND CONTEUDO_ENCOMENDA.TAMANHO_PRODUTO = [PRODUTO-PERSONALIZADO].TAMANHO "
                                + " AND CONTEUDO_ENCOMENDA.COR_PRODUTO = [PRODUTO-PERSONALIZADO].COR"
                                + " AND CONTEUDO_ENCOMENDA.ID_PRODUTO = [PRODUTO-PERSONALIZADO].ID"
                                + " JOIN ESTADO ON ENCOMENDA.ESTADO=ESTADO.ID"
                                + " JOIN UTILIZADOR ON UTILIZADOR.N_FUNCIONARIO=N_GESTOR_VENDA"
                                + " WHERE CLIENTE.NCLIENTE LIKE @nCli"
                                + " GROUP BY ENCOMENDA.N_ENCOMENDA, DATA_CONFIRMACAO, DATA_ENTREGA_PREV, ESTADO.DESCRIÇAO, DESCONTO, CLIENTE.NCLIENTE, CLIENTE.NOME, N_GESTOR_VENDA;"
                                , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nCli", nCliente);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Encomenda> enc = new ObservableCollection<Encomenda>();
            while (reader.Read())
            {
                Encomenda Enc = new Encomenda();
                Enc.Cliente = new Cliente();
                Enc.GestorVendas = new Utilizador();
                Enc.NEncomenda = Convert.ToInt32(reader["N_ENCOMENDA"].ToString());
                Enc.Estado = reader["DESCRIÇAO"].ToString();
                Enc.DataPrevistaEntrega = Convert.ToDateTime(reader["DATA_ENTREGA_PREV"]);
                Enc.DataConfirmacao = Convert.ToDateTime(reader["DATA_CONFIRMACAO"]);
                Enc.Desconto = Convert.ToInt32(reader["DESCONTO"]);
                Enc.GestorVendas.NFuncionario = Convert.ToInt32(reader["N_GESTOR_VENDA"].ToString());
                Enc.Cliente.Nome = reader["NOME"].ToString();
                Enc.Cliente.NCliente = Convert.ToInt32(reader["NCLIENTE"].ToString());
                Enc.LocalEntrega = reader["LOCALENTREGA"].ToString();
                Enc.Preco = Convert.ToDouble(reader["PRECOTOTAL"].ToString());
                enc.Add(Enc);
            }
            closeSGBDConnection();
            return enc;
        }

        public ObservableCollection<ProdutoPersonalizado> getProdutosFromEncomendaDB(int nEncomenda)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT [PRODUTO-PERSONALIZADO].REFERENCIA as REF, [PRODUTO-BASE].NOME , [PRODUTO-PERSONALIZADO].TAMANHO AS TAM, [PRODUTO-PERSONALIZADO].COR AS COLOR, [PRODUTO-PERSONALIZADO].ID AS IDENT, [PRODUTO-PERSONALIZADO].PRECO AS PRICE, [PRODUTO-PERSONALIZADO].UNIDADES_ARMAZEM AS UA, CONTEUDO_ENCOMENDA.QUANTIDADE AS QT"
                                + " FROM ENCOMENDA"
                                + " JOIN CONTEUDO_ENCOMENDA ON CONTEUDO_ENCOMENDA.N_ENCOMENDA=ENCOMENDA.N_ENCOMENDA"
                                + " JOIN [PRODUTO-PERSONALIZADO] ON CONTEUDO_ENCOMENDA.REFERENCIA_PRODUTO=[PRODUTO-PERSONALIZADO].REFERENCIA "
                                + " AND CONTEUDO_ENCOMENDA.TAMANHO_PRODUTO = [PRODUTO-PERSONALIZADO].TAMANHO "
                                + " AND CONTEUDO_ENCOMENDA.COR_PRODUTO = [PRODUTO-PERSONALIZADO].COR"
                                + " AND CONTEUDO_ENCOMENDA.ID_PRODUTO = [PRODUTO-PERSONALIZADO].ID"
                                + " JOIN [PRODUTO-BASE] ON [PRODUTO-BASE].REFERENCIA=[PRODUTO-PERSONALIZADO].REFERENCIA"
                                + " WHERE ENCOMENDA.N_ENCOMENDA=@encomenda"
                                , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@encomenda", nEncomenda);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> prodList = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REF"].ToString());
                prod.ProdutoBase.Nome = reader["NOME"].ToString();
                prod.Preco = Convert.ToDouble(reader["PRICE"].ToString());
                prod.Tamanho = reader["TAM"].ToString();
                prod.Cor = reader["COLOR"].ToString();
                prod.ID = Convert.ToInt32(reader["IDENT"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UA"].ToString());
                prod.Quantidade = Convert.ToInt32(reader["QT"].ToString());
                prodList.Add(prod);
            }
            closeSGBDConnection();
            return prodList;
        }

        public ObservableCollection<ProdutoPersonalizado> getProdutosPersonalizadosFromProdutoBaseDB(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT [PRODUTO-BASE].REFERENCIA, TAMANHO, COR, ID, N_ETIQUETA, PRECO, UNIDADES_ARMAZEM "
                            + "FROM [PRODUTO-BASE] JOIN [PRODUTO-PERSONALIZADO] ON [PRODUTO-PERSONALIZADO].REFERENCIA=[PRODUTO-BASE].REFERENCIA"
                            + " WHERE [PRODUTO-BASE].REFERENCIA=@REFERENCIA;"
                            , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFERENCIA", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtosPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                produtosPers.Add(prod);
            }
            reader.Close();
            closeSGBDConnection();
            return produtosPers;
        }

        public ObservableCollection<ProdutoPersonalizado> getProdutosPersonalizadosFromDBRef(int refer)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT TAMANHO, COR, ID, PRECO, UNIDADES_ARMAZEM, "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-BASE].NOME as nomeBase, DATA_ALTERACAO, "
                + "INSTRUCOES_PRODUCAO, IVA, PAIS_FABRICO, [PRODUTO-PERSONALIZADO].N_ETIQUETA, NORMAS, "
                + "PAIS_FABRICO, COMPOSICAO FROM[PRODUTO-PERSONALIZADO] JOIN[PRODUTO-BASE] ON "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA JOIN "
                + "ETIQUETA ON ETIQUETA.N_ETIQUETA = [PRODUTO-PERSONALIZADO].N_ETIQUETA WHERE [PRODUTO-PERSONALIZADO].REFERENCIA = @refer; "
                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@refer", refer);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtoPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.ProdutoBase.Nome = reader["nomeBase"].ToString();
                prod.ProdutoBase.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.ProdutoBase.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.ProdutoBase.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.Etiqueta = new Etiqueta();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                prod.Etiqueta.Normas = reader["NORMAS"].ToString();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                produtoPers.Add(prod);
            }
            reader.Close();
            this.closeSGBDConnection();
            return produtoPers;
        }

        public ObservableCollection<ProdutoPersonalizado> getProdutosPersonalizadosFromDBCor(string cor)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT TAMANHO, COR, ID, PRECO, UNIDADES_ARMAZEM, "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-BASE].NOME as nomeBase, DATA_ALTERACAO, "
                + "INSTRUCOES_PRODUCAO, IVA, PAIS_FABRICO, [PRODUTO-PERSONALIZADO].N_ETIQUETA, NORMAS, "
                + "PAIS_FABRICO, COMPOSICAO FROM[PRODUTO-PERSONALIZADO] JOIN[PRODUTO-BASE] ON "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA JOIN "
                + "ETIQUETA ON ETIQUETA.N_ETIQUETA = [PRODUTO-PERSONALIZADO].N_ETIQUETA WHERE COR LIKE @cor; "
                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@cor", "%" + cor + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtoPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.ProdutoBase.Nome = reader["nomeBase"].ToString();
                prod.ProdutoBase.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.ProdutoBase.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.ProdutoBase.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.Etiqueta = new Etiqueta();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                prod.Etiqueta.Normas = reader["NORMAS"].ToString();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                produtoPers.Add(prod);
            }
            reader.Close();
            this.closeSGBDConnection();
            return produtoPers;
        }

        public Boolean checkIfProdutoPersonalizadoExists(ProdutoPersonalizado prod)
        {
            if (!this.verifySGBDConnection())
                return false;

            SqlCommand cmd = new SqlCommand("SELECT * "
                            + " FROM [PRODUTO-PERSONALIZADO] "
                            + " WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO AND [PRODUTO-PERSONALIZADO].COR=@COR AND [PRODUTO-PERSONALIZADO].ID=@ID;"
                            , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFERENCIA", prod.ProdutoBase.Referencia);
            cmd.Parameters.AddWithValue("@TAMANHO", prod.Tamanho);
            cmd.Parameters.AddWithValue("@COR", prod.Cor);
            cmd.Parameters.AddWithValue("@ID", prod.ID);

            if (((int)cmd.ExecuteScalar()) >= 1)
            {
                closeSGBDConnection();
                return true;
            }
            else
            {
                closeSGBDConnection();
                return false;
            }
        }

        public ProdutoPersonalizado getProdutoPersonalizadoFromDB(int referencia, String tamanho, String cor, int id)
        {
            if (!this.verifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("SELECT PRECO, UNIDADES_ARMAZEM, ETIQUETA.N_ETIQUETA, NORMAS, PAIS_FABRICO, COMPOSICAO "
                            + " FROM [PRODUTO-PERSONALIZADO] JOIN [ETIQUETA] ON [PRODUTO-PERSONALIZADO].N_ETIQUETA=[ETIQUETA].N_ETIQUETA"
                            + " WHERE [PRODUTO-PERSONALIZADO].REFERENCIA=@REFERENCIA AND [PRODUTO-PERSONALIZADO].TAMANHO=@TAMANHO AND [PRODUTO-PERSONALIZADO].COR=@COR AND [PRODUTO-PERSONALIZADO].ID=@ID;"
                            , cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REFERENCIA", referencia);
            cmd.Parameters.AddWithValue("@TAMANHO", tamanho);
            cmd.Parameters.AddWithValue("@COR", cor);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            ProdutoPersonalizado prod = new ProdutoPersonalizado();
            prod.ProdutoBase.Referencia = referencia;
            prod.Tamanho = tamanho;
            prod.Cor = cor;
            prod.ID = id;
            prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
            prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
            prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
            prod.Etiqueta.Normas = reader["NORMAS"].ToString();
            prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
            prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
            reader.Close();
            closeSGBDConnection();
            return prod;
        }

        public ObservableCollection<ProdutoBase> getProdutosBaseFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT REFERENCIA, IMAGEM_DESENHO,[PRODUTO-BASE].NOME as nomeProduto, INSTRUCOES_PRODUCAO, "
                            + "DATA_ALTERACAO, IVA, N_FUNCIONARIO, UTILIZADOR.NOME as userName FROM [PRODUTO-BASE] "
                            + "JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoBase> produtosBase = new ObservableCollection<ProdutoBase>();
            while (reader.Read())
            {
                ProdutoBase prod = new ProdutoBase();
                prod.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Nome = reader["nomeProduto"].ToString();
                prod.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.GestorProducao = new Utilizador();
                prod.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                prod.GestorProducao.Nome = reader["userName"].ToString();
                try
                {
                    prod.Pic = (byte[])reader["IMAGEM_DESENHO"];
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("Não foi possivel obter a imagem do desenho do produto base da base de dados.");
                }
                produtosBase.Add(prod);
            }
            reader.Close();
            closeSGBDConnection();
            return produtosBase;
        }

        public ObservableCollection<ProdutoBase> getProdutosBaseFromDBNome(string nome)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT REFERENCIA, IMAGEM_DESENHO, [PRODUTO-BASE].NOME as nomeProduto, INSTRUCOES_PRODUCAO, "
                            + "DATA_ALTERACAO, IVA, N_FUNCIONARIO, UTILIZADOR.NOME as userName FROM [PRODUTO-BASE] "
                            + "JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO WHERE [PRODUTO-BASE].NOME LIKE @nome", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoBase> produtosBase = new ObservableCollection<ProdutoBase>();
            while (reader.Read())
            {
                ProdutoBase prod = new ProdutoBase();
                prod.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Nome = reader["nomeProduto"].ToString();
                prod.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.GestorProducao = new Utilizador();
                prod.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                prod.GestorProducao.Nome = reader["userName"].ToString();
                try
                {
                    prod.Pic = (byte[])reader["IMAGEM_DESENHO"];
                }
                catch (InvalidCastException e)
                {
                    throw new InvalidCastException("Não foi possivel obter a imagem do desenho do produto base da base de dados.");
                }
                produtosBase.Add(prod);
            }
            reader.Close();
            closeSGBDConnection();
            return produtosBase;
        }


        public ProdutoBase getProdutoBaseFromDBWithRef(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT REFERENCIA, IMAGEM_DESENHO, [PRODUTO-BASE].NOME as nomeProduto, INSTRUCOES_PRODUCAO, "
                            + "DATA_ALTERACAO, IVA, N_GESTOR_PROD, UTILIZADOR.NOME as userName FROM [PRODUTO-BASE] "
                            + " JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO WHERE REFERENCIA=@REF;", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@REF", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            ProdutoBase prod = null;
            if (reader.Read())
            {
                prod = new ProdutoBase();
                prod.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Nome = reader["nomeProduto"].ToString();
                prod.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.GestorProducao = new Utilizador();
                prod.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                prod.GestorProducao.Nome = reader["userName"].ToString();
                try
                {
                    prod.Pic = (byte[])reader["IMAGEM_DESENHO"];
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Não foi possivel obter a imagem do desenho do produto base da base de dados. ERRO: " + ex.Message);
                }
            }
            reader.Close();
            closeSGBDConnection();
            return prod;
        }

        public ObservableCollection<ProdutoBase> getProdutoBaseFromDBNGestor(int nGestor)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT REFERENCIA, IMAGEM_DESENHO, [PRODUTO-BASE].NOME as nomeProduto, INSTRUCOES_PRODUCAO, "
                            + "DATA_ALTERACAO, IVA, N_GESTOR_PROD, UTILIZADOR.NOME as userName FROM [PRODUTO-BASE] "
                            + " JOIN UTILIZADOR ON N_GESTOR_PROD=N_FUNCIONARIO WHERE N_GESTOR_PROD = @ges;", cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ges", nGestor);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            ObservableCollection<ProdutoBase> prods = new ObservableCollection<ProdutoBase>();
            while (reader.Read())
            {
                ProdutoBase prod = new ProdutoBase();
                prod.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Nome = reader["nomeProduto"].ToString();
                prod.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.GestorProducao = new Utilizador();
                prod.GestorProducao.NFuncionario = Convert.ToInt32(reader["N_GESTOR_PROD"].ToString());
                prod.GestorProducao.Nome = reader["userName"].ToString();
                try
                {
                    prod.Pic = (byte[])reader["IMAGEM_DESENHO"];
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException("Não foi possivel obter a imagem do desenho do produto base da base de dados. ERRO: " + ex.Message);
                }
                prods.Add(prod);
            }
            reader.Close();
            closeSGBDConnection();
            return prods;
        }

        public Utilizador getUtilizadorFromDB(int user)
        {
            if (!this.verifySGBDConnection())
                return null;
            Utilizador utilizador = new Utilizador();
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, IMAGEM, UTILIZADOR.EMAIL AS EMAILUSER, SALARIO, NOME, TIPO, PASS, UTILIZADOR.TELEFONE AS TELEFONEUSER, HORA_ENTRADA, HORA_SAIDA, IMAGEM, N_FUNCIONARIO_SUPER, ZONAUSER.CODPOSTAL1 AS CODUSER1, ZONAUSER.CODPOSTAL2 AS CODUSER2 ,ZONAUSER.DISTRITO AS DISTRITOUSER, ZONAUSER.LOCALIDADE AS LOCALIDADEUSER, UTILIZADOR.RUA AS RUAUSER, UTILIZADOR.N_PORTA AS PORTAUSER, ZONAFABRICA.CODPOSTAL1 AS CODFABRICA1, ZONAFABRICA.CODPOSTAL2 AS CODFABRICA2, ZONAFABRICA.DISTRITO AS DISTRITOFABRICA, ZONAFABRICA.LOCALIDADE AS LOCALIDADEFABRICA, [FABRICA-FILIAL].RUA AS RUAFABRICA, [FABRICA-FILIAL].N_PORTA AS PORTAFABRICA, [FABRICA-FILIAL].EMAIL AS EMAILFABRICA, [FABRICA-FILIAL].TELEFONE AS TELEFONEFABRICA, [FABRICA-FILIAL].FAX AS FAXFABRICA, N_FILIAL  FROM UTILIZADOR"
                                            + " JOIN ZONA AS ZONAUSER ON UTILIZADOR.CODPOSTAL1 = ZONAUSER.CODPOSTAL1 AND UTILIZADOR.CODPOSTAL2 = ZONAUSER.CODPOSTAL2"
                                            + " JOIN[FABRICA-FILIAL] ON [FABRICA-FILIAL].N_FILIAL = UTILIZADOR.N_FABRICA"
                                            + " JOIN ZONA AS ZONAFABRICA ON [FABRICA-FILIAL].CODPOSTAL1 = ZONAFABRICA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONAFABRICA.CODPOSTAL2"
                                            + " JOIN[UTILIZADOR-TIPOS] ON[UTILIZADOR-TIPOS].UTILIZADOR = UTILIZADOR.N_FUNCIONARIO"
                                            + " JOIN[TIPO-UTILIZADOR] ON[TIPO-UTILIZADOR].ID =[UTILIZADOR-TIPOS].ID_TIPO"
                                            + " WHERE N_FUNCIONARIO = @USER;"
                                            , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                utilizador.Filial = new filial();
                utilizador.Filial.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
                utilizador.Filial.Email = reader["EMAILFABRICA"].ToString();
                utilizador.Filial.Fax = reader["FAXFABRICA"].ToString();
                utilizador.Filial.Telefone = reader["TELEFONEFABRICA"].ToString();
                utilizador.Filial.Localizacao = new Localizacao();
                utilizador.Filial.Localizacao.CodigoPostal = reader["CODFABRICA1"].ToString() + "-" + reader["CODFABRICA2"].ToString();
                utilizador.Filial.Localizacao.Distrito = reader["DISTRITOFABRICA"].ToString();
                utilizador.Filial.Localizacao.Localidade = reader["LOCALIDADEFABRICA"].ToString();
                utilizador.Filial.Localizacao.Rua1 = reader["RUAFABRICA"].ToString();
                utilizador.Filial.Localizacao.Porta = Convert.ToInt32(reader["PORTAFABRICA"].ToString());
                utilizador.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                utilizador.Email = reader["EMAILUSER"].ToString();
                utilizador.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                utilizador.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());
                utilizador.Nome = reader["NOME"].ToString();
                utilizador.Password = reader["PASS"].ToString();
                utilizador.Salario = Convert.ToDouble(reader["SALARIO"].ToString());
                utilizador.Telemovel = reader["TELEFONEUSER"].ToString();
                utilizador.TiposUser.Add(reader["TIPO"].ToString());
                utilizador.Localizacao = new Localizacao();
                utilizador.Localizacao.CodigoPostal = reader["CODUSER1"].ToString() + "-" + reader["CODUSER2"].ToString();
                utilizador.Localizacao.Distrito = reader["DISTRITOUSER"].ToString();
                utilizador.Localizacao.Localidade = reader["LOCALIDADEUSER"].ToString();
                utilizador.Localizacao.Porta = Convert.ToInt32(reader["PORTAUSER"].ToString());
                utilizador.Localizacao.Rua1 = reader["RUAUSER"].ToString();
                utilizador.Supervisor = new Utilizador();
                if (!string.IsNullOrEmpty(reader["N_FUNCIONARIO_SUPER"].ToString()))
                {
                    utilizador.Supervisor.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO_SUPER"].ToString());
                }
                byte[] img = null;
                try
                {
                    img = (byte[])reader["IMAGEM"];
                }
                catch (InvalidCastException e)
                {

                }
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    utilizador.Imagem = Image.FromStream(ms);
                }
                Console.WriteLine(utilizador.TiposUser);
            }
            reader.Close();
            this.closeSGBDConnection();
            return utilizador;
        }


        public String entregarEncomenda(int nEncomenda)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("dbo.entregarEncomenda", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            // set up the parameters
            cmd.Parameters.Add("@nEncomenda", SqlDbType.Int);
            cmd.Parameters.Add("@out", SqlDbType.VarChar, 70).Direction = ParameterDirection.Output;

            // set parameter values
            cmd.Parameters["@nEncomenda"].Value = nEncomenda;

            // execute stored procedure
            cmd.ExecuteNonQuery();

            // read output value from @NewId
            String strResult = cmd.Parameters["@out"].Value.ToString();
            return strResult;
        }

        public void registarProdutoBase(ProdutoBase produtoBase)
        {

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO [PRODUTO-BASE] (NOME, IVA, DATA_ALTERACAO, INSTRUCOES_PRODUCAO, N_GESTOR_PROD, IMAGEM_DESENHO) "
                + "values (@nome_Produto, @iva, @Data_alteracao, @instr, @nGestor, @imagem ) ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome_Produto", produtoBase.Nome);
            cmd.Parameters.AddWithValue("@iva", produtoBase.IVA1);
            cmd.Parameters.AddWithValue("@Data_alteracao", DateTime.Today);
            cmd.Parameters.AddWithValue("@instr", produtoBase.InstrProd);
            cmd.Parameters.AddWithValue("@nGestor", produtoBase.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@imagem", produtoBase.Pic);
            /* IDataParameter par = cmd.CreateParameter();
             par.ParameterName = "@imagem";
             par.DbType = DbType.Binary;
             par.Value = produtoBase.Pic;
             cmd.Parameters.Add(par);*/
            cmd.Connection = Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o Desenho produto na base de dados. \n MENSAGEM DE ERRO: \n" + ex.Message);
            }
            finally
            {
                closeSGBDConnection();
            }
        }

        public String adicionarMaterial(int referencia, double quantidade)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("dbo.adicionarMaterial", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            // set up the parameters
            cmd.Parameters.Add("@ref", SqlDbType.Int);
            cmd.Parameters.Add("@qtd", SqlDbType.Decimal);
            cmd.Parameters.Add("@out", SqlDbType.VarChar, 70).Direction = ParameterDirection.Output;

            // set parameter values
            cmd.Parameters["@ref"].Value = referencia;
            cmd.Parameters["@qtd"].Value = quantidade;

            // execute stored procedure
            cmd.ExecuteNonQuery();

            // read output value from @NewId
            String strResult = cmd.Parameters["@out"].Value.ToString();
            return strResult;
        }

        public void inserirMaterial(MaterialTextil mat)
        {

            if (!this.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = this.Cn;
            cmd.Parameters.Clear();
            Console.WriteLine(mat.Cor);
            cmd.Parameters.AddWithValue("@COR", mat.Cor);
            cmd.Parameters.AddWithValue("@FORNECEDORREF", mat.ReferenciaFornecedor);
            cmd.Parameters.AddWithValue("@FORNECEDOR", mat.Fornecedor.NIF_Fornecedor);
            cmd.Parameters.AddWithValue("@DESIGNACAO", mat.Designacao);
            cmd.CommandText = "INSERT INTO MATERIAIS_TÊXTEIS (REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO) " +
            "VALUES (@FORNECEDORREF, @FORNECEDOR, @COR, @DESIGNACAO);";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }

            if (!this.verifySGBDConnection())
                return;
            SqlCommand getRef = new SqlCommand();
            getRef.Connection = this.Cn;
            getRef.Parameters.AddWithValue("REFERENCIA_FORN", mat.ReferenciaFornecedor);
            getRef.Parameters.AddWithValue("NIF_FORNECEDOR", mat.Fornecedor.NIF_Fornecedor);
            getRef.CommandText = "SELECT REFERENCIA_FABRICA FROM MATERIAIS_TÊXTEIS WHERE REFERENCIA_FORN=@REFERENCIA_FORN AND NIF_FORNECEDOR=@NIF_FORNECEDOR";
            SqlDataReader reader = getRef.ExecuteReader();
            while (reader.Read())
                mat.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
            this.closeSGBDConnection();

            if (!this.verifySGBDConnection())
                return;



            if (mat.GetType() == typeof(Pano))
            {
                Pano pano = (Pano)mat;
                SqlCommand panocmd = new SqlCommand();
                panocmd.Connection = this.Cn;
                panocmd.Parameters.Clear();
                panocmd.Parameters.AddWithValue("@REFERENCIA", pano.Referencia);
                panocmd.Parameters.AddWithValue("@GRAMAGEM", pano.Gramagem);
                panocmd.Parameters.AddWithValue("@PRECO", pano.PrecoMetroQuadrado);
                panocmd.Parameters.AddWithValue("@AREAARMAZEM", pano.AreaArmazem);
                panocmd.Parameters.AddWithValue("@TIPO", pano.Tipo);
                panocmd.CommandText = "INSERT INTO PANO (REFERENCIA_FABRICA, TIPO, GRAMAGEM, AREA_ARMAZEM, PRECO_POR_M2) " +
"VALUES (@REFERENCIA, @TIPO, @GRAMAGEM, @AREAARMAZEM, @PRECO);";
                try
                {
                    panocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            else if (mat.GetType() == typeof(Linha))
            {
                Linha linha = (Linha)mat;
                SqlCommand linhacmd = new SqlCommand();
                linhacmd.Connection = this.Cn;
                linhacmd.Parameters.Clear();
                linhacmd.Parameters.AddWithValue("@REFERENCIA", linha.Referencia);
                linhacmd.Parameters.AddWithValue("@GROSSURA", linha.Grossura);
                linhacmd.Parameters.AddWithValue("@PRECO", linha.Preco100Metros);
                linhacmd.Parameters.AddWithValue("@COMPRIMENTO", linha.ComprimentoStock);
                linhacmd.CommandText = "INSERT INTO LINHA (REFERENCIA_FABRICA, GROSSURA, PRECO_CEM_METROS, COMPRIMENTO_ARMAZEM) " +
                "VALUES (@REFERENCIA, @GROSSURA, @PRECO, @COMPRIMENTO);";
                try
                {
                    linhacmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            else
            {
                AcessoriosCostura acessorio = (AcessoriosCostura)mat;
                SqlCommand acessoriocmd = new SqlCommand();
                Console.WriteLine(acessorio.PrecoUnidade);
                Console.WriteLine(acessorio.Referencia);
                acessoriocmd.Connection = this.Cn;
                acessoriocmd.Parameters.Clear();
                acessoriocmd.Parameters.AddWithValue("@REFERENCIA", acessorio.Referencia);
                acessoriocmd.Parameters.AddWithValue("@PRECO_UNIDADE", acessorio.PrecoUnidade);
                acessoriocmd.CommandText = "INSERT INTO ACESSORIO (REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE) " +
"VALUES (@REFERENCIA, 0, @PRECO_UNIDADE);";
                try
                {
                    acessoriocmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    this.closeSGBDConnection();
                    throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }

                if (mat.GetType() == typeof(Mola))
                {
                    Mola mola = (Mola)mat;
                    SqlCommand molacmd = new SqlCommand();
                    molacmd.Connection = this.Cn;
                    molacmd.Parameters.Clear();
                    molacmd.Parameters.AddWithValue("@REFERENCIA", mola.Referencia);
                    molacmd.Parameters.AddWithValue("@DIAMETRO", mola.Diametro);
                    molacmd.CommandText = "INSERT INTO MOLA (REFERENCIA_FABRICA, DIAMETRO) " +
    "VALUES (@REFERENCIA, @DIAMETRO);";
                    try
                    {
                        molacmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
                else if (mat.GetType() == typeof(Botao))
                {
                    Botao botao = (Botao)mat;
                    SqlCommand botaocmd = new SqlCommand();
                    botaocmd.Connection = this.Cn;
                    botaocmd.Parameters.Clear();
                    botaocmd.Parameters.AddWithValue("@REFERENCIA", botao.Referencia);
                    botaocmd.Parameters.AddWithValue("@DIAMETRO", botao.Diametro);
                    botaocmd.CommandText = "INSERT INTO BOTAO (REFERENCIA_FABRICA, DIAMETRO) " +
    "VALUES (@REFERENCIA, @DIAMETRO);";
                    try
                    {
                        botaocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
                else if (mat.GetType() == typeof(FitaVelcro))
                {
                    FitaVelcro fita = (FitaVelcro)mat;
                    SqlCommand fitacmd = new SqlCommand();
                    fitacmd.Connection = this.Cn;
                    fitacmd.Parameters.Clear();
                    fitacmd.Parameters.AddWithValue("@REFERENCIA", fita.Referencia);
                    fitacmd.Parameters.AddWithValue("@LARGURA", fita.Largura);
                    fitacmd.Parameters.AddWithValue("@COMPRIMENTO", fita.Comprimento);
                    fitacmd.CommandText = "INSERT INTO [FITA-VELCRO] (REFERENCIA_FABRICA, COMPRIMENTO, LARGURA) " +
    "VALUES (@REFERENCIA, @COMPRIMENTO, @LARGURA);";
                    try
                    {
                        fitacmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }

                }
                else if (mat.GetType() == typeof(Fecho))
                {
                    Fecho fecho = (Fecho)mat;
                    SqlCommand fechocmd = new SqlCommand();
                    fechocmd.Connection = this.Cn;
                    fechocmd.Parameters.Clear();
                    fechocmd.Parameters.AddWithValue("@REFERENCIA", fecho.Referencia);
                    fechocmd.Parameters.AddWithValue("@TAMANHO_DENTE", fecho.TamanhoDente);
                    fechocmd.Parameters.AddWithValue("@COMPRIMENTO", fecho.Comprimento);
                    fechocmd.CommandText = "INSERT INTO FECHO (REFERENCIA_FABRICA, TAMANHO_DENTE ,COMPRIMENTO) " +
"VALUES (@REFERENCIA, @TAMANHO_DENTE, @COMPRIMENTO);";
                    try
                    {
                        fechocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }

                }
                else if (mat.GetType() == typeof(Elastico))
                {
                    Elastico elastico = (Elastico)mat;
                    SqlCommand elasticocmd = new SqlCommand();
                    elasticocmd.Connection = this.Cn;
                    elasticocmd.Parameters.Clear();
                    elasticocmd.Parameters.AddWithValue("@REFERENCIA", elastico.Referencia);
                    elasticocmd.Parameters.AddWithValue("@LARGURA", elastico.Largura);
                    elasticocmd.Parameters.AddWithValue("@COMPRIMENTO", elastico.Comprimento);
                    elasticocmd.CommandText = "INSERT INTO ELASTICO (REFERENCIA_FABRICA, COMPRIMENTO, LARGURA) " +
    "VALUES (@REFERENCIA, @COMPRIMENTO, @LARGURA);";
                    try
                    {
                        elasticocmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        this.closeSGBDConnection();
                        throw new Exception("Falha ao criar material na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                    }
                }
            }
        }

        public void EnviarEncomenda(Encomenda encomenda, List<ProdutoPersonalizado> listaProdutos)
        {
            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.Parameters.Clear();
            cmd.CommandText = "Insert into ENCOMENDA(ESTADO, DESCONTO, DATA_CONFIRMACAO, DATA_ENTREGA_PREV ,N_GESTOR_VENDA, CLIENTE, LOCALENTREGA) values(1, @DESCONTO, @DATEC, @DATEP ,@GESTOR, @CLIENTE, @LOCAL);";
            cmd.Parameters.AddWithValue("@DATEC", encomenda.DataConfirmacao);
            cmd.Parameters.AddWithValue("@DESCONTO", encomenda.Desconto);
            cmd.Parameters.AddWithValue("@GESTOR", encomenda.GestorVendas.NFuncionario);
            cmd.Parameters.AddWithValue("@CLIENTE", encomenda.Cliente.NCliente);
            cmd.Parameters.AddWithValue("@LOCAL", 1);
            cmd.Parameters.AddWithValue("@DATEP", encomenda.DataPrevistaEntrega);
            cmd.Connection = this.Cn;
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
                this.closeSGBDConnection();
            }


            int nencomenda = this.getLastIdentity("ENCOMENDA");

            for (int i = 0; i < listaProdutos.Count; i++)
            {
                this.verifySGBDConnection();
                SqlCommand values = new SqlCommand();
                int referencia = (int)(listaProdutos.ElementAt(i)).ProdutoBase.Referencia;
                String cor = (listaProdutos.ElementAt(i)).Cor;
                String tamanho = (listaProdutos.ElementAt(i)).Tamanho;
                int id = (int)(listaProdutos.ElementAt(i)).ID;
                int quantidade = (int)(listaProdutos.ElementAt(i)).Quantidade;
                values.Parameters.Clear();
                values.Parameters.AddWithValue("@ENCOMENDA", nencomenda);
                values.Parameters.AddWithValue("@REFERENCIA", referencia);
                values.Parameters.AddWithValue("@TAMANHO", tamanho);
                values.Parameters.AddWithValue("@COR", cor);
                values.Parameters.AddWithValue("@ID", id);
                values.Parameters.AddWithValue("@QUANTIDADE", quantidade);
                values.CommandText = "INSERT INTO [CONTEUDO_ENCOMENDA](N_ENCOMENDA, REFERENCIA_PRODUTO, TAMANHO_PRODUTO, COR_PRODUTO, ID_PRODUTO,QUANTIDADE) VALUES(@ENCOMENDA, @REFERENCIA, @TAMANHO, @COR, @ID,@QUANTIDADE)";
                values.Connection = this.Cn;
                try
                {
                    values.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao associar produtos á encomenda na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
                finally
                {
                    this.closeSGBDConnection();
                }
            }

        }


        public String getMaterialType(int referencia)
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("select dbo.getTipoMaterial(@referencia)", cn);
            cmd.Parameters.AddWithValue("@referencia", referencia);
            String strResult = cmd.ExecuteScalar().ToString();
            closeSGBDConnection();
            return strResult;
        }

        public Etiqueta getEtiqueta(int n)
        {
            Etiqueta et = new Etiqueta();
            if (!this.verifySGBDConnection())
                return et;
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.getEtiqueta(@nEtiqueta)", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nEtiqueta", n);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            et.Numero = n;
            et.Normas = reader["NORMAS"].ToString();
            et.PaisFabrico = reader["PAIS_FABRICO"].ToString();
            et.Composicao = reader["COMPOSICAO"].ToString();
            reader.Close();
            this.closeSGBDConnection();
            return et;
        }

        public int getEtiqueta(String normas, String composiçao, String pais)
        {

            if (!this.verifySGBDConnection())
                return 0;
            SqlCommand cmd = new SqlCommand("SELECT dbo.getEtiquetaNumero(@normas, @comp, @pais)", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@normas", normas);
            cmd.Parameters.AddWithValue("@comp", composiçao);
            cmd.Parameters.AddWithValue("@pais", pais);
            int n = (int)cmd.ExecuteScalar();
            this.closeSGBDConnection();
            return n;
        }

        public void insertEtiqueta(String normas, String composiçao, String pais)
        {
            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand("INSERT INTO ETIQUETA (NORMAS, PAIS_FABRICO, COMPOSICAO) VALUES "
                + "(@normas, @pais, @comp) ", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@normas", normas);
            cmd.Parameters.AddWithValue("@comp", composiçao);
            cmd.Parameters.AddWithValue("@pais", pais);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.closeSGBDConnection();
                throw new Exception("Falha ao criar Etiqueta na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            this.closeSGBDConnection();
        }

        public int getLastIdentity(String tableName)
        {
            if (!this.verifySGBDConnection())
                return 0;
            SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT(@table) ", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@table", tableName);
            int id = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            this.closeSGBDConnection();
            return id;
        }

        public Pano getPano(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            Pano pano = new Pano();
            SqlCommand cmd = new SqlCommand("SELECT PANO.REFERENCIA_FABRICA AS refFabr, TIPO, GRAMAGEM, "
                + "AREA_ARMAZEM, PRECO_POR_M2, REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM PANO "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = PANO.REFERENCIA_FABRICA "
                + "WHERE PANO.REFERENCIA_FABRICA = @ref"
                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Pano p = new Pano();
            while (reader.Read())
            {
                pano.Referencia = referencia;
                pano.Cor = reader["COR"].ToString();
                pano.Fornecedor = new Fornecedor();
                pano.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                pano.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                pano.Designacao = reader["DESIGNACAO"].ToString();
                pano.Gramagem = Convert.ToInt32(reader["GRAMAGEM"].ToString());
                pano.Tipo = reader["TIPO"].ToString();
                pano.AreaArmazem = Convert.ToDouble(reader["AREA_ARMAZEM"].ToString());
                pano.PrecoMetroQuadrado = Convert.ToDouble(reader["PRECO_POR_M2"].ToString());
                //quantidade selecionado

            }
            reader.Close();
            this.closeSGBDConnection();
            return pano;
        }


        public Linha getLinha(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT LINHA.REFERENCIA_FABRICA, GROSSURA, COMPRIMENTO_ARMAZEM, "
                + "PRECO_CEM_METROS, REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM LINHA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = LINHA.REFERENCIA_FABRICA WHERE LINHA.REFERENCIA_FABRICA = @ref", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Linha l = new Linha();
            while (reader.Read())
            {
                l.Referencia = referencia;
                l.Grossura = Convert.ToDouble(reader["GROSSURA"].ToString());
                l.ComprimentoStock = Convert.ToDouble(reader["COMPRIMENTO_ARMAZEM"].ToString());
                l.Preco100Metros = Convert.ToDouble(reader["PRECO_CEM_METROS"].ToString());
                l.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                l.Cor = reader["COR"].ToString();
                l.Designacao = reader["DESIGNACAO"].ToString();
                l.Fornecedor = new Fornecedor();
                l.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
            }
            reader.Close();
            this.closeSGBDConnection();
            return l;
        }

        public Fecho getFecho(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT FECHO.REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE, "
                + "COMPRIMENTO, TAMANHO_DENTE, REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM FECHO "
                + "JOIN ACESSORIO ON FECHO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + " WHERE FECHO.REFERENCIA_FABRICA = @ref", this.Cn);

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            Fecho f = new Fecho();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                f.Referencia = referencia;
                f.Cor = reader["COR"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                f.Fornecedor = new Fornecedor();
                f.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                f.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
                f.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                f.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                f.TamanhoDente = Convert.ToDouble(reader["TAMANHO_DENTE"].ToString());

            }
            reader.Close();
            this.closeSGBDConnection();
            return f;
        }

        public Mola getMola(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT MOLA.REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE, DIAMETRO, "
                    + "REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM MOLA "
                    + "JOIN ACESSORIO ON MOLA.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                    + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA  "
                    + "WHERE MOLA.REFERENCIA_FABRICA = @ref", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Mola m = new Mola();
            while (reader.Read())
            {
                m.Referencia = referencia;
                m.Cor = reader["COR"].ToString();
                m.Designacao = reader["DESIGNACAO"].ToString();
                m.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                m.Fornecedor = new Fornecedor();
                m.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                m.Diametro = Convert.ToDouble(reader["DIAMETRO"].ToString());
                m.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                m.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
            }
            reader.Close();
            this.closeSGBDConnection();
            return m;
        }

        public Botao getBotao(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT BOTAO.REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE, DIAMETRO, "
                + "REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM BOTAO JOIN ACESSORIO "
                + "ON BOTAO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                    + "WHERE BOTAO.REFERENCIA_FABRICA = @ref", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Botao b = new Botao();
            while (reader.Read())
            {
                b.Referencia = referencia;
                b.Cor = reader["COR"].ToString();
                b.Designacao = reader["DESIGNACAO"].ToString();
                b.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                b.Fornecedor = new Fornecedor();
                b.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                b.Diametro = Convert.ToDouble(reader["DIAMETRO"].ToString());
                b.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                b.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());

            }
            reader.Close();
            this.closeSGBDConnection();
            return b;
        }


        public Elastico getElastico(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT ELASTICO.REFERENCIA_FABRICA as ref, QUANTIDADE_ARMAZEM, "
                + "COR, PRECO_UNIDADE, LARGURA, REFERENCIA_FORN, COR, DESIGNACAO, COMPRIMENTO, NIF_FORNECEDOR FROM ELASTICO "
                + "JOIN ACESSORIO ON ELASTICO.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "WHERE ELASTICO.REFERENCIA_FABRICA = @ref", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            Elastico el = new Elastico();
            while (reader.Read())
            {
                el.Referencia = referencia;
                el.Cor = reader["COR"].ToString();
                el.Designacao = reader["DESIGNACAO"].ToString();
                el.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                el.Fornecedor = new Fornecedor();
                el.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                el.Largura = Convert.ToDouble(reader["LARGURA"].ToString());
                el.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                el.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                el.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());
            }
            reader.Close();
            this.closeSGBDConnection();
            return el;
        }

        public FitaVelcro getFitaVelcro(int referencia)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT [FITA-VELCRO].REFERENCIA_FABRICA, QUANTIDADE_ARMAZEM, PRECO_UNIDADE, "
                + "LARGURA, COMPRIMENTO, REFERENCIA_FORN, NIF_FORNECEDOR, COR, DESIGNACAO FROM[FITA-VELCRO] "
                + "JOIN ACESSORIO ON[FITA-VELCRO].REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA "
                + "JOIN MATERIAIS_TÊXTEIS ON MATERIAIS_TÊXTEIS.REFERENCIA_FABRICA = ACESSORIO.REFERENCIA_FABRICA  "
                    + "WHERE [FITA-VELCRO].REFERENCIA_FABRICA = @ref", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            SqlDataReader reader = cmd.ExecuteReader();
            FitaVelcro f = new FitaVelcro();
            while (reader.Read())
            {
                f.Referencia = referencia;
                f.Referencia = referencia;
                f.Cor = reader["COR"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                f.Fornecedor = new Fornecedor();
                f.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                f.Largura = Convert.ToDouble(reader["LARGURA"].ToString());
                f.Comprimento = Convert.ToDouble(reader["COMPRIMENTO"].ToString());
                f.QuantidadeArmazem = Convert.ToInt32(reader["QUANTIDADE_ARMAZEM"].ToString());
                f.PrecoUnidade = Convert.ToDouble(reader["PRECO_UNIDADE"].ToString());

            }
            reader.Close();
            this.closeSGBDConnection();
            return f;
        }

        // ----------------Fabrica filial----------

        public ObservableCollection<filial> getFiliaisFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;

            ObservableCollection<filial> FiliaisTexteis = new ObservableCollection<filial>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [FABRICA-FILIAL] JOIN ZONA ON "
                + "([FABRICA-FILIAL].CODPOSTAL1 = ZONA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONA.CODPOSTAL2) "
                + "JOIN UTILIZADOR ON CHEFE = N_FUNCIONARIO", this.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filial f = new filial();
                f.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
                f.Email = reader["EMAIL"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Chefe = new Utilizador();
                f.Chefe.NFuncionario = Convert.ToInt32(reader["CHEFE"].ToString());
                f.Chefe.Nome = reader["NOME"].ToString();
                FiliaisTexteis.Add(f);
            }
            reader.Close();
            this.closeSGBDConnection();
            return FiliaisTexteis;
        }

        public ObservableCollection<filial> getFiliaisInDBnFil(int nFilial)
        {
            if (!this.verifySGBDConnection())
                return null;

            ObservableCollection<filial> FiliaisTexteis = new ObservableCollection<filial>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [FABRICA-FILIAL] JOIN ZONA ON "
                + "([FABRICA-FILIAL].CODPOSTAL1 = ZONA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONA.CODPOSTAL2) "
                + "JOIN UTILIZADOR ON CHEFE = N_FUNCIONARIO WHERE N_FILIAL = @nFilial", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nFilial", nFilial);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filial f = new filial();
                f.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
                f.Email = reader["EMAIL"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Chefe = new Utilizador();
                f.Chefe.NFuncionario = Convert.ToInt32(reader["CHEFE"].ToString());
                f.Chefe.Nome = reader["NOME"].ToString();
                FiliaisTexteis.Add(f);
            }
            reader.Close();
            this.closeSGBDConnection();
            return FiliaisTexteis;
        }

        public ObservableCollection<filial> getFiliaisInDBEmail(string email)
        {
            if (!this.verifySGBDConnection())
                return null;

            ObservableCollection<filial> FiliaisTexteis = new ObservableCollection<filial>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [FABRICA-FILIAL] JOIN ZONA ON "
                + "([FABRICA-FILIAL].CODPOSTAL1 = ZONA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONA.CODPOSTAL2) "
                + "JOIN UTILIZADOR ON CHEFE = N_FUNCIONARIO WHERE  [FABRICA-FILIAL].EMAIL LIKE @email", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@email", "%" + email + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filial f = new filial();
                f.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
                f.Email = reader["EMAIL"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Chefe = new Utilizador();
                f.Chefe.NFuncionario = Convert.ToInt32(reader["CHEFE"].ToString());
                f.Chefe.Nome = reader["NOME"].ToString();
                FiliaisTexteis.Add(f);
            }
            reader.Close();
            this.closeSGBDConnection();
            return FiliaisTexteis;
        }

        public ObservableCollection<filial> getFiliaisInDBPhone(string tel)
        {
            if (!this.verifySGBDConnection())
                return null;

            ObservableCollection<filial> FiliaisTexteis = new ObservableCollection<filial>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM [FABRICA-FILIAL] JOIN ZONA ON "
                + "([FABRICA-FILIAL].CODPOSTAL1 = ZONA.CODPOSTAL1 AND [FABRICA-FILIAL].CODPOSTAL2 = ZONA.CODPOSTAL2) "
                + "JOIN UTILIZADOR ON CHEFE = N_FUNCIONARIO WHERE [FABRICA-FILIAL].TELEFONE LIKE @tel", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@tel", "%" + tel + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filial f = new filial();
                f.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
                f.Email = reader["EMAIL"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Chefe = new Utilizador();
                f.Chefe.NFuncionario = Convert.ToInt32(reader["CHEFE"].ToString());
                f.Chefe.Nome = reader["NOME"].ToString();
                FiliaisTexteis.Add(f);
            }
            reader.Close();
            this.closeSGBDConnection();
            return FiliaisTexteis;
        }

        public void EnviarFilial(filial f)
        {

            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO [FABRICA-FILIAL] (EMAIL, TELEFONE, FAX, CODPOSTAL1, CODPOSTAL2, RUA, N_PORTA, CHEFE) VALUES "
                + "(@Email, @TELEFONE, @Fax, @COD_POSTAL1, @COD_POSTAL2, @RUA, @N_PORTA, @chefe)";
            cmd.Parameters.Clear();

            if (string.IsNullOrEmpty(f.Fax))
            {
                cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fax", f.Fax);
            }
            cmd.Parameters.AddWithValue("@Email", f.Email);
            cmd.Parameters.AddWithValue("@TELEFONE", f.Telefone);
            cmd.Parameters.AddWithValue("@COD_POSTAL1", f.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@COD_POSTAL2", f.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@RUA", f.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@N_PORTA", f.Localizacao.Porta);
            cmd.Parameters.AddWithValue("@chefe", f.Chefe.NFuncionario);
            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao adicionar Fábrica Filial na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {

                this.closeSGBDConnection();
            }

        }

        public int getNfilialFromDB(string email, string telefone)
        {
            if (!this.verifySGBDConnection())
                return 0;
            SqlCommand cmd = new SqlCommand("SELECT dbo.nFilial(@Email, @TELEFONE)", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@TELEFONE", telefone);
            int n = Convert.ToInt32(cmd.ExecuteScalar());
            this.closeSGBDConnection();
            return n;
        }

        public void AtualizarFilial(filial f)
        {
            int rows = 0;
            if (!this.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE [FABRICA-FILIAL] " + "SET EMAIL = @EMAIL, "
                + " TELEFONE = @TELEFONE, " + " CODPOSTAL1 = @CODPOSTAL1, " + " CODPOSTAL2 = @CODPOSTAL2, " +
                " RUA = @RUA, " + " N_PORTA = @N_PORTA, FAX = @Fax, "
                + "CHEFE = @chefe" + " WHERE N_FILIAL = @N_FILIAL";
            cmd.Parameters.Clear();
            if (string.IsNullOrEmpty(f.Fax))
            {
                cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fax", f.Fax);
            }
            cmd.Parameters.AddWithValue("@N_FILIAL", f.NFilial);
            cmd.Parameters.AddWithValue("@EMAIL", f.Email);
            cmd.Parameters.AddWithValue("@TELEFONE", f.Telefone);
            cmd.Parameters.AddWithValue("@CODPOSTAL1", f.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@CODPOSTAL2", f.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@RUA", f.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@N_PORTA", f.Localizacao.Porta);
            cmd.Parameters.AddWithValue("@chefe", f.Chefe.NFuncionario);
            cmd.Connection = this.Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar a Filial na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    Xceed.Wpf.Toolkit.MessageBox.Show("A atualização da Fábrica Filial foi submetida com sucesso!", "Atualização Bem Sucedida", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                   if (Xceed.Wpf.Toolkit.MessageBox.Show("Não foi possivel atualizar a informação da Fábrica Filial. Pretende tentar novamente?", "Erro", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    this.AtualizarFilial(f);
                this.closeSGBDConnection();
            }
        }

        public ObservableCollection<Utilizador> getUtilizadoresFromFilial(int nFilial)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZADOR JOIN ZONA ON (ZONA.CODPOSTAL1 = UTILIZADOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = UTILIZADOR.CODPOSTAL2) "
                + " WHERE N_FABRICA = @nFilial", this.Cn);
            ObservableCollection<Utilizador> users = new ObservableCollection<Utilizador>();
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nFilial", nFilial);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Utilizador u = new Utilizador();
                u.Localizacao = new Localizacao();
                u.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                u.Nome = reader["NOME"].ToString();
                u.Email = reader["EMAIL"].ToString();
                u.Telemovel = reader["TELEFONE"].ToString();
                u.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                u.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());
                u.Localizacao.CodigoPostal = reader["CODPOSTAL1"].ToString() + "-" + reader["CODPOSTAL2"].ToString();
                u.Localizacao.Rua1 = reader["RUA"].ToString();
                u.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                u.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                u.Filial = new filial();
                u.Filial.NFilial = Convert.ToInt32(reader["N_FABRICA"].ToString());
                u.Supervisor = new Utilizador();
                u.Supervisor.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO_SUPER"].ToString());
                users.Add(u);
            }
            reader.Close();

            foreach (Utilizador u in users)
            {
                u.TiposUser = this.getUserTypesFromDB(u.NFuncionario);
            }
            this.closeSGBDConnection();
            return users;
        }

        public Utilizador getChefeFilialFromDB(int nChefe)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, UTILIZADOR.EMAIL as userMail, SALARIO, NOME, UTILIZADOR.TELEFONE as userPhone, "
                + " HORA_ENTRADA, HORA_SAIDA, UTILIZADOR.CODPOSTAL1 as codP1, UTILIZADOR.CODPOSTAL2 as codP2, UTILIZADOR.RUA as userRua, UTILIZADOR.N_PORTA as userPorta, "
                + " ZONA.LOCALIDADE as loc, N_FUNCIONARIO_SUPER FROM UTILIZADOR JOIN[FABRICA-FILIAL]  ON CHEFE = N_FUNCIONARIO "
                + " JOIN ZONA ON(ZONA.CODPOSTAL1 = UTILIZADOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = UTILIZADOR.CODPOSTAL2) "
                + " WHERE CHEFE = 1", this.Cn);
            Utilizador u = new Utilizador();
            u.Localizacao = new Localizacao();
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@chefe", nChefe);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                u.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                u.Nome = reader["NOME"].ToString();
                u.Email = reader["userMail"].ToString();
                u.Telemovel = reader["userPhone"].ToString();
                u.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                u.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());
                u.Localizacao.CodigoPostal = reader["codP1"].ToString() + "-" + reader["codP2"].ToString();
                u.Localizacao.Rua1 = reader["userRua"].ToString();
                u.Localizacao.Localidade = reader["loc"].ToString();
                u.Localizacao.Porta = Convert.ToInt32(reader["userPorta"].ToString());
            }
            reader.Close();
            this.closeSGBDConnection();
            return u;
        }

        public ObservableCollection<Fornecedor> getFornecedoresFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT NOME, NIF, EMAIL, TELEFONE, FAX, DESIGNACAO, "
                + " FORNECEDOR.CODPOSTAL1 as cod1, FORNECEDOR.CODPOSTAL2 as cod2, RUA, DISTRITO, LOCALIDADE, "
                + " N_PORTA FROM FORNECEDOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = FORNECEDOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = FORNECEDOR.CODPOSTAL2)", Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
            while (reader.Read())
            {
                Fornecedor f = new Fornecedor();
                f.NIF_Fornecedor = reader["NIF"].ToString();
                f.Email = reader["EMAIL"].ToString();
                f.Nome = reader["NOME"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                fornecedores.Add(f);
            }
            closeSGBDConnection();
            return fornecedores;
        }

        public ObservableCollection<Fornecedor> searchFornecedoresInDBDes(string des)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT NOME, NIF, EMAIL, TELEFONE, FAX, DESIGNACAO, "
                + " FORNECEDOR.CODPOSTAL1 as cod1, FORNECEDOR.CODPOSTAL2 as cod2, RUA, DISTRITO, LOCALIDADE, "
                + " N_PORTA FROM FORNECEDOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = FORNECEDOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = FORNECEDOR.CODPOSTAL2)"
                + " WHERE DESIGNACAO LIKE  @des;", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@des", "%" + des + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
            while (reader.Read())
            {
                Fornecedor f = new Fornecedor();
                f.NIF_Fornecedor = reader["NIF"].ToString();
                f.Email = reader["EMAIL"].ToString();
                f.Nome = reader["NOME"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                fornecedores.Add(f);
            }
            this.closeSGBDConnection();
            return fornecedores;
        }

        public ObservableCollection<Fornecedor> searchFornecedoresInDBTel(string tel)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT NOME, NIF, EMAIL, TELEFONE, FAX, DESIGNACAO, "
                + " FORNECEDOR.CODPOSTAL1 as cod1, FORNECEDOR.CODPOSTAL2 as cod2, RUA, DISTRITO, LOCALIDADE, "
                + " N_PORTA FROM FORNECEDOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = FORNECEDOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = FORNECEDOR.CODPOSTAL2)"
                + " WHERE TELEFONE LIKE @tel;", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@tel", "%" + tel + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
            while (reader.Read())
            {
                Fornecedor f = new Fornecedor();
                f.NIF_Fornecedor = reader["NIF"].ToString();
                f.Email = reader["EMAIL"].ToString();
                f.Nome = reader["NOME"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                fornecedores.Add(f);
            }
            this.closeSGBDConnection();
            return fornecedores;
        }

        public ObservableCollection<Fornecedor> searchFornecedoresInDBMail(string mail)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT NOME, NIF, EMAIL, TELEFONE, FAX, DESIGNACAO, "
                + " FORNECEDOR.CODPOSTAL1 as cod1, FORNECEDOR.CODPOSTAL2 as cod2, RUA, DISTRITO, LOCALIDADE, "
                + " N_PORTA FROM FORNECEDOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = FORNECEDOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = FORNECEDOR.CODPOSTAL2)"
                + " WHERE EMAIL LIKE @mail;", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@mail", "%" + mail + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
            while (reader.Read())
            {
                Fornecedor f = new Fornecedor();
                f.NIF_Fornecedor = reader["NIF"].ToString();
                f.Email = reader["EMAIL"].ToString();
                f.Nome = reader["NOME"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                fornecedores.Add(f);
            }
            this.closeSGBDConnection();
            return fornecedores;
        }

        public ObservableCollection<Fornecedor> searchFornecedoresInDBNIF(string nif)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT NOME, NIF, EMAIL, TELEFONE, FAX, DESIGNACAO, "
                + " FORNECEDOR.CODPOSTAL1 as cod1, FORNECEDOR.CODPOSTAL2 as cod2, RUA, DISTRITO, LOCALIDADE, "
                + " N_PORTA FROM FORNECEDOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = FORNECEDOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = FORNECEDOR.CODPOSTAL2)"
                + " WHERE NIF LIKE @nif;", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nif", "%" + nif + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Fornecedor> fornecedores = new ObservableCollection<Fornecedor>();
            while (reader.Read())
            {
                Fornecedor f = new Fornecedor();
                f.NIF_Fornecedor = reader["NIF"].ToString();
                f.Email = reader["EMAIL"].ToString();
                f.Nome = reader["NOME"].ToString();
                f.Fax = reader["FAX"].ToString();
                f.Telefone = reader["TELEFONE"].ToString();
                f.Designacao = reader["DESIGNACAO"].ToString();
                f.Localizacao = new Localizacao();
                f.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                f.Localizacao.Rua1 = reader["RUA"].ToString();
                f.Localizacao.Distrito = reader["DISTRITO"].ToString();
                f.Localizacao.Localidade = reader["LOCALIDADE"].ToString();
                f.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                fornecedores.Add(f);
            }
            this.closeSGBDConnection();
            return fornecedores;
        }



        public void EnviarFornecedor(Fornecedor f)
        {

            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO Fornecedor (NIF, EMAIL, NOME, FAX, TELEFONE, DESIGNACAO, CODPOSTAL1, CODPOSTAL2, RUA, N_PORTA)" +
                "VALUES (@NIF, @Email, @Nome, @Fax, @Telefone, @design, @codPostal1, @codPostal2, @rua, @N_PORTA);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NIF", f.NIF_Fornecedor);
            cmd.Parameters.AddWithValue("@Email", f.Email);
            cmd.Parameters.AddWithValue("@Nome", f.Nome);
            if (string.IsNullOrEmpty(f.Fax))
                cmd.Parameters.AddWithValue("@Fax", DBNull.Value);

            else
                cmd.Parameters.AddWithValue("@Fax", f.Fax);

            cmd.Parameters.AddWithValue("@Telefone", f.Telefone);
            cmd.Parameters.AddWithValue("@design", f.Designacao);
            cmd.Parameters.AddWithValue("@codPostal1", f.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@codPostal2", f.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@rua", f.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@N_PORTA", f.Localizacao.Porta);
            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao adicionar Fornecedor na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }
        }

        public ObservableCollection<MaterialTextil> getMateriaisFornecedorFromDB(int NifForn)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * "
                                + " FROM [MATERIAIS_TÊXTEIS] JOIN FORNECEDOR ON FORNECEDOR.NIF=[MATERIAIS_TÊXTEIS].NIF_FORNECEDOR "
                                + "WHERE NIF_FORNECEDOR = @nif", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nif", NifForn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<MaterialTextil> m = new ObservableCollection<MaterialTextil>();
            while (reader.Read())
            {
                MaterialTextil material = new MaterialTextil();
                material.Fornecedor = new Fornecedor();
                material.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                material.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                material.Cor = reader["COR"].ToString();
                material.Designacao = reader["DESIGNACAO"].ToString();
                material.Fornecedor.Nome = reader["NOME"].ToString();
                material.Fornecedor.NIF_Fornecedor = reader["NIF"].ToString();
                m.Add(material);
            }
            reader.Close();
            for (int i = 0; i < m.Count; i++)
            {
                m.ElementAt(i).TipoMaterial1 = getMaterialType(m.ElementAt(i).Referencia);
                m.ElementAt(i).QuantidadeStockD = getQuantidadeMaterial(m.ElementAt(i).Referencia);
            }
            this.closeSGBDConnection();
            return m;
        }

        public void AtualizarFornecedor(Fornecedor f)
        {
            int rows = 0;
            if (!this.verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE FORNECEDOR " + "SET EMAIL = @EMAIL, "
                + " TELEFONE = @TELEFONE, " + " CODPOSTAL1 = @CODPOSTAL1, " + " CODPOSTAL2 = @CODPOSTAL2, " +
                " FAX = @Fax, " + "  RUA = @RUA, " + " N_PORTA = @N_PORTA, "
                + "DESIGNACAO = @desi, " + "NOME = @nome " + "WHERE NIF = @NIF";
            cmd.Parameters.Clear();
            if (string.IsNullOrEmpty(f.Fax))
            {
                cmd.Parameters.AddWithValue("@Fax", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Fax", f.Fax);
            }
            cmd.Parameters.AddWithValue("@NIF", f.NIF_Fornecedor);
            cmd.Parameters.AddWithValue("@EMAIL", f.Email);
            cmd.Parameters.AddWithValue("@TELEFONE", f.Telefone);
            cmd.Parameters.AddWithValue("@CODPOSTAL1", f.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@CODPOSTAL2", f.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@RUA", f.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@N_PORTA", f.Localizacao.Porta);
            cmd.Parameters.AddWithValue("@desi", f.Designacao);
            cmd.Parameters.AddWithValue("@nome", f.Nome);
            cmd.Connection = this.Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar a Filial na base de dados\n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    Xceed.Wpf.Toolkit.MessageBox.Show("A atualização do Fornecedor foi submetida com sucesso!", "Atualização Bem Sucedida", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                   if (Xceed.Wpf.Toolkit.MessageBox.Show("Não foi possivel atualizar a informação do fornecedor. Pretende tentar novamente?", "Erro", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    this.AtualizarFornecedor(f);
                this.closeSGBDConnection();
            }
        }

        // ------------ empregados --------

        public ObservableCollection<Utilizador> getEmpregadosFromDB()
        {
            if (!this.verifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, NOME, EMAIL, TELEFONE, RUA, N_PORTA, "
                + "DISTRITO, LOCALIDADE, SALARIO, N_FABRICA, HORA_ENTRADA, HORA_SAIDA, UTILIZADOR.CODPOSTAL1 as cod1, "
                + " UTILIZADOR.CODPOSTAL2 as cod2, IMAGEM, N_FUNCIONARIO_SUPER FROM UTILIZADOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = UTILIZADOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = UTILIZADOR.CODPOSTAL2)", this.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Utilizador> user = new ObservableCollection<Utilizador>();
            while (reader.Read())
            {
                Utilizador u = new Utilizador();
                u.Localizacao = new Localizacao();
                u.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                u.Nome = reader["NOME"].ToString();
                u.Email = reader["EMAIL"].ToString();
                u.Telemovel = reader["TELEFONE"].ToString();
                u.Salario = Convert.ToDouble(reader["SALARIO"].ToString());
                u.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                u.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());

                u.Filial = new filial();
                u.Filial.NFilial = Convert.ToInt32(reader["N_FABRICA"].ToString());
                u.Supervisor = new Utilizador();
                u.Supervisor.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO_SUPER"].ToString());
                u.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                u.Localizacao.Rua1 = reader["RUA"].ToString();
                u.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                u.Localizacao.Distrito = reader["DISTRITO"].ToString();
                u.Localizacao.Localidade = reader["LOCALIDADE"].ToString();

                byte[] img = null;
                try
                {
                    img = (byte[])reader["IMAGEM"];
                }
                catch (InvalidCastException e)
                {

                }
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    u.Imagem = Image.FromStream(ms);
                }
                user.Add(u);
            }
            reader.Close();

            foreach (Utilizador u in user)
            {
                u.TiposUser = this.getUserTypesFromDB(u.NFuncionario);
            }
            this.closeSGBDConnection();
            return user;
        }

        public ObservableCollection<Utilizador> searchEmpregadosInDB(string nome)
        {
            if (!this.verifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, NOME, EMAIL, TELEFONE, RUA, N_PORTA, "
                + "DISTRITO, LOCALIDADE, SALARIO, N_FABRICA, HORA_ENTRADA, HORA_SAIDA, UTILIZADOR.CODPOSTAL1 as cod1, "
                + " UTILIZADOR.CODPOSTAL2 as cod2, IMAGEM, N_FUNCIONARIO_SUPER FROM UTILIZADOR JOIN ZONA ON "
                + "(ZONA.CODPOSTAL1 = UTILIZADOR.CODPOSTAL1 AND ZONA.CODPOSTAL2 = UTILIZADOR.CODPOSTAL2) "
                + "WHERE NOME LIKE @nome;", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nome", "%" + nome + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Utilizador> user = new ObservableCollection<Utilizador>();
            while (reader.Read())
            {
                Utilizador u = new Utilizador();
                u.Localizacao = new Localizacao();
                u.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                u.Nome = reader["NOME"].ToString();
                u.Email = reader["EMAIL"].ToString();
                u.Telemovel = reader["TELEFONE"].ToString();
                u.Salario = Convert.ToDouble(reader["SALARIO"].ToString());
                u.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                u.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());

                u.Filial = new filial();
                u.Filial.NFilial = Convert.ToInt32(reader["N_FABRICA"].ToString());
                u.Supervisor = new Utilizador();
                u.Supervisor.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO_SUPER"].ToString());
                u.Localizacao.CodigoPostal = reader["cod1"].ToString() + "-" + reader["cod2"].ToString();
                u.Localizacao.Rua1 = reader["RUA"].ToString();
                u.Localizacao.Porta = Convert.ToInt32(reader["N_PORTA"].ToString());
                u.Localizacao.Distrito = reader["DISTRITO"].ToString();
                u.Localizacao.Localidade = reader["LOCALIDADE"].ToString();

                byte[] img = null;
                try
                {
                    img = (byte[])reader["IMAGEM"];
                }
                catch (InvalidCastException e)
                {

                }
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    u.Imagem = Image.FromStream(ms);
                }
                user.Add(u);
            }
            reader.Close();

            foreach (Utilizador u in user)
            {
                u.TiposUser = this.getUserTypesFromDB(u.NFuncionario);
            }
            this.closeSGBDConnection();
            return user;
        }


        public List<string> getUserTypesFromDB(int nFunc)
        {

            SqlCommand cmd = new SqlCommand("SELECT TIPO FROM [UTILIZADOR-TIPOS]"
            + " JOIN [TIPO-UTILIZADOR] ON ID_TIPO = ID WHERE UTILIZADOR = @nFunc", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nFunc", nFunc);
            List<string> userTypes = new List<string>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userTypes.Add(reader["TIPO"].ToString());
            }
            reader.Close();
            return userTypes;
        }

        public void EnviarEmpregado(Utilizador user)
        {

            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO UTILIZADOR (NOME, EMAIL, SALARIO, PASS, TELEFONE, N_FABRICA, "
                + " HORA_ENTRADA, HORA_SAIDA, CODPOSTAL1, CODPOSTAL2, RUA, N_PORTA, N_FUNCIONARIO_SUPER) "
                + "VALUES (@NOME, @email, @salario, @pass, @telefone, @nFilial, @entrada, @saida, @cod1, @cod2, @rua, "
                + "@nPorta, @super);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NOME", user.Nome);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@salario", user.Salario);
            cmd.Parameters.AddWithValue("@pass", user.Password);
            cmd.Parameters.AddWithValue("@telefone", user.Telemovel);
            cmd.Parameters.AddWithValue("@nFilial", user.Filial.NFilial);
            cmd.Parameters.AddWithValue("@entrada", user.HoraEntrada);
            cmd.Parameters.AddWithValue("@saida", user.HoraSaida);
            cmd.Parameters.AddWithValue("@cod1", user.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@cod2", user.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@rua", user.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@nPorta", user.Localizacao.Porta);
            cmd.Parameters.AddWithValue("@super", user.Supervisor.NFuncionario);

            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o empregado.\n Detalhes do erro: \n " + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }
            //obter o nº do Funcionario inserido
            int n = this.getLastIdentity("UTILIZADOR");
            //inserir o tipo de funcionario
            int id;
            if (!this.verifySGBDConnection())
                return;
            foreach (string s in user.TiposUser)
            {
                id = getTypeIDfromDB(s);
                if (!this.verifySGBDConnection())
                    return;
                cmd = new SqlCommand("INSERT INTO [UTILIZADOR-TIPOS](UTILIZADOR, ID_TIPO) VALUES " +
                    "(@nFunc, @IDtipo)", this.cn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nFunc", n);

                cmd.Parameters.AddWithValue("@IDtipo", id);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao registar o tipo de empregado. \n Detalhes do erro: \n" + ex.Message);
                }
                finally
                {
                    this.closeSGBDConnection();
                }
            }
        }

        public int getTypeIDfromDB(string type)
        {
            if (!this.verifySGBDConnection())
                return 0;
            SqlCommand cmd = new SqlCommand("SELECT dbo.getTypeID(@type);", this.cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@type", type);
            int n = Convert.ToInt32(cmd.ExecuteScalar());
            this.closeSGBDConnection();
            return n;
        }

        public void atualizarEmpregado(Utilizador user)
        {
            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE UTILIZADOR SET NOME = @NOME, SALARIO = @salario, "
                + " N_FABRICA = @nFilial, HORA_ENTRADA = @entrada, HORA_SAIDA = @saida, "
                + " N_FUNCIONARIO_SUPER = @super "
                + "WHERE N_FUNCIONARIO = @nfunc";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nfunc", user.NFuncionario);
            cmd.Parameters.AddWithValue("@NOME", user.Nome);
            cmd.Parameters.AddWithValue("@salario", user.Salario);
            cmd.Parameters.AddWithValue("@nFilial", user.Filial.NFilial);
            cmd.Parameters.AddWithValue("@entrada", user.HoraEntrada);
            cmd.Parameters.AddWithValue("@saida", user.HoraSaida);
            cmd.Parameters.AddWithValue("@super", user.Supervisor.NFuncionario);

            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao editar o empregado.\n Detalhes do erro: \n " + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }


            if (!this.verifySGBDConnection())
                return;
            cmd = new SqlCommand("DELETE FROM [UTILIZADOR-TIPOS] WHERE UTILIZADOR = @nFunc ", this.cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nFunc", user.NFuncionario);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao editar o tipo de empregado. \n Detalhes do erro: \n" + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }

            //editar o tipo de funcionario
            int id;
            if (!this.verifySGBDConnection())
                return;
            foreach (string s in user.TiposUser)
            {
                id = getTypeIDfromDB(s);
                if (!this.verifySGBDConnection())
                    return;
                cmd = new SqlCommand("INSERT INTO [UTILIZADOR-TIPOS] (UTILIZADOR, ID_TIPO) VALUES " +
                    "(@nFunc, @IDtipo)", this.cn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@nFunc", user.NFuncionario);
                cmd.Parameters.AddWithValue("@IDtipo", id);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao editar o tipo de empregado. \n Detalhes do erro: \n" + ex.Message);
                }
                finally
                {
                    this.closeSGBDConnection();
                }
            }
        }


        public void editarInfPessoal(Utilizador user)
        {
            if (!this.verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE UTILIZADOR SET EMAIL = @email, "
                + " TELEFONE = @telefone , CODPOSTAL1 = @cod1, CODPOSTAL2 = @cod2, RUA = @rua, N_PORTA = @nPorta "
                + "WHERE N_FUNCIONARIO = @nfunc";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nfunc", user.NFuncionario);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@telefone", user.Telemovel);
            cmd.Parameters.AddWithValue("@cod1", user.Localizacao.CodigoPostal1);
            cmd.Parameters.AddWithValue("@cod2", user.Localizacao.CodigoPostal2);
            cmd.Parameters.AddWithValue("@rua", user.Localizacao.Rua1);
            cmd.Parameters.AddWithValue("@nPorta", user.Localizacao.Porta);

            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao editar informações pessoais.\n Detalhes do erro: \n " + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }
        }




        public Utilizador getSupervisor(int nFunc)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM UTILIZADOR WHERE N_FUNCIONARIO = @supervisor", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@supervisor", nFunc);
            SqlDataReader reader = cmd.ExecuteReader();
            Utilizador user = new Utilizador();
            while (reader.Read())
            {
                user.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO"].ToString());
                user.Nome = reader["NOME"].ToString();
                user.Email = reader["EMAIL"].ToString();
                user.Telemovel = reader["TELEFONE"].ToString();
                user.HoraEntrada = TimeSpan.Parse(reader["HORA_ENTRADA"].ToString());
                user.HoraSaida = TimeSpan.Parse(reader["HORA_SAIDA"].ToString());
                byte[] img = null;
                try
                {
                    img = (byte[])reader["IMAGEM"];
                }
                catch (InvalidCastException e)
                {

                }
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    user.Imagem = Image.FromStream(ms);
                }
            }
            reader.Close();
            this.closeSGBDConnection();
            return user;
        }

        //materiais 

        public ObservableCollection<MaterialTextil> getMateriaisFromDB()
        {
            this.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT * "
                                + " FROM [MATERIAIS_TÊXTEIS] JOIN FORNECEDOR ON FORNECEDOR.NIF=[MATERIAIS_TÊXTEIS].NIF_FORNECEDOR"
                                , this.Cn);

            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<MaterialTextil> m = new ObservableCollection<MaterialTextil>();
            while (reader.Read())
            {
                MaterialTextil material = new MaterialTextil();
                material.Fornecedor = new Fornecedor();
                material.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                material.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                material.Cor = reader["COR"].ToString();
                material.Designacao = reader["DESIGNACAO"].ToString();
                material.Fornecedor.Nome = reader["NOME"].ToString();
                material.Fornecedor.NIF_Fornecedor = reader["NIF"].ToString();
                m.Add(material);
            }
            reader.Close();
            for (int i = 0; i < m.Count; i++)
            {
                m.ElementAt(i).TipoMaterial1 = getMaterialType(m.ElementAt(i).Referencia);
                m.ElementAt(i).QuantidadeStockD = getQuantidadeMaterial(m.ElementAt(i).Referencia);
            }
            this.closeSGBDConnection();
            return m;
        }


        public ObservableCollection<MaterialTextil> getMaterialFromDBRef(int refFabrica)
        {
            this.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT * "
                                + " FROM [MATERIAIS_TÊXTEIS] JOIN FORNECEDOR ON FORNECEDOR.NIF=[MATERIAIS_TÊXTEIS].NIF_FORNECEDOR "
                                + "WHERE REFERENCIA_FABRICA = @ref"
                                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", refFabrica);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<MaterialTextil> m = new ObservableCollection<MaterialTextil>();
            while (reader.Read())
            {
                MaterialTextil material = new MaterialTextil();
                material.Fornecedor = new Fornecedor();
                material.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                material.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                material.Cor = reader["COR"].ToString();
                material.Designacao = reader["DESIGNACAO"].ToString();
                material.Fornecedor.Nome = reader["NOME"].ToString();
                material.Fornecedor.NIF_Fornecedor = reader["NIF"].ToString();
                m.Add(material);
            }
            reader.Close();


            for (int i = 0; i < m.Count; i++)
            {
                m.ElementAt(i).TipoMaterial1 = getMaterialType(m.ElementAt(i).Referencia);
                m.ElementAt(i).QuantidadeStockD = getQuantidadeMaterial(m.ElementAt(i).Referencia);
            }


            this.closeSGBDConnection();
            return m;
        }

        public ObservableCollection<MaterialTextil> getMaterialFromDBCor(string cor)
        {
            this.verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT * "
                                + " FROM [MATERIAIS_TÊXTEIS] JOIN FORNECEDOR ON FORNECEDOR.NIF=[MATERIAIS_TÊXTEIS].NIF_FORNECEDOR "
                                + "WHERE COR LIKE @cor"
                                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@cor", "%" + cor + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<MaterialTextil> m = new ObservableCollection<MaterialTextil>();
            while (reader.Read())
            {
                MaterialTextil material = new MaterialTextil();
                material.Fornecedor = new Fornecedor();
                material.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                material.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                material.Cor = reader["COR"].ToString();
                material.Designacao = reader["DESIGNACAO"].ToString();
                material.Fornecedor.Nome = reader["NOME"].ToString();
                material.Fornecedor.NIF_Fornecedor = reader["NIF"].ToString();
                m.Add(material);
            }
            reader.Close();

            for (int i = 0; i < m.Count; i++)
            {
                m.ElementAt(i).TipoMaterial1 = getMaterialType(m.ElementAt(i).Referencia);
                m.ElementAt(i).QuantidadeStockD = getQuantidadeMaterial(m.ElementAt(i).Referencia);
            }


            this.closeSGBDConnection();
            return m;
        }

        public ObservableCollection<MaterialTextil> materiaisProduto(int referencia, String tamanho, String cor, int id, int qtProd)
        {
            ObservableCollection<MaterialTextil> mt = new ObservableCollection<MaterialTextil>();
            if (!this.verifySGBDConnection())
                return mt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.getProductMaterials(@ref, @tamanho, @cor, @id);", this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", referencia);
            cmd.Parameters.AddWithValue("@tamanho", tamanho);
            cmd.Parameters.AddWithValue("@cor", cor);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MaterialTextil m = new MaterialTextil();
                m.Designacao = reader["DESIGNACAO"].ToString();
                m.Fornecedor = new Fornecedor();
                m.Fornecedor.NIF_Fornecedor = reader["NIF_FORNECEDOR"].ToString();
                m.ReferenciaFornecedor = reader["REFERENCIA_FORN"].ToString();
                m.Cor = reader["COR"].ToString();
                m.Referencia = Convert.ToInt32(reader["REFERENCIA_FABRICA"].ToString());
                m.QuantidadeSelecionada = (Convert.ToDouble(reader["QUANTIDADE"].ToString()) * qtProd).ToString();
                m.QuantidadeSelecionadaD = Convert.ToDouble(m.QuantidadeSelecionada) * qtProd;
                mt.Add(m);
            }
            reader.Close();
            this.closeSGBDConnection();
            return mt;
        }


        public ObservableCollection<ProdutoPersonalizado> getProdutosContainingMaterial(int refFabrica)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM [MATERIAIS-PRODUTO] WHERE REFERENCIA_FABRICA = @ref", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@ref", refFabrica);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtos = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Quantidade = Convert.ToInt32(reader["QUANTIDADE"].ToString());
                produtos.Add(prod);
            }
            reader.Close();
            this.closeSGBDConnection();
            return produtos;
        }
        public bool produzirProduto(ProdutoPersonalizado prodPers, int qtd)
        {
            if (!this.verifySGBDConnection())
                return false;

            SqlCommand cmd = new SqlCommand("DECLARE @validation INT; EXEC dbo.produzirProduto @ref, "
                + "@tamanho, @cor, @id, @qtd, @validation OUTPUT; SELECT @validation", this.Cn);

            cmd.Parameters.AddWithValue("@ref", prodPers.ProdutoBase.Referencia);
            cmd.Parameters.AddWithValue("@tamanho", prodPers.Tamanho);
            cmd.Parameters.AddWithValue("@cor", prodPers.Cor);
            cmd.Parameters.AddWithValue("@id", prodPers.ID);
            cmd.Parameters.AddWithValue("@qtd", qtd);
            int retVal = Convert.ToInt32(cmd.ExecuteScalar());
            this.closeSGBDConnection();
            if (retVal == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public ObservableCollection<ProdutoPersonalizado> getProdutosPers()
        {
            verifySGBDConnection();
            SqlCommand cmd = new SqlCommand("SELECT TAMANHO, COR, ID, PRECO, UNIDADES_ARMAZEM, "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-BASE].NOME as nomeBase, DATA_ALTERACAO, "
                + "INSTRUCOES_PRODUCAO, IVA, PAIS_FABRICO, [PRODUTO-PERSONALIZADO].N_ETIQUETA, NORMAS, "
                + "PAIS_FABRICO, COMPOSICAO FROM[PRODUTO-PERSONALIZADO] JOIN[PRODUTO-BASE] ON "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA JOIN "
                + "ETIQUETA ON ETIQUETA.N_ETIQUETA = [PRODUTO-PERSONALIZADO].N_ETIQUETA; "
                , this.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtoPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.ProdutoBase.Nome = reader["nomeBase"].ToString();
                prod.ProdutoBase.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.ProdutoBase.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.ProdutoBase.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.Etiqueta = new Etiqueta();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                prod.Etiqueta.Normas = reader["NORMAS"].ToString();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                produtoPers.Add(prod);
            }
            reader.Close();
            this.closeSGBDConnection();
            return produtoPers;
        }

        public ObservableCollection<Etiqueta> getEtiquetas()
        {
            if (!this.verifySGBDConnection())
                return null;

            SqlCommand cmd = new SqlCommand("SELECT * FROM ETIQUETA", this.Cn);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<Etiqueta> etiquetas = new ObservableCollection<Etiqueta>();
            while (reader.Read())
            {
                Etiqueta et = new Etiqueta();
                et.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                et.Normas = reader["NORMAS"].ToString();
                et.Composicao = reader["COMPOSICAO"].ToString();
                et.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                etiquetas.Add(et);
            }
            reader.Close();
            this.closeSGBDConnection();
            return etiquetas;
        }

        public ObservableCollection<ProdutoPersonalizado> searchAndGetProdutosPersID(int ID)
        {
            if (!this.verifySGBDConnection())
                return null;
            SqlCommand cmd = new SqlCommand("SELECT TAMANHO, COR, ID, PRECO, UNIDADES_ARMAZEM, "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA, [PRODUTO-BASE].NOME as nomeBase, DATA_ALTERACAO, "
                + "INSTRUCOES_PRODUCAO, IVA, PAIS_FABRICO, [PRODUTO-PERSONALIZADO].N_ETIQUETA, NORMAS, "
                + "PAIS_FABRICO, COMPOSICAO FROM[PRODUTO-PERSONALIZADO] JOIN[PRODUTO-BASE] ON "
                + "[PRODUTO-PERSONALIZADO].REFERENCIA = [PRODUTO-BASE].REFERENCIA JOIN "
                + "ETIQUETA ON ETIQUETA.N_ETIQUETA = [PRODUTO-PERSONALIZADO].N_ETIQUETA WHERE ID = @id; "
                , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", ID);
            SqlDataReader reader = cmd.ExecuteReader();
            ObservableCollection<ProdutoPersonalizado> produtoPers = new ObservableCollection<ProdutoPersonalizado>();
            while (reader.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Tamanho = reader["TAMANHO"].ToString();
                prod.Cor = reader["COR"].ToString();
                prod.ID = Convert.ToInt32(reader["ID"].ToString());
                prod.Preco = Convert.ToDouble(reader["PRECO"].ToString());
                prod.UnidadesStock = Convert.ToInt32(reader["UNIDADES_ARMAZEM"].ToString());
                prod.ProdutoBase = new ProdutoBase();
                prod.ProdutoBase.Referencia = Convert.ToInt32(reader["REFERENCIA"].ToString());
                prod.ProdutoBase.Nome = reader["nomeBase"].ToString();
                prod.ProdutoBase.InstrProd = reader["INSTRUCOES_PRODUCAO"].ToString();
                prod.ProdutoBase.DataAlteraçao = Convert.ToDateTime(reader["DATA_ALTERACAO"]);
                prod.ProdutoBase.IVA1 = Convert.ToDouble(reader["IVA"].ToString());
                prod.Etiqueta = new Etiqueta();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Numero = Convert.ToInt32(reader["N_ETIQUETA"].ToString());
                prod.Etiqueta.Normas = reader["NORMAS"].ToString();
                prod.Etiqueta.PaisFabrico = reader["PAIS_FABRICO"].ToString();
                prod.Etiqueta.Composicao = reader["COMPOSICAO"].ToString();
                produtoPers.Add(prod);
            }
            reader.Close();
            this.closeSGBDConnection();
            return produtoPers;
        }


        public bool EnviarProduto(ProdutoPersonalizado prod, ObservableCollection<MaterialTextil> materiaisSelecionados)
        {
            //registar etiqueta na base de dados primerio
            //inserir primeiro a nova etiqueta na base de dados
            this.insertEtiqueta(prod.Etiqueta.Normas, prod.Etiqueta.Composicao, prod.Etiqueta.PaisFabrico);
            //obter o numero da etiqueta adicionada, pois é necessario para o registo do produto
            prod.Etiqueta.Numero = this.getEtiqueta(prod.Etiqueta.Normas, prod.Etiqueta.Composicao, prod.Etiqueta.PaisFabrico);

            //produto
            if (!this.verifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO [PRODUTO-PERSONALIZADO] (REFERENCIA, TAMANHO, COR, PRECO, N_ETIQUETA) "
                + "VALUES (@refProdBase, @tamanho, @cor, @preco, @nEt);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@refProdBase", prod.ProdutoBase.Referencia);
            cmd.Parameters.AddWithValue("@tamanho", prod.Tamanho);
            cmd.Parameters.AddWithValue("@cor", prod.Cor);
            cmd.Parameters.AddWithValue("@preco", prod.Preco);
            cmd.Parameters.AddWithValue("@nEt", prod.Etiqueta.Numero);
            cmd.Connection = this.Cn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registar o Produto na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                this.closeSGBDConnection();
            }
            //materiais do produto
            int ID = this.getLastIdentity("[PRODUTO-PERSONALIZADO]");

            for (int i = 0; i < materiaisSelecionados.Count; i++)
            {
                if (!this.verifySGBDConnection())
                    return false;
                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [MATERIAIS-PRODUTO] (REFERENCIA, TAMANHO, COR, ID, REFERENCIA_FABRICA, QUANTIDADE) "
                    + "VALUES (@refProdBase, @tamanho, @cor, @id, @refFabrica, @qtd);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@refProdBase", prod.ProdutoBase.Referencia);
                cmd.Parameters.AddWithValue("@tamanho", prod.Tamanho);
                cmd.Parameters.AddWithValue("@cor", prod.Cor);
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@refFabrica", materiaisSelecionados.ElementAt(i).Referencia);
                cmd.Parameters.AddWithValue("@qtd", Convert.ToDouble(materiaisSelecionados.ElementAt(i).QuantidadeSelecionada));
                cmd.Connection = this.Cn;
                cmd.Connection = this.Cn;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Falha ao registar o Produto na base de dados. \n ERROR MESSAGE: \n" + ex.Message);
                }
            }
            return true;
        }

        public bool changePass(int nFunc, String oldPass, String newPass)
        {
            if (!this.verifySGBDConnection())
                return false;
            SqlCommand cmd = new SqlCommand("SELECT dbo.getUserPass(@nFunc)", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@nFunc", nFunc);
            String userPass = (String)cmd.ExecuteScalar();
            if (!userPass.Equals(oldPass, StringComparison.Ordinal))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("A palavra passe atual está incorreta. A password não foi alterada.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            cmd = new SqlCommand("UPDATE  UTILIZADOR SET PASS = @pass WHERE N_FUNCIONARIO = @nFunc", Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pass", newPass);
            cmd.Parameters.AddWithValue("@nFunc", nFunc);
            cmd.ExecuteNonQuery();
            return true;
        }


        public void AtualizarProdutoBase(ProdutoBase prod)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "UPDATE [PRODUTO-BASE] SET NOME = @NOME, DATA_ALTERACAO = @DataAlt, " +
                " IVA = @iva, N_GESTOR_PROD = @N_GestorProd, INSTRUCOES_PRODUCAO = @IntrProd, IMAGEM_DESENHO=@imagem WHERE REFERENCIA = @Referencia";
            cmd.Parameters.Clear(); //falta inserir a imagem do produto!
            cmd.Parameters.AddWithValue("@NOME", prod.Nome);
            cmd.Parameters.AddWithValue("@iva", prod.IVA1);
            cmd.Parameters.AddWithValue("@N_GestorProd", prod.GestorProducao.NFuncionario);
            cmd.Parameters.AddWithValue("@DataAlt", DateTime.Today);
            cmd.Parameters.AddWithValue("@IntrProd", prod.InstrProd);
            cmd.Parameters.AddWithValue("@Referencia", prod.Referencia);
            cmd.Parameters.AddWithValue("@imagem", prod.Pic);
            cmd.Connection = Cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel atualizar o Desenho produto \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows != 1)
                    throw new Exception("Não foi possivel atualizar o Produto Base");
                closeSGBDConnection();
            }
        }
    }
}
