using APICatalogo.Context;
using APICatalogo.Models;
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
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // /api/produtos/primeiro
        //[HttpGet("primeiro")]        
        //[HttpGet("/primeiro")]
        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> GetPrimeiro(string valor)
        {
            var teste = valor;
            return _context.Produtos.FirstOrDefault();                
        }

        // /api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
            if (produtos == null)
            {
                _logger.LogWarning($"Produtos não encontrados...");
                return NotFound("Produtos não encontrados...");
            }
            return produtos;           
        }

        // /api/produtos/1
        [HttpGet("{id:int:min(1)}", Name="ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery]int id)
        {                          
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    _logger.LogWarning($"Produto com id= {id} não encontrada...");
                    return NotFound($"Produto com id= {id} não encontrado...");
                }
                return produto;                                  
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

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new {id = produto.ProdutoId}, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            //var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrada...");
                return NotFound($"Produto com id= {id} não localizado...");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
