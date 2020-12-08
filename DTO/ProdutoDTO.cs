using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Desafio_API.DTO
{
    public class ProdutoDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage="Informe o nome")]
        public string Name { get; set; }

        [Required(ErrorMessage="Informe o código do produto")]
        public string codProduct { get; set; }

        [Required(ErrorMessage="Informe o valor")]
        public decimal Value { get; set; }

        [Required(ErrorMessage="Informe se a promoção está ativa")]
        public bool Promotion { get; set; }

        [Required(ErrorMessage="Informe o valor da promoção")]
        public decimal valuePromotion { get; set; }

        [Required(ErrorMessage="Informe a categoria")]
        public string Category { get; set; }

        [Required(ErrorMessage="Informe a imagem")]
        public string Image { get; set; }

        [Required(ErrorMessage="Informe a quantidade")]
        public int Quantity { get; set; }

        [Required(ErrorMessage="Informe o fornecedor")]
        public int FornecedorId { get; set; }
        public List<int> VendaId { get; set; }
        
    }
        
}