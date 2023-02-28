using API_Catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Catalogo.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Categoria>? Categorias { get; set; }   
        public DbSet<Produto>? Produtos { get; set; }   
    }
}
