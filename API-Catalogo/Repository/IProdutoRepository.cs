using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutoPorPreco();
    }
}
