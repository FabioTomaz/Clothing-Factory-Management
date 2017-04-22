using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Utilizador
    {
        private int nFuncionario;
        private String password;
        private String nome;
        private double salario;
        private String horaEntrada;
        private String horaSaida;
        private filial filial;

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

        public string HoraEntrada
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

        public string HoraSaida
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

        internal filial Filial
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
    }
}
