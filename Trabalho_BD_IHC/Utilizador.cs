﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace Trabalho_BD_IHC
{

    public class Utilizador
    {
        private int nFuncionario;
        private String password;
        private String nome;
        private String email;
        private String telemovel;
        private Localizacao localizacao;
        private double salario;
        private TimeSpan horaEntrada;
        private TimeSpan horaSaida;
        private filial filial;
        private List<String> tiposUser;
        private Utilizador supervisor;
        private Image imagem;
        private Boolean gestorProducao;
        private Boolean gestorEmpresa;
        private Boolean gestorVendas;
        private Boolean gestorRecursosHumanos;
        public static Utilizador loggedUser;


        public Utilizador()
        {
            this.tiposUser = new List<string>();
        }

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

        public TimeSpan HoraEntrada
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

        public TimeSpan HoraSaida
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

       

        internal Localizacao Localizacao
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

        public Utilizador Supervisor
        {
            get
            {
                return supervisor;
            }

            set
            {
                supervisor = value;
            }
        }

        public Image Imagem
        {
            get
            {
                return imagem;
            }

            set
            {
                imagem = value;
            }
        }
        public List<string> TiposUser
        {
            get
            {
                return tiposUser;
            }

            set
            {
                tiposUser = value;
            }
        }

    }
}
