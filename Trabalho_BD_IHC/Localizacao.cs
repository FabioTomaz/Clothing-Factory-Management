using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Localizacao
    {
        private String codigoPostal;
        private String distrito;
        private String concelho;
        private String localidade;
        private String Rua;
        private int porta;

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

        public string Distrito
        {
            get
            {
                return distrito;
            }

            set
            {
                distrito = value;
            }
        }

        public string Concelho
        {
            get
            {
                return concelho;
            }

            set
            {
                concelho = value;
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

        public string Rua1
        {
            get
            {
                return Rua;
            }

            set
            {
                Rua = value;
            }
        }

        public int Porta
        {
            get
            {
                return porta;
            }

            set
            {
                porta = value;
            }
        }
    }
}
