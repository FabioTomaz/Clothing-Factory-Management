﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Cliente
    {
        private String _nif;
        private String _nome;
        private String _nib;
        private String _email;
        private String _codigoPostal;
        private String _distrito;
        private String _concelho;
        private String _localidade;
        private String _rua;
        private int _nPorta;
        private int _nCasa;
        private String _telemovel;
        private int _NCliente;

        public string Nif
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

        public string Nib
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

        public string Telemovel
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

        public int NCliente
        {
            get
            {
                return _NCliente;
            }

            set
            {
                _NCliente = value;
            }
        }

        public string Distrito
        {
            get
            {
                return _distrito;
            }

            set
            {
                _distrito = value;
            }
        }

        public string Concelho
        {
            get
            {
                return _concelho;
            }

            set
            {
                _concelho = value;
            }
        }

        public string Localidade
        {
            get
            {
                return _localidade;
            }

            set
            {
                _localidade = value;
            }
        }

        public int NPorta
        {
            get
            {
                return _nPorta;
            }

            set
            {
                _nPorta = value;
            }
        }
    }
}