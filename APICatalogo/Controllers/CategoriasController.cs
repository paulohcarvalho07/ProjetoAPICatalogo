using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Numerics;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = new List<CategoriaDTO();
            foreach(var categoria in categorias)
            {
                var categoriaDto = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };

                categoriasDto.Add(categoriaDto);
            }
                

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }

            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };


            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos");
            }

            var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novacategoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaCriada.CategoriaId,
                Nome = categoriaCriada.Nome,
                ImagemUrl = categoriaCriada.ImagemUrl
            };

            return new CreatedAtRouteResult("ObterCategoria", new { id = novacategoriaDto.CategoriaId }, novacategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos..."); 
                return BadRequest("Dados inválidos");
            }

            var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaAtualizada.CategoriaId,
                Nome = categoriaAtualizada.Nome,
                ImagemUrl = categoriaAtualizada.ImagemUrl
            };

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria == null)  
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaExcluida.CategoriaId,
                Nome = categoriaExcluida.Nome,
                ImagemUrl = categoriaExcluida.ImagemUrl
            };

            return Ok(categoriaExcluidaDto);
        }
    }
}
