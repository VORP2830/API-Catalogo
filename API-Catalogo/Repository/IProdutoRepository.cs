using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutoPorPreco();
    }
}
