using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    class MaterialTextil
    {
        private int referencia;
        private int referenciaFornecedor;
        private String designacao;
        private String cor;
        private Fornecedor fornecedor;

        public int Referencia
        {
            get
            {
                return referencia;
            }

            set
            {
                referencia = value;
            }
        }
        public int ReferenciaFornecedor
        {
            get
            {
                return referenciaFornecedor;
            }

            set
            {
                referenciaFornecedor = value;
            }
        }
        public String Designacao
        {
            get
            {
                return designacao;
            }

            set
            {
                designacao = value;
            }
        }
        public String Cor
        {
            get
            {
                return cor;
            }

            set
            {
                cor = value;
            }
        }
        

        public Fornecedor Fornecedor
        {
            get
            {
                return fornecedor;
            }

            set
            {
                fornecedor = value;
            }
        }
    }
}
