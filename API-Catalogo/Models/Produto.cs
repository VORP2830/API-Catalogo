using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_Catalogo.Models
{
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "O nome do produto é obrigatorio")]
        [StringLength(20, ErrorMessage = "O nome deve ter entre 5 e 20 caracteres", MinimumLength = 5)]
        public string? Nome { get; set; }
        [Required(ErrorMessage ="A descrição é obrigatoria")]
        [StringLength (10, ErrorMessage = "A descrição deve ter no maximo {1} caracteres")] 
        public string? Descricao { get; set; }
        [Required(ErrorMessage = "O preço é obrigatorio")]
        [Range(1, 10000, ErrorMessage = "O preço de estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; } 
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if(primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new
                        ValidationResult("A primeira letra do produto deve ser maiuscula",
                        new[] { nameof(this.Nome)}
                        );
                }
            }

            if(this.Estoque < 0)
            {
                yield return new
                    ValidationResult("O estoque não pode ser negativo",
                    new[] { nameof(this.Nome)}
                    );
            }
        }
    }
}
