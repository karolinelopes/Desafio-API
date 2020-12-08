using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Authorize(Roles = "Admin")]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private HATEOAS.HATEOAS HATEOAS;

        public ProdutosController(ApplicationDbContext db)
        {
            this.db = db;
            HATEOAS = new HATEOAS.HATEOAS("localhost:5001/api/v1/produtos");
            HATEOAS.AddAction("GET_INFO","GET");
            HATEOAS.AddAction("DELETE_PRODUTO","DELETE");
            HATEOAS.AddAction("UPDATE_PRODUTO","PUT");
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var produtos = db.Produtos.ToList();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try{
                var produto = db.Produtos.First(p => p.Id == id);
                ProdutoContainer produtoHATEOAS = new ProdutoContainer();
                produtoHATEOAS.produto = produto;
                produtoHATEOAS.links = HATEOAS.GetActions(produto.Id.ToString());
                return Ok(produtoHATEOAS);
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
            
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProdutoDTO produtoDTO)
        {
            if(ModelState.IsValid)
            {
                Produto produto = new Produto();
                produto.Name = produtoDTO.Name;
                produto.Value = produtoDTO.Value;
                produto.codProduct = produtoDTO.codProduct;
                produto.Category = produtoDTO.Category;
                produto.Promotion = produtoDTO.Promotion;
                produto.valuePromotion = produtoDTO.valuePromotion;
                produto.Image = produtoDTO.Image;
                produto.Quantity = produtoDTO.Quantity;
                produto.Fornecedor = db.Fornecedores.First(f => f.Id == produtoDTO.FornecedorId);

                db.Produtos.Add(produto);
                db.SaveChanges();

                Response.StatusCode = 201;
                return new ObjectResult("Produto cadastrado!");
            }else{
                return BadRequest(ModelState);
            }
            
        }

        [HttpPut]
        public IActionResult Put([FromBody] ProdutoDTO produtoDTO)
        {
            if(produtoDTO.Id > 0)
            {
                try{
                    var p = db.Produtos.First(ProdutoDTO => ProdutoDTO.Id == produtoDTO.Id);
                    if(p != null)
                    {
                        p.Name = produtoDTO.Name != null ? produtoDTO.Name : p.Name;
                        p.codProduct = produtoDTO.codProduct != null ? produtoDTO.codProduct : p.codProduct;
                        p.Value = produtoDTO.Value < 0 ? produtoDTO.Value : p.Value;
                        p.Promotion = produtoDTO.Promotion != false ? produtoDTO.Promotion : p.Promotion;
                        p.valuePromotion = produtoDTO.valuePromotion < 0 ? produtoDTO.valuePromotion : p.valuePromotion;
                        p.Category = produtoDTO.Category != null ? produtoDTO.Category : p.Category;
                        p.Image = produtoDTO.Image != null ? produtoDTO.Image : p.Image;
                        p.Quantity = produtoDTO.Quantity < 0 ? produtoDTO.Quantity : p.Quantity;
                        
                        db.SaveChanges();
                        return Ok();
                    }else{
                        Response.StatusCode = 404;
                        return new ObjectResult("Produto não encontrado");
                    }
                }catch{
                    Response.StatusCode = 404;
                    return new ObjectResult("Produto não encontrado");
                }
            }
            else
            {
                Response.StatusCode = 404;
                return new ObjectResult("Produto não encontrado");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try{
                Produto produto = db.Produtos.First(p => p.Id == id);
                db.Produtos.Remove(produto);
                db.SaveChanges();
                return Ok();
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Produto não encontrado");
            }
        }

        [HttpGet("asc")]
        public IActionResult Asc()
        {
            var produto = db.Produtos.OrderBy(p => p.Name).ToList();
            return Ok(produto);
        }

        [HttpGet("desc")]
        public IActionResult Desc()
        {
            var produto = db.Produtos.OrderByDescending(p => p.Name).ToList();
            return Ok(produto);
        }

        //Busca o produto por um nome específico
        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            try{
                var produto = db.Produtos.FirstOrDefault(p => p.Name.Contains(name));
                return Ok(produto);
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("Produto não encontrado");
            }
            
        }


        public class ProdutoContainer
        {
            public Produto produto { get; set; }
            public Link[] links { get; set; }
        }
        
    }
}