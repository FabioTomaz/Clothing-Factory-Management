using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;

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

        public DataHandler() {
            Cn = getSGBDConnection();
        }
        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("data source=localhost;integrated security=true;initial catalog=GESTAO-FABRICA-VESTUARIO-LABORAL");
        }

        public bool verifySGBDConnection()
        {
            if (Cn == null)
                Cn = getSGBDConnection();

            if (Cn.State != ConnectionState.Open)
                try
                {
                    Cn.Open();
                }
                catch (System.Data.SqlClient.SqlException) {
                    return false;
                }

            return Cn.State == ConnectionState.Open;
        }

        public bool closeSGBDConnection() {

            if (Cn.State != ConnectionState.Closed)
                Cn.Close();

            return Cn.State == ConnectionState.Closed;
        }

        public Utilizador getUtilizadorFromDB(int user)
        {
            this.verifySGBDConnection();
            Utilizador utilizador = new Utilizador();
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, IMAGEM, UTILIZADOR.EMAIL AS EMAILUSER, SALARIO, NOME, TIPO, PASS, UTILIZADOR.TELEFONE AS TELEFONEUSER, HORA_ENTRADA, HORA_SAIDA, IMAGEM, N_FUNCIONARIO_SUPER, ZONAUSER.COD_POSTAL AS CODUSER, ZONAUSER.DISTRITO AS DISTRITOUSER, ZONAUSER.CONCELHO AS CONCELHOUSER, ZONAUSER.LOCALIDADE AS LOCALIDADEUSER, UTILIZADOR.RUA AS RUAUSER, UTILIZADOR.N_PORTA AS PORTAUSER, ZONAFABRICA.COD_POSTAL AS CODFABRICA, ZONAFABRICA.DISTRITO AS DISTRITOFABRICA, ZONAFABRICA.CONCELHO AS CONCELHOFABRICA, ZONAFABRICA.LOCALIDADE AS LOCALIDADEFABRICA, [FABRICA-FILIAL].RUA AS RUAFABRICA, [FABRICA-FILIAL].N_PORTA AS PORTAFABRICA, [FABRICA-FILIAL].EMAIL AS EMAILFABRICA, [FABRICA-FILIAL].TELEFONE AS TELEFONEFABRICA, [FABRICA-FILIAL].FAX AS FAXFABRICA, N_FILIAL  FROM UTILIZADOR"
                                            + " JOIN ZONA AS ZONAUSER ON UTILIZADOR.COD_POSTAL = ZONAUSER.COD_POSTAL"
                                            + " JOIN[FABRICA-FILIAL] ON[FABRICA-FILIAL].N_FILIAL = UTILIZADOR.N_FABRICA"
                                            + " JOIN ZONA AS ZONAFABRICA ON[FABRICA-FILIAL].COD_POSTAL = ZONAFABRICA.COD_POSTAL"
                                            + " JOIN[UTILIZADOR-TIPOS] ON[UTILIZADOR-TIPOS].UTILIZADOR = UTILIZADOR.N_FUNCIONARIO"
                                            + " JOIN[TIPO-UTILIZADOR] ON[TIPO-UTILIZADOR].ID =[UTILIZADOR-TIPOS].ID_TIPO"
                                            + " WHERE N_FUNCIONARIO = @USER;"
                                            , this.Cn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@USER", user);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            utilizador.Filial = new filial();
            utilizador.Filial.NFilial = Convert.ToInt32(reader["N_FILIAL"].ToString());
            utilizador.Filial.Email = reader["EMAILFABRICA"].ToString();
            utilizador.Filial.Fax = reader["FAXFABRICA"].ToString();
            utilizador.Filial.Telefone = reader["TELEFONEFABRICA"].ToString();
            utilizador.Filial.Localizacao = new Localizacao();
            utilizador.Filial.Localizacao.CodigoPostal = reader["CODFABRICA"].ToString();
            utilizador.Filial.Localizacao.Distrito = reader["DISTRITOFABRICA"].ToString();
            utilizador.Filial.Localizacao.Concelho = reader["CONCELHOFABRICA"].ToString();
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
            utilizador.TipoUser = reader["TIPO"].ToString();
            utilizador.Localizacao = new Localizacao();
            utilizador.Localizacao.CodigoPostal = reader["CODUSER"].ToString();
            utilizador.Localizacao.Distrito = reader["DISTRITOUSER"].ToString();
            utilizador.Localizacao.Concelho = reader["CONCELHOUSER"].ToString();
            utilizador.Localizacao.Localidade = reader["LOCALIDADEUSER"].ToString();
            utilizador.Localizacao.Porta = Convert.ToInt32(reader["PORTAUSER"].ToString());
            utilizador.Localizacao.Rua1 = reader["RUAUSER"].ToString();
            utilizador.Supervisor = new Utilizador();
            utilizador.Supervisor.NFuncionario = Convert.ToInt32(reader["N_FUNCIONARIO_SUPER"].ToString());
            byte[] img = (byte[])reader["IMAGEM"];
            if(img == null)
            {
                MemoryStream ms = new MemoryStream(img);
                utilizador.Imagem = Image.FromStream(ms);
            }
            else
            {
                MemoryStream ms = new MemoryStream(img);
                utilizador.Imagem = Image.FromStream(ms);
            }
            this.closeSGBDConnection();
            return utilizador;
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

    }
}
