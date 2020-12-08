using System;
using System.Collections.Generic;

namespace Desafio_API.Models
{
    public class Venda
    {
        public Venda() { }
        public Venda(int id, int fornecedorId, int clienteId, decimal totalCompra, DateTime dataCompra)
        {
            this.Id = id;
            this.FornecedorId = fornecedorId;
            this.ClienteId = clienteId;
            this.totalCompra = totalCompra;
            this.dataCompra = dataCompra;

        }
        public int Id { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public decimal totalCompra { get; set; }
        public DateTime dataCompra { get; set; }
        public List<ProdutoVenda> ProdutosVendas { get; set; }
    }
}