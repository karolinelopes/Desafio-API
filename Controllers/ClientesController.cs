using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Desafio_API.Data;
using Desafio_API.DTO;
using Desafio_API.HATEOAS;
using Desafio_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Desafio_API.Controllers
{
    [Route("api/v1/[controller]")] 
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private HATEOAS.HATEOAS HATEOAS;

        public ClientesController(ApplicationDbContext db)
        {
            this.db = db;
            HATEOAS = new HATEOAS.HATEOAS("localhost:5001/api/v1/clientes");
            HATEOAS.AddAction("GET_INFO","GET");
            HATEOAS.AddAction("DELETE_CLIENTE","DELETE");
            HATEOAS.AddAction("UPDATE_CLIENTE","PUT");
        }
        
        //Lista completa de clientes
        [HttpGet]
        public IActionResult Get()
        {
            var clientes = db.Clientes.ToList();
            return Ok(clientes);
        }

        //Cria um cliente
        [HttpPost]
        public IActionResult Post([FromBody] ClienteDTO clienteDTO)
        {
            if(ModelState.IsValid)
            {
                Cliente cliente = new Cliente();
                cliente.Name = clienteDTO.Name;
                cliente.Document = clienteDTO.Document;
                cliente.Email = clienteDTO.Email;
                cliente.dateRegister = DateTime.Now;
                cliente.Password = clienteDTO.Password;

                db.Clientes.Add(cliente);
                db.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("Cliente cadastrado!");
            }
            else{
                return BadRequest(ModelState);
            }
        }

        //Realizar o login do cliente
        //Para realizar o login no POST é necessário somente informar o email e a senha(email e password(como está escrito na model))
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Cliente credenciais)
        {
        
            try{
               Cliente cliente =  db.Clientes.First(user => user.Email.Equals(credenciais.Email));
               if(cliente != null){
                   if(cliente.Password.Equals(credenciais.Password)){
                       string chaveSeguranca = "Desafio-API-Chave-Ainda-Mais-Secreta";
                       var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSeguranca));
                       var credenciaisAcesso = new SigningCredentials(chaveSimetrica,SecurityAlgorithms.HmacSha256Signature);
                    
                       var claims = new List<Claim>();
                       claims.Add(new Claim("id",cliente.Id.ToString()));
                       claims.Add(new Claim("email",cliente.Email));
                       claims.Add(new Claim(ClaimTypes.Role,"Admin"));

                       var token = new JwtSecurityToken(
                           issuer: "desafioapi.com",
                           expires: DateTime.Now.AddHours(1),
                           audience: "usuario-comum",
                           signingCredentials: credenciaisAcesso,
                           claims: claims
                       );

                       return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                   }else{
                       Response.StatusCode = 401; //Não autorizado
                       return new ObjectResult("Não autorizado login");
                   }
               }else{
                   Response.StatusCode = 401; //Não autorizado
                   return new ObjectResult("Não autorizado login");
               }
            }catch(Exception){
                Response.StatusCode = 401; //Não autorizado
                return new ObjectResult("Não autorizado login");
            }
        }

        //Busca o cliente por um id específico
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try{
                Cliente cliente = db.Clientes.First(c => c.Id == id);
                ClienteContainer clienteHATEOAS = new ClienteContainer();
                clienteHATEOAS.cliente = cliente;
                clienteHATEOAS.links = HATEOAS.GetActions(cliente.Id.ToString());
                return Ok(clienteHATEOAS);
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Cliente não encontrado");
            }
        }

        //Edita um cliente
        [HttpPut]
        public IActionResult Put([FromBody] ClienteDTO clienteDTO)
        {
            if(clienteDTO.Id > 0)
            {
                try{
                    var c = db.Clientes.First(ClienteDTO => ClienteDTO.Id == clienteDTO.Id);
                    if(c != null)
                    {
                        c.Name = clienteDTO.Name != null ? clienteDTO.Name : c.Name;
                        c.Email = clienteDTO.Email != null ? clienteDTO.Email : c.Email;
                        c.Password = clienteDTO.Password != null ? clienteDTO.Password : c.Password;
                        c.Document = clienteDTO.Document != null ? clienteDTO.Document : c.Document;
                        
                        db.SaveChanges();
                        return Ok();
                    }else{
                        Response.StatusCode = 404;
                        return new ObjectResult("Cliente não encontrado");
                    }
                }catch{
                    Response.StatusCode = 404;
                    return new ObjectResult("Cliente não encontrado");
                }
            }
            else
            {
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }

        //Deleta um cliente
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try{
                Cliente cliente = db.Clientes.First(c => c.Id == id);
                db.Clientes.Remove(cliente);
                db.SaveChanges();
                return Ok();
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Cliente não encontrado");
            }
        }

        //Busca o cliente por um nome específico
        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            try{
                var cliente = db.Clientes.FirstOrDefault(c => c.Name.Contains(name));
                return Ok(cliente);
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("Cliente não encontrado");
            }
            
        }


        //Lista em ordem alfábetica
        [HttpGet("asc")]
        public IActionResult Asc()
        {
            var cliente = db.Clientes.OrderBy(c => c.Name).ToList();
            return Ok(cliente);
        }

        //Lista em ordem descrente
        [HttpGet("desc")]
        public IActionResult Desc()
        {
            var cliente = db.Clientes.OrderByDescending(c => c.Name).ToList();
            return Ok(cliente);
        }

        public class ClienteContainer
        {
            public Cliente cliente { get; set; }
            public Link[] links { get; set; }
        }
    }
}