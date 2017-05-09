using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_BD_IHC
{
    public class MaterialTextil
    {
        private int referencia;
        private String referenciaFornecedor;
        private String designacao;
        private String cor;
        private String fornecedor;

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
        public String ReferenciaFornecedor
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
        

        public String Fornecedor
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
