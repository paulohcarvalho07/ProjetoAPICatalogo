using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // /api/produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
            if (produtos is null)
            {
                _logger.LogWarning($"Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }
            return Ok(produtos);           
        }

        // /api/produtos/1
        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public ActionResult<Produto> Get([FromQuery]int id)
        {                          
                var produto = _repository.GetProduto(id);

                if (produto is null)
                {
                    _logger.LogWarning($"Produto com id= {id} não encontrada...");
                    return NotFound($"Produto com id= {id} não encontrado...");
                }
                return Ok(produto);                                  
        }

        // /produtos
        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto", new {id = novoProduto.ProdutoId}, novoProduto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            bool atualizado = _repository.Update(produto);

            if (atualizado)
            {
                return Ok(produto);
            }
            else
            {
                return StatusCode(500, $"Falha ao atualizar o produto de id= {id}");
            }                
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            bool deletado = _repository.Delete(id);

            if (deletado)
            {
                return Ok($"Produto de id= {id} foi excluido");
            }
            else
            {
                return StatusCode(500, $"Falha ao excluir o produto de id= {id}");
            }
        }
    }
}
