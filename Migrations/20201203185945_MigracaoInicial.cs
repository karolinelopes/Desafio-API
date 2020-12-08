using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Desafio_API.Migrations
{
    public partial class MigracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Document = table.Column<string>(nullable: true),
                    dateRegister = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    codProduct = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false),
                    Promotion = table.Column<bool>(nullable: false),
                    valuePromotion = table.Column<decimal>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FornecedorId = table.Column<int>(nullable: false),
                    ClienteId = table.Column<int>(nullable: false),
                    totalCompra = table.Column<decimal>(nullable: false),
                    dataCompra = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendas_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosVendas",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(nullable: false),
                    VendaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosVendas", x => new { x.ProdutoId, x.VendaId });
                    table.ForeignKey(
                        name: "FK_ProdutosVendas_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProdutosVendas_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Document", "Email", "Name", "Password", "dateRegister" },
                values: new object[,]
                {
                    { 1, "123.456.789-10", "dany@email.com", "Daenerys Targeryes", "Senh@", new DateTime(2020, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "123.456.789-10", "dany@email.com", "Aria Stark", "Senh@", new DateTime(2020, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "123.456.789-10", "dany@email.com", "Sansa Stark", "Senh@", new DateTime(2020, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Fornecedores",
                columns: new[] { "Id", "CNPJ", "Name" },
                values: new object[,]
                {
                    { 1, "12.123.321/0189-03", "Asus" },
                    { 2, "21.456.608/1025-87", "Lenovo" },
                    { 3, "58.103.328/1056-01", "Dell" }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Category", "FornecedorId", "Image", "Name", "Promotion", "Quantity", "Value", "codProduct", "valuePromotion" },
                values: new object[,]
                {
                    { 1, "Eletrônicos", 1, "asus.jpg", "Notebook", false, 10, 2500m, "A01", 0m },
                    { 2, "Eletrônicos", 2, "lenovo.jpg", "Computador", true, 100, 3500m, "A02", 3200m },
                    { 3, "Eletrônicos", 3, "dell.jpg", "Computador", true, 50, 3000m, "A03", 2900m }
                });

            migrationBuilder.InsertData(
                table: "Vendas",
                columns: new[] { "Id", "ClienteId", "FornecedorId", "dataCompra", "totalCompra" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2020, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2500m },
                    { 2, 1, 2, new DateTime(2020, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3500m },
                    { 3, 1, 3, new DateTime(2020, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4500m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_FornecedorId",
                table: "Produtos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosVendas_VendaId",
                table: "ProdutosVendas",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteId",
                table: "Vendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_FornecedorId",
                table: "Vendas",
                column: "FornecedorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutosVendas");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
