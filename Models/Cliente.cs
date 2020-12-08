using System;

namespace Desafio_API.Models
{
    public class Cliente
    {
        public Cliente() { }

        public Cliente(int id,string name, string email, string password, string document, DateTime dateRegister)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Document = document;
            this.dateRegister = dateRegister;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Document { get; set; }
        public DateTime dateRegister { get; set; }
    }
}