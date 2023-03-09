using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriasPaginas(CategoriasParameters categoriasParameters);
            Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
