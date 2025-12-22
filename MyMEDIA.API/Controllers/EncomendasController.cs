using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.API.DTOs;
using MyMEDIA.Domain.Entities;
using MyMEDIA.Infraestrutura.Data;
using System.Security.Claims;

namespace MyMEDIA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // só clientes autenticados
    public class EncomendasController : ControllerBase
    {
        private readonly MyMediaDbContext _db;
        public EncomendasController(MyMediaDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EncomendaPostDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var encomenda = new Encomenda
                {
                    UserId = userId,
                    DataCriacao = DateTime.UtcNow,
                    Pago = true, // simulação
                    Total = dto.Itens.Sum(i => i.PrecoUnitario * i.Quantidade)
                };

                foreach (var item in dto.Itens)
                {
                    var prod = await _db.Produtos.FindAsync(item.ProdutoId);
                    if (prod == null || prod.Stock < item.Quantidade)
                        return BadRequest($"Stock insuficiente para produto {item.ProdutoId}");

                    prod.Stock -= item.Quantidade;

                    _db.ItensEncomenda.Add(new ItemEncomenda
                    {
                        Encomenda = encomenda,
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario
                    });
                }

                await _db.Encomendas.AddAsync(encomenda);
                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return CreatedAtAction(nameof(Get), new { id = encomenda.Id }, encomenda.Id);
            }
            catch
            {
                await tx.RollbackAsync();
                return StatusCode(500, "Erro ao processar encomenda");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var enc = await _db.Encomendas
                .Include(e => e.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(e => e.Id == id);
            return enc == null ? NotFound() : Ok(enc);
        }
    }
}