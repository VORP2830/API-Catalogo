using API_Catalogo.Models;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutoPorPreco();
    }
}
