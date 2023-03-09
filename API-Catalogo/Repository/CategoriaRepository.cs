using API_Catalogo.Context;
using API_Catalogo.Models;
using API_Catalogo.Pagination;
using APICatalogo.Repository;
using Microsoft.EntityFrameworkCore;

namespace API_Catalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto) 
        {
        }

        public PagedList<Categoria> GetCategoriasPaginas(CategoriasParameters categoriasParameters)
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(x => x.Nome),
                categoriasParameters.PageNumber,
                categoriasParameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos).ToList();
        }

    }
}
