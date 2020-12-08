using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Desafio_API.DTO
{
    public class FornecedorDTO
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage="Informe o nome")]
        public string Name { get; set; }
        [Required(ErrorMessage="Informe o CNPJ")]
        public string CNPJ { get; set; }
        public List<int> ProdutoId { get; set; }
    }
}