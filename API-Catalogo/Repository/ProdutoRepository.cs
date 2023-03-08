using API_Catalogo.Context;
using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }
        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return Get()
                .OrderBy(on => on.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList();
        }
        public IEnumerable<Produto> GetProdutoPorPreco()
        {
            return Get().OrderBy(x => x.Preco).ToList();
        }
    }
}
