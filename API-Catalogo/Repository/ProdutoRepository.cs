using API_Catalogo.Context;
using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API_Catalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }
        public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
        {
            return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
                produtosParameters.PageNumber, 
                produtosParameters.PageSize);
        }
        public async Task<IEnumerable<Produto>> GetProdutoPorPreco()
        {
            return await Get().OrderBy(x => x.Preco).ToListAsync();
        }
    }
}

