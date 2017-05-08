using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Fornecedor
    {
        private int NIF;
        private String nome;
        private int fax;
        private String email;
        private int telefone;
        private String país;
        private String localidade;
        private String codigoPostal;
        private String rua;
        private String designacao;

        public int NIF_Fornecedor
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

        public int Fax
        {
            get
            {
                return fax;
            }

            set
            {
                fax = value;
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

        public string Designacao
        {
            get
            {
                return designacao;
            }

            set
            {
                designacao = value;
            }
        }
    }
}
