using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.API.DTOs;
using MyMEDIA.Domain.DTOs;
using MyMEDIA.Domain.Entities;
using MyMEDIA.Domain.Enums;
using MyMEDIA.Infraestrutura.Data;
using System.Security.Claims;

namespace MyMEDIA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly MyMediaDbContext _db;
        public ProdutosController(MyMediaDbContext db) => _db = db;

        // LISTAGEM PÚBLICA
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? categoriaId,
            [FromQuery] string? nome,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 12)
        {
            IQueryable<Produto> q = _db.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Where(p => p.Estado == EstadoProdutoEnum.Ativo);

            if (categoriaId.HasValue) q = q.Where(p => p.CategoriaId == categoriaId);
            if (!string.IsNullOrWhiteSpace(nome)) q = q.Where(p => p.Nome.Contains(nome));

            var total = await q.CountAsync();
            var items = await q
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    PrecoBase = p.PrecoBase,
                    PrecoFinal = p.PrecoFinal,
                    Stock = p.Stock,
                    ImagemUrl = p.ImagemUrl,
                    FornecedorId = p.FornecedorId,
                    FornecedorNome = p.Fornecedor.Nome,
                    CategoriaId = p.CategoriaId,
                    CategoriaNome = p.Categoria.Nome,
                    Estado = p.Estado.ToString()
                })
                .ToListAsync();

            return Ok(new { total, pagina, tamanho, items });
        }

        // DETALHE PÚBLICO
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _db.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .Where(p => p.Id == id && p.Estado == EstadoProdutoEnum.Ativo)
                .Select(p => new ProdutoDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    PrecoBase = p.PrecoBase,
                    PrecoFinal = p.PrecoFinal,
                    Stock = p.Stock,
                    ImagemUrl = p.ImagemUrl,
                    FornecedorId = p.FornecedorId,
                    FornecedorNome = p.Fornecedor.Nome,
                    CategoriaId = p.CategoriaId,
                    CategoriaNome = p.Categoria.Nome,
                    Estado = p.Estado.ToString()
                })
                .FirstOrDefaultAsync();

            return dto == null ? NotFound() : Ok(dto);
        }

        // CRUD PROTEGIDO ------------------------------------------------------

        [HttpPost]
        [Authorize(Roles = "Fornecedor,Admin,Funcionario")]
        public async Task<IActionResult> Post([FromBody] ProdutoCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var fornId = await _db.Users
                .Where(u => u.Id == userId && u.TipoUtilizador == "Fornecedor")
                .Select(u => u.FornecedorId)
                .FirstOrDefaultAsync();

            if (fornId == 0) return BadRequest("Fornecedor não associado.");

            var prod = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                PrecoBase = dto.PrecoBase,
                PrecoFinal = dto.PrecoBase, // admin aplica % depois
                Stock = dto.Stock,
                ImagemUrl = dto.ImagemUrl,
                CategoriaId = dto.CategoriaId,
                FornecedorId = fornId,
                Estado = EstadoProdutoEnum.Pendente
            };

            _db.Produtos.Add(prod);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = prod.Id }, prod.Id);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Fornecedor,Admin,Funcionario")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoUpdateDto dto)
        {
            var prod = await _db.Produtos.FindAsync(id);
            if (prod == null) return NotFound();

            // só permite editar os seus produtos (se for fornecedor)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            if (role == "Fornecedor" && prod.FornecedorId !=
                await _db.Users.Where(u => u.Id == userId).Select(u => u.FornecedorId).FirstOrDefaultAsync())
                return Forbid();

            prod.Nome = dto.Nome;
            prod.Descricao = dto.Descricao;
            prod.PrecoBase = dto.PrecoBase;
            prod.Stock = dto.Stock;
            prod.ImagemUrl = dto.ImagemUrl;
            prod.CategoriaId = dto.CategoriaId;
            prod.Estado = dto.Estado;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Funcionario")]
        public async Task<IActionResult> Delete(int id)
        {
            var prod = await _db.Produtos.FindAsync(id);
            if (prod == null) return NotFound();

            // não apaga se já vendido
            bool vendido = await _db.ItensEncomenda.AnyAsync(i => i.ProdutoId == id);
            if (vendido) return BadRequest("Produto já foi vendido.");

            _db.Produtos.Remove(prod);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}