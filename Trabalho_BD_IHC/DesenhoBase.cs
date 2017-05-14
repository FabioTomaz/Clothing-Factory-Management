using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Trabalho_BD_IHC
{
    public class DesenhoBase
    {
        private int nDesenho;
        private String nome;
        private DateTime dataAlteraçao;
        private Utilizador gestorProducao;
        private String instrucoesProducao;
        private ImageSource pic;
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

        public Utilizador GestorProducao
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

        public ImageSource Pic
        {
            get
            {
                return pic;
            }

            set
            {
                pic = value;
            }
        }
    }
}
