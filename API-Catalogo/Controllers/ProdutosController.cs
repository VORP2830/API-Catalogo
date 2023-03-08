using API_Catalogo.Context;
using API_Catalogo.Filters;
using API_Catalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Catalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }
        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPreco()
        {
            return _uof.ProdutoRepository.GetProdutoPorPreco().ToList();
        }
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest();
                }
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok();
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
                    return NotFound("Produto não encontrado");
                }
                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
    }
}
