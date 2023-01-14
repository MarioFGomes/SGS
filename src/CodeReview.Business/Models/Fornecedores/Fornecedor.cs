using CodeReview.Business.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeReview.Business.Models.Produtos;

namespace CodeReview.Business.Models.Fornecedores
{
    public class Fornecedor:Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor tipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
        public ICollection<Produto> produtos { get; set; }

    }
}
