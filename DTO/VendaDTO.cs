using System;
using System.Collections.Generic;

namespace Desafio_API.DTO
{
    public class VendaDTO
    {
        public int Id { get; set; }
        public int FornecedorId { get; set; }
        public int ClienteId { get; set; }
        public decimal totalCompra { get; set; }
        public DateTime dataCompra { get; set; }
        public List<int> ProdutoId { get; set; }
    }
}