using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.API.DTOs;
using MyMEDIA.Domain.DTOs;
using MyMEDIA.Domain.Entities;
using MyMEDIA.Infraestrutura.Data;

namespace MyMEDIA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly MyMediaDbContext _db;
        public CategoriasController(MyMediaDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _db.Categorias
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    CategoriaPaiId = c.CategoriaPaiId
                })
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}/produtos")]
        public async Task<IActionResult> GetProdutos(int id)
        {
            bool existe = await _db.Categorias.AnyAsync(c => c.Id == id);
            if (!existe) return NotFound();

            var prod = await _db.Produtos
                .Where(p => p.CategoriaId == id && p.Estado == Domain.Enums.EstadoProdutoEnum.Ativo)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    PrecoFinal = p.PrecoFinal,
                    ImagemUrl = p.ImagemUrl,
                    CategoriaNome = p.Categoria.Nome
                })
                .ToListAsync();
            return Ok(prod);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Funcionario")]
        public async Task<IActionResult> Post([FromBody] CategoriaDto dto)
        {
            var cat = new Categoria
            {
                Nome = dto.Nome,
                CategoriaPaiId = dto.CategoriaPaiId
            };
            _db.Categorias.Add(cat);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cat.Id }, cat.Id);
        }
    }
}