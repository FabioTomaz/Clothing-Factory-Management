using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Fornecedor
    {
        private String NIF;
        private String nome;
        private String fax;
        private String email;
        private String telefone;
        private String país;
        private String localidade;
        private String codigoPostal;
        private String rua;
        private String designacao;
        private int nPorta;

        public String NIF_Fornecedor
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

        public String Fax
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

        public String Telefone
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
    }
}
