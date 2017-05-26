using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class filial
    {
        private int nFilial;
        private string fax;
        private string email;
        private string telefone;
        private Localizacao localizacao;
        private Utilizador chefe;


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

        public string Fax
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

        public string Telefone
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

        public Utilizador Chefe
        {
            get
            {
                return chefe;
            }

            set
            {
                chefe = value;
            }
        }
    }
}
