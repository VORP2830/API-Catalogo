using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;

namespace API_Catalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasPaginas(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
