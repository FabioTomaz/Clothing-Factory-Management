using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class DesenhoBase
    {
        private int nDesenho;
        private String nome;
        private DateTime dataAlteraçao;
        private GestorProducao gestorProducao;
        private String instrucoesProducao;

        public int NDesenho
        {
            get
            {
                return nDesenho;
            }

            set
            {
                nDesenho = value;
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

        public GestorProducao GestorProducao
        {
            get
            {
                return gestorProducao;
            }

            set
            {
                gestorProducao = value;
            }
        }

        public string InstrucoesProducao
        {
            get
            {
                return instrucoesProducao;
            }

            set
            {
                instrucoesProducao = value;
            }
        }

        public DateTime DataAlteraçao
        {
            get
            {
                return dataAlteraçao;
            }

            set
            {
                dataAlteraçao = value;
            }
        }
    }
}
