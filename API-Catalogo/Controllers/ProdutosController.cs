using API_Catalogo.Context;
using API_Catalogo.DTOs;
using API_Catalogo.Filters;
using API_Catalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Catalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }
        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutoPorPreco().ToList();
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
                var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return produtosDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return produtoDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody]ProdutoDTO produtoDto)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id,[FromBody] ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest();
                }
                var produto = _mapper.Map<Produto>(produtoDto);

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
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
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

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok(produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no sistema!");
            }
        }
    }
}
