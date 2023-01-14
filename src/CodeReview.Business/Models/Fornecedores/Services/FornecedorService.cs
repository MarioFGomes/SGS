using CodeReview.Business.Core.Models.Validation;
using CodeReview.Business.Core.Notificacoes;
using CodeReview.Business.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Models.Fornecedores.Services
{
    public class FornecedorService : BaseService , IFornecedorService
    {

        private readonly IFornecedorRepository _fornecedorRepositorio;
        private readonly IEnderecoRepository _enderecoRepository;


        public FornecedorService(IFornecedorRepository fornecedorRepositorio, IEnderecoRepository enderecoRepository, INotificador notificador):base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _enderecoRepository = enderecoRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            fornecedor.Endereco.Id = fornecedor.Id;
            fornecedor.Endereco.fornecedor = fornecedor;

            //if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
            //     || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            if (await FornecedorExistente(fornecedor)) return;

            await _fornecedorRepositorio.Adicionar(fornecedor);

 
            
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) ) return;

            if (await FornecedorExistente(fornecedor)) return;

            await _fornecedorRepositorio.Atualizar(fornecedor);
        }

        public async Task Remover(Guid id)
        {
            var fornecedor = await _fornecedorRepositorio.ObterFornecedorProdutosEndereco(id);

            if (fornecedor.produtos.Any())
            {
                Notificar("Fornecedor Possui Produtos Cadastrados");
                return;
            };

            if (fornecedor.Endereco != null)
            {
                await _enderecoRepository.Remover(fornecedor.Endereco.Id);
            }

            await _fornecedorRepositorio.Remover(id);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }

        private async Task<bool> FornecedorExistente(Fornecedor fornecedor)
        {
            var fornecedorAtual=await _fornecedorRepositorio.BuscarTodos(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);

            if( !fornecedorAtual.Any()) return false;

            Notificar("Já um Fornecedor com este NIF cadastrado");

            return true;
        }

       
        public void Dispose()
        {
            _fornecedorRepositorio?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}
