using API_Catalogo.Context;
using API_Catalogo.Models;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }
        public IEnumerable<Produto> GetProdutoPorPreco()
        {
            return Get().OrderBy(x => x.Preco).ToList();
        }
    }
}
