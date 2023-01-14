using CodeReview.Business.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview.Business.Models.Produtos
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor( Guid fornecedorId);
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedores();
        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}
