using CodeReview.Business.Models.Produtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CodeReview.Infra.Data.Context;

namespace CodeReview.Infra.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(MeuDbContext context) : base(context) { }
        
        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            
            return await Db.Produtos.AsNoTracking().Include(f=>f.fornecedor).FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await BuscarTodos(p => p.FornecedorId == fornecedorId);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedores()
        {
          
          return await Db.Produtos.AsNoTracking().Include(f => f.fornecedor).OrderBy(p => p.Nome).ToListAsync();
        }
    }
}
