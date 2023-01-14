using CodeReview.Business.Models.Fornecedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeReview.AppMvc.ViewModels
{
    public class EnderecoViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [StringLength(50,ErrorMessage ="O campo {0} precisa ter entre {2} e {1} caracteres",MinimumLength =1)]
        public string Morada { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Municipio { get; set; }
        public string Numero { get; set; }

        [HiddenInput]
        public Guid FornecedorID { get; set; }

        public EnderecoViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}