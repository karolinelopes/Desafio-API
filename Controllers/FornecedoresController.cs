using System;
using System.Linq;
using Desafio_API.Data;
using Desafio_API.DTO;
using Desafio_API.HATEOAS;
using Desafio_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_API.Controllers
{
    [Route("api/v1/[controller]")] 
    [ApiController]
    [Authorize]
    public class FornecedoresController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private HATEOAS.HATEOAS HATEOAS;
        public FornecedoresController(ApplicationDbContext db)
        {
            this.db = db;
            HATEOAS = new HATEOAS.HATEOAS("localhost:5001/api/v1/forncedores");
            HATEOAS.AddAction("GET_INFO","GET");
            HATEOAS.AddAction("DELETE_FORNECEDORES","DELETE");
            HATEOAS.AddAction("UPDATE_FORNCEDORES","PUT");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var fornecedores = db.Fornecedores.ToList();
            return Ok(fornecedores);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FornecedorDTO fornecedorDTO)
        {
            if(ModelState.IsValid)
            {
                Fornecedor fornecedor = new Fornecedor();
                fornecedor.Name = fornecedorDTO.Name;
                fornecedor.CNPJ = fornecedorDTO.CNPJ;

                db.Fornecedores.Add(fornecedor);
                db.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("Fornecedor cadastrado!");
            }
            else{
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try{
                var fornecedor = db.Fornecedores.First(f => f.Id == id);
                FornecedorContainer fornecedorHATEOAS = new FornecedorContainer();
                fornecedorHATEOAS.fornecedor = fornecedor;
                fornecedorHATEOAS.links = HATEOAS.GetActions(fornecedor.Id.ToString());
                return Ok(fornecedorHATEOAS);
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Fornecedor não encontrado");
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try{
                Fornecedor fornecedor = db.Fornecedores.First(f => f.Id == id);
                db.Fornecedores.Remove(fornecedor);
                db.SaveChanges();
                return Ok();
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Fornecedor não encontrado");
            }
        }

        [HttpGet("asc")]
        public IActionResult Asc()
        {
            var fornecedor = db.Fornecedores.OrderBy(f => f.Name).ToList();
            return Ok(fornecedor);
        }

        [HttpGet("desc")]
        public IActionResult Desc()
        {
            var fornecedor = db.Fornecedores.OrderByDescending(f => f.Name).ToList();
            return Ok(fornecedor);
        }

        //Busca o fornecedor por um nome específico
        [HttpGet("name/{name}")]
         public IActionResult GetByName(string name)
        {
            try{
                var fornecedor = db.Fornecedores.FirstOrDefault(f => f.Name.Contains(name));
                return Ok(fornecedor);
            }catch(Exception){
                Response.StatusCode = 404;
                return new ObjectResult("Fornecedor não encontrado");
            }
            
        }

           public class FornecedorContainer
        {
            public Fornecedor fornecedor { get; set; }
            public Link[] links { get; set; }
        }
    }
}