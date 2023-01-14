using CodeReview.Business.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Models.Fornecedores
{
    public class Endereco:Entity
    {
        public string Morada { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Rua { get; set; }
        public string Municipio { get; set; }
        public string Numero { get; set; }

        public Guid FornecedorID { get; set; }

        public Fornecedor fornecedor { get; set; }
    }
}
