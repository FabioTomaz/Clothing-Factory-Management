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
        private String designacao;
        private int nFilial;
        private Localizacao localizacao;

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

  
        public int NFilial
        {
            get
            {
                return nFilial;
            }

            set
            {
                nFilial = value;
            }
        }

        public Localizacao Localizacao
        {
            get
            {
                return localizacao;
            }

            set
            {
                localizacao = value;
            }
        }
    }
}
