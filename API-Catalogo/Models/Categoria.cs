using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API_Catalogo.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }
        [Key]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatorio")]
        [StringLength(30, ErrorMessage = "O nome deve ter no maximo {1} caracteres")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "É obrigatorio ter uma imagem")]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }  
        public ICollection<Produto>? Produtos { get; set; }
    }
}
