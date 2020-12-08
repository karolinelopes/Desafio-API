using System.Collections.Generic;

namespace Desafio_API.Models
{
    public class Produto
    {
        public Produto() { }

        public Produto(int id,string name, string codProduct, decimal value, bool promotion, decimal valuePromotion, string category, string image, int quantity, int fornecedorId)
        {
            this.Id = id;
            this.Name = name;
            this.codProduct = codProduct;
            this.Value = value;
            this.Promotion = promotion;
            this.valuePromotion = valuePromotion;
            this.Category = category;
            this.Image = image;
            this.Quantity = quantity;
            this.FornecedorId = fornecedorId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string codProduct { get; set; }
        public decimal Value { get; set; }
        public bool Promotion { get; set; }
        public decimal valuePromotion { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public List<ProdutoVenda> ProdutosVendas { get; set; }
    }
}