using API_Catalogo.Models;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
            Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
