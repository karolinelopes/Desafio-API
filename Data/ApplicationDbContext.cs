using System;
using System.Collections.Generic;
using Desafio_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Desafio_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ProdutoVenda> ProdutosVendas { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProdutoVenda>()
            .HasKey(x => new {x.ProdutoId, x.VendaId});

            builder.Entity<ProdutoVenda>()
            .HasOne(pv => pv.Produto)
            .WithMany(p => p.ProdutosVendas)
            .HasForeignKey(pv => pv.ProdutoId)
            .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<ProdutoVenda>()
            .HasOne(pv => pv.Venda)
            .WithMany(v => v.ProdutosVendas)
            .HasForeignKey(pv => pv.VendaId)
            .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Cliente>()
            .HasData(new List<Cliente>(){
                new Cliente(1,"Daenerys Targeryes", "dany@email.com", "Senh@",  "123.456.789-10", DateTime.Parse("2020/12/03")),
                new Cliente(2,"Aria Stark", "dany@email.com", "Senh@",  "123.456.789-10", DateTime.Parse("2020/03/16")),
                new Cliente(3,"Sansa Stark", "dany@email.com", "Senh@",  "123.456.789-10", DateTime.Parse("2020/03/11"))
            });

            builder.Entity<Fornecedor>()
            .HasData(new List<Fornecedor>(){
                new Fornecedor(1,"Asus", "12.123.321/0189-03"),
                new Fornecedor(2,"Lenovo", "21.456.608/1025-87"),
                new Fornecedor(3,"Dell", "58.103.328/1056-01")
            });

            builder.Entity<Produto>()
            .HasData(new List<Produto>(){
                new Produto(1,"Notebook", "A01", 2500, false, 0, "Eletrônicos", "asus.jpg", 10, 1),
                new Produto(2,"Computador", "A02", 3500, true, 3200, "Eletrônicos", "lenovo.jpg", 100, 2),
                new Produto(3,"Computador", "A03", 3000, true, 2900, "Eletrônicos", "dell.jpg", 50, 3)
            });

            builder.Entity<Venda>()
            .HasData(new List<Venda>(){
                new Venda(1,1,1,2500,DateTime.Parse("2020/12/03")),
                new Venda(2,2,1,3500,DateTime.Parse("2020/12/03")),
                new Venda(3,3,1,4500,DateTime.Parse("2020/03/11"))
            });
        }
    }
}