using API_Catalogo.DTOs;
using API_Catalogo.Filters;
using API_Catalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_Catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try
            {
                var categorias = _uof.CategoriaRepository.Get().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                if (categorias is null || !categorias.Any())
                {
                    return NotFound("Categorias não encontradas");
                }

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(x => x.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada");
                }
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpGet("{categoriaId:int:min(1)}/produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategoriasProdutos();
                if(categorias is null)
                {
                    return NotFound("Não existem categorias");
                }
                var ctaegoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return ctaegoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                var categoriaDtoOutput = _mapper.Map<CategoriaDTO>(categoria);
                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaDtoOutput);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest($"Não foi possível atualizar o produto com id={id}");
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();

                return Ok($"Produto com id={id} foi atualizado com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound($"Produto com id={id} não encontrado.");
                }

                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                return Ok($"Produto com id={id} foi excluído com sucesso.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
    }
}
