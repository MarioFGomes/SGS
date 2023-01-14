using CodeReview.Business.Core.Validation.Documents;
using CodeReview.Business.Models.Fornecedores;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Core.Models.Validation
{
    public class FornecedorValidation: AbstractValidator<Fornecedor>
    {
       public FornecedorValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(f => f.tipoFornecedor == TipoFornecedor.Pessoa, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(ValidacaoDocs.TamanhoCpf)
                .WithMessage("O Campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}");

                RuleFor(f => ValidacaoDocs.Validar(f.Documento)).Equal(true)
              .WithMessage("O Documento Fornecido é Invalido");
            });

            When(f => f.tipoFornecedor == TipoFornecedor.Entidade, () =>
            {
                RuleFor(f => f.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
                .WithMessage("O Campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}");

                RuleFor(f => CnpjValidacao.Validar(f.Documento)).Equal(true)
              .WithMessage("O Documento Fornecido é Invalido");

            });
        }

    }
}
