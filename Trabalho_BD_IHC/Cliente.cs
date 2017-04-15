using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Cliente
    {
        private int NIF;
        private String nome;
        private int NIB;
        private String email;
        private String país;
        private String localidade;
        private String codigoPostal;
        private String rua;
        private int telefone;
        private int telemovel;


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

        public string País
        {
            get
            {
                return país;
            }

            set
            {
                país = value;
            }
        }

        public string Localidade
        {
            get
            {
                return localidade;
            }

            set
            {
                localidade = value;
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

        public int Telefone
        {
            get
            {
                return telefone;
            }

            set
            {
                telefone = value;
            }
        }

        public int Telemovel
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

        public int NIF1
        {
            get
            {
                return NIF;
            }

            set
            {
                NIF = value;
            }
        }

        public int NIB1
        {
            get
            {
                return NIB;
            }

            set
            {
                NIB = value;
            }
        }
    }
}
