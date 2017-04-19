using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class Cliente
    {
        private int _nif;
        private String _nome;
        private String _nib;
        private String _email;
        private String _codigoPostal;
        private String _rua;
        private int _nCasa;
        private int _telemovel;

        public void adicionarClienteBaseDados() {

        }

        public int Nif
        {
            get
            {
                return _nif;
            }

            set
            {
                _nif = value;
            }
        }

        public string Nome
        {
            get
            {
                return _nome;
            }

            set
            {
                _nome = value;
            }
        }

        public String Nib
        {
            get
            {
                return _nib;
            }

            set
            {
                _nib = value;
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
            }
        }

        public string CodigoPostal
        {
            get
            {
                return _codigoPostal;
            }

            set
            {
                _codigoPostal = value;
            }
        }

        public string Rua
        {
            get
            {
                return _rua;
            }

            set
            {
                _rua = value;
            }
        }

        public int NCasa
        {
            get
            {
                return _nCasa;
            }

            set
            {
                _nCasa = value;
            }
        }

        public int Telemovel
        {
            get
            {
                return _telemovel;
            }

            set
            {
                _telemovel = value;
            }
        }

        public override String ToString()
        {
            return Nif + "   " + Nome;
        }
    }
}
      