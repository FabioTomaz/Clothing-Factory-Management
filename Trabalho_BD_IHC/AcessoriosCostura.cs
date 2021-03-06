﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class AcessoriosCostura : MaterialTextil
    {
        private double precoUnidade;
        private int quantidadeArmazem;
        private int quantidadeNecessaria;

        public double PrecoUnidade
        {
            get
            {
                return precoUnidade;
            }

            set
            {
                precoUnidade = value;
            }
        }

        public int QuantidadeArmazem
        {
            get
            {
                return quantidadeArmazem;
            }

            set
            {
                quantidadeArmazem = value;
            }
        }

        public int QuantidadeNecessaria
        {
            get
            {
                return quantidadeNecessaria;
            }

            set
            {
                quantidadeNecessaria = value;
            }
        }
    }
}
