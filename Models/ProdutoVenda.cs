namespace Desafio_API.Models
{
    public class ProdutoVenda
    {
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int VendaId { get; set; }
        public Venda Venda { get; set; }

    }
}