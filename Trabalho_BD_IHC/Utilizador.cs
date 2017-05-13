using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{

    public class Utilizador
    {
        private int nFuncionario;
        private String password;
        private String nome;
        private String email;
        private String telemovel;
        private String codigoPostal;
        private String rua;
        private int nPorta;
        private double salario;
        private DateTime horaEntrada;
        private DateTime horaSaida;
        private filial filial;
        private String tipoUser;
        public static Utilizador loggedUser;
        public int NFuncionario
        {
            get
            {
                return nFuncionario;
            }

            set
            {
                nFuncionario = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Nome
        {
            get
            {
                return nome;
            }

            set
            {
                nome = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Telemovel
        {
            get
            {
                return telemovel;
            }

            set
            {
                telemovel = value;
            }
        }

        public string CodigoPostal
        {
            get
            {
                return codigoPostal;
            }

            set
            {
                codigoPostal = value;
            }
        }

        public string Rua
        {
            get
            {
                return rua;
            }

            set
            {
                rua = value;
            }
        }

        public int NPorta
        {
            get
            {
                return nPorta;
            }

            set
            {
                nPorta = value;
            }
        }

        public double Salario
        {
            get
            {
                return salario;
            }

            set
            {
                salario = value;
            }
        }

        public DateTime HoraEntrada
        {
            get
            {
                return horaEntrada;
            }

            set
            {
                horaEntrada = value;
            }
        }

        public DateTime HoraSaida
        {
            get
            {
                return horaSaida;
            }

            set
            {
                horaSaida = value;
            }
        }

        public filial Filial
        {
            get
            {
                return filial;
            }

            set
            {
                filial = value;
            }
        }

        public String TipoUser
        {
            get
            {
                return tipoUser;
            }

            set
            {
                tipoUser = value;
            }
        }
    }
}
