using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

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
            SqlCommand cmd = new SqlCommand("SELECT N_FUNCIONARIO, UTILIZADOR.EMAIL AS EMAILUSER, SALARIO, NOME, TIPO, PASS, UTILIZADOR.TELEFONE AS TELEFONEUSER, HORA_ENTRADA, HORA_SAIDA, IMAGEM, N_FUNCIONARIO_SUPER, ZONAUSER.COD_POSTAL AS CODUSER, ZONAUSER.DISTRITO AS DISTRITOUSER, ZONAUSER.CONCELHO AS CONCELHOUSER, ZONAUSER.LOCALIDADE AS LOCALIDADEUSER, UTILIZADOR.RUA AS RUAUSER, UTILIZADOR.N_PORTA AS PORTAUSER, ZONAFABRICA.COD_POSTAL AS CODFABRICA, ZONAFABRICA.DISTRITO AS DISTRITOFABRICA, ZONAFABRICA.CONCELHO AS CONCELHOFABRICA, ZONAFABRICA.LOCALIDADE AS LOCALIDADEFABRICA, [FABRICA-FILIAL].RUA AS RUAFABRICA, [FABRICA-FILIAL].N_PORTA AS PORTAFABRICA, [FABRICA-FILIAL].EMAIL AS EMAILFABRICA, [FABRICA-FILIAL].TELEFONE AS TELEFONEFABRICA, [FABRICA-FILIAL].FAX AS FAXFABRICA, N_FILIAL  FROM UTILIZADOR"
                                            + " JOIN ZONA AS ZONAUSER ON UTILIZADOR.COD_POSTAL = ZONAUSER.COD_POSTAL"
                                            + " JOIN[FABRICA-FILIAL] ON[FABRICA-FILIAL].N_FILIAL = UTILIZADOR.N_FABRICA"
                                            + " JOIN ZONA AS ZONAFABRICA ON[FABRICA-FILIAL].COD_POSTAL = ZONAFABRICA.COD_POSTAL"
                                            + " JOIN[UTILIZADOR-TIPOS] ON[UTILIZADOR-TIPOS].UTILIZADOR = UTILIZADOR.N_FUNCIONARIO"
                                            + " JOIN[TIPO-UTILIZADOR] ON[TIPO-UTILIZADOR].ID =[UTILIZADOR-TIPOS].ID_TIPO"
                                            + " WHERE N_FUNCIONARIO = 1;"
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
            this.closeSGBDConnection();
            return utilizador;
        }

    }
}
