using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Domain.Entities;
using MyMEDIA.Domain.Enums;
using MyMEDIA.Identity;
using MyMEDIA.Infraestrutura.Data;

namespace MyMEDIA.API.Data
{
    public static class SeedData
    {
        public static async Task Init(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MyMediaDbContext>();
            var um = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await db.Database.MigrateAsync();
            await SeedCategorias(db);
            var fornId = await SeedFornecedor(db);
            await SeedUsers(um, fornId);
            await SeedProdutos(db, fornId);
        }

        private static async Task SeedCategorias(MyMediaDbContext db)
        {
            if (await db.Categorias.AnyAsync()) return;

            var cats = new[]
            {
                new Categoria { Nome = "Música" },
                new Categoria { Nome = "Cinema", CategoriaPaiId = 1 },
                new Categoria { Nome = "Jogos", CategoriaPaiId = 1 }
            };
            await db.Categorias.AddRangeAsync(cats);
            await db.SaveChangesAsync();
        }

        private static async Task<int> SeedFornecedor(MyMediaDbContext db)
        {
            if (await db.Fornecedores.AnyAsync())
                return await db.Fornecedores.Select(f => f.Id).FirstAsync();

            var f = new Fornecedor
            {
                Nome = "Fornecedor Teste Lda",
                Email = "forn@mail.pt",
                Telefone = "239000000"
            };
            db.Fornecedores.Add(f);
            await db.SaveChangesAsync();
            return f.Id;
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> um, int fornId)
        {
            if (await um.Users.AnyAsync()) return;

            var users = new[]
            {
                new ApplicationUser
                {
                    UserName = "cliente1",
                    Email = "cliente1@mail.pt",
                    EmailConfirmed = true,
                    NomeCompleto = "Cliente Um",
                    TipoUtilizador = "Cliente",
                    Estado = "Ativo"
                },
                new ApplicationUser
                {
                    UserName = "forn1",
                    Email = "forn1@mail.pt",
                    EmailConfirmed = true,
                    NomeCompleto = "Fornecedor Um",
                    TipoUtilizador = "Fornecedor",
                    Estado = "Ativo",
                    FornecedorId = fornId
                },
                new ApplicationUser
                {
                    UserName = "func1",
                    Email = "func1@mail.pt",
                    EmailConfirmed = true,
                    NomeCompleto = "Funcionário Um",
                    TipoUtilizador = "Funcionario",
                    Estado = "Ativo"
                },
                new ApplicationUser
                {
                    UserName = "admin1",
                    Email = "admin1@mail.pt",
                    EmailConfirmed = true,
                    NomeCompleto = "Administrador",
                    TipoUtilizador = "Admin",
                    Estado = "Ativo"
                }
            };

            foreach (var u in users)
                await um.CreateAsync(u, "123456");
        }

        private static async Task SeedProdutos(MyMediaDbContext db, int fornId)
        {
            if (await db.Produtos.AnyAsync()) return;

            var prods = new[]
            {
                new Produto
                {
                    Nome = "CD – Greatest Hits 2025",
                    Descricao = "Álbum duplo com os maiores sucessos.",
                    PrecoBase = 15,
                    PrecoFinal = 18,
                    Stock = 50,
                    ImagemUrl = "https://via.placeholder.com/300x300?text=CD",
                    CategoriaId = 1,
                    FornecedorId = fornId,
                    Estado = EstadoProdutoEnum.Ativo
                },
                new Produto
                {
                    Nome = "Blu-Ray – Interstellar",
                    Descricao = "Edição de colecionador 4K.",
                    PrecoBase = 25,
                    PrecoFinal = 30,
                    Stock = 30,
                    ImagemUrl = "https://via.placeholder.com/300x300?text=BluRay",
                    CategoriaId = 2,
                    FornecedorId = fornId,
                    Estado = EstadoProdutoEnum.Ativo
                },
                new Produto
                {
                    Nome = "Jogo – Elden Ring",
                    Descricao = "PS5 novo selado.",
                    PrecoBase = 55,
                    PrecoFinal = 65,
                    Stock = 20,
                    ImagemUrl = "https://via.placeholder.com/300x300?text=Jogo",
                    CategoriaId = 3,
                    FornecedorId = fornId,
                    Estado = EstadoProdutoEnum.Pendente
                }
            };
            await db.Produtos.AddRangeAsync(prods);
            await db.SaveChangesAsync();
        }
    }
}