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
        private readonly IUnitOfWork _uof;        
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null) 
                return NotFound();

            return Ok(produtos);
        }

        // /api/produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _uof.ProdutoRepository.GetAll();
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
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

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

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

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

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            
            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return Ok(produtoDeletado);
        }
    }
}
