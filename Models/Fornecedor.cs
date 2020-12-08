using System.Collections.Generic;

namespace Desafio_API.Models
{
    public class Fornecedor
    {
        public Fornecedor() { }
        public Fornecedor(int id,string name, string cnpj)
        {
            this.Id = id;
            this.Name = name;
            this.CNPJ = cnpj;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public virtual List<Produto> Produtos { get; set; }
    }
}