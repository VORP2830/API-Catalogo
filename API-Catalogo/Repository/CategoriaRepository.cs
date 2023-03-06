﻿using API_Catalogo.Context;
using API_Catalogo.Models;
using APICatalogo.Repository;
using Microsoft.EntityFrameworkCore;

namespace API_Catalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto) 
        {
        }

        public IEnumerable<Categoria> GetCategoriaPordutos() 
        {
            return Get().Include(x => x.Produtos);
        }

    }
}
