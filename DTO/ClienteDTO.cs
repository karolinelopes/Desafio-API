using System;
using System.ComponentModel.DataAnnotations;

namespace Desafio_API.DTO
{
    public class ClienteDTO
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage="Informe o nome")]
        public string Name { get; set; }
        [Required(ErrorMessage="Informe o email")]
        public string Email { get; set; }
        [Required(ErrorMessage="Informe a senha")]
        public string Password { get; set; }
        [Required(ErrorMessage="Informe o número dp CPF")]
        public string Document { get; set; }
        public DateTime dateRegister { get; set; }
    }
}