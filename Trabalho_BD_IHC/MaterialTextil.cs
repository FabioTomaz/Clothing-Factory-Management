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
        private Fornecedor fornecedor;
        private String TipoMaterial;
        private String quantidadeSelecionada;


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

        public string TipoMaterial1
        {
            get
            {
                return TipoMaterial;
            }

            set
            {
                TipoMaterial = value;
            }
        }

        public string QuantidadeSelecionada
        {
            get
            {
                return quantidadeSelecionada;
            }

            set
            {
                quantidadeSelecionada = value;
            }
        }

    }
}
