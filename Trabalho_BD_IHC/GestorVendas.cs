using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class GestorVendas : Utilizador
    {
        private Utilizador nFuncionario_vendas;

        public Utilizador NFuncionario_vendas
        {
            get
            {
                return nFuncionario_vendas;
            }

            set
            {
                nFuncionario_vendas = value;
            }
        }
    }
}
