﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class Botao : AcessoriosCostura
    {
        private double diametro;

        public double Diametro
        {
            get
            {
                return diametro;
            }

            set
            {
                diametro = value;
            }
        }
    }
}
