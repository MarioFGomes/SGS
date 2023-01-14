using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CodeReview.AppMvc.Extensions;

namespace CodeReview.AppMvc.ViewModels
{
    public class ProdutoViewModel
    {

      
        [Key]
        public Guid id { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatorio")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorID { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(200,ErrorMessage ="O campo {0} precisa ter entre {2} e {1} caracteres ", MinimumLength =2)]
        public string Nome { get; set; }
        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres ", MinimumLength = 2)]
        public string Descricao { get; set; }
        [DisplayName("Image do Produto")]
        public HttpPostedFileBase ImageUpload { get; set; }
        public string Image { get; set; }
        [MoedaAttribute]
        [Required(ErrorMessage =" O campo {0} é obrigatorio")]
        public decimal Valor { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        public FornecedorViewModel Fornecedor { get; set; }

        public IEnumerable<FornecedorViewModel> Fornecedores { get; set; }

        public ProdutoViewModel()
        {
            id = Guid.NewGuid();
        }
    }
}