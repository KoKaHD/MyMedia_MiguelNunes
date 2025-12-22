using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Domain.Entities;
using MyMEDIA.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MyMEDIA.Infraestrutura.Data
{
    public class MyMediaDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyMediaDbContext(DbContextOptions<MyMediaDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
        public DbSet<Encomenda> Encomendas => Set<Encomenda>();
        public DbSet<ItemEncomenda> ItensEncomenda => Set<ItemEncomenda>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}