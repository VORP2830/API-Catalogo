using API_Catalogo.Models;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface ICategoriaRepository
    {
        interface ICategoriaRepository : IRepository<Categoria>
        {
            IEnumerable<Categoria> GetCategoriasProduto();
        }
    }
}
