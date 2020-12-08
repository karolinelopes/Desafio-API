using System;
using System.Linq;
using Desafio_API.Data;
using Desafio_API.DTO;
using Desafio_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_API.Controllers
{
    [Route("api/v1/[controller]")] 
    [ApiController]
    [Authorize]
    public class VendasController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public VendasController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vendas = db.Vendas.ToList();
            return Ok(vendas);
        }

        [HttpPost]
        public IActionResult Post([FromBody] VendaDTO vendaDTO)
        {
             if(ModelState.IsValid)
            {
                Venda venda = new Venda();
                venda.dataCompra = DateTime.Now;
                venda.Fornecedor = db.Fornecedores.First(v => v.Id == vendaDTO.Id);
                venda.Cliente = db.Clientes.First(v => v.Id == vendaDTO.Id);
                db.Vendas.Add(venda);
                db.SaveChanges();

                foreach(var prod in vendaDTO.ProdutoId)
                {
                    ProdutoVenda pv = new ProdutoVenda();
                    pv.Venda = db.Vendas.First(vp => vp.Id == venda.Id);
                    pv.Produto = db.Produtos.First(vp => vp.Id == prod);

                    db.ProdutosVendas.Add(pv);
                    db.SaveChanges();
                }
                

                Response.StatusCode = 201;
                return new ObjectResult("Venda realizada com sucesso!");
                
            }else{
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try{
                Venda venda = db.Vendas.First(v => v.Id == id);
                return Ok(venda);
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] VendaDTO vendaDTO)
        {
            if(vendaDTO.Id > 0)
            {
                try{
                    var v = db.Vendas.First(VendaDTO => VendaDTO.Id == vendaDTO.Id);
                    if(v != null)
                    {
                        v.dataCompra = vendaDTO.dataCompra != null ?  vendaDTO.dataCompra : v.dataCompra;
                        v.ClienteId = vendaDTO.ClienteId < 0 ? vendaDTO.ClienteId : v.ClienteId;
                        v.FornecedorId = vendaDTO.FornecedorId < 0 ? vendaDTO.FornecedorId : v.FornecedorId;
                        v.totalCompra = vendaDTO.totalCompra < 0 ? vendaDTO.totalCompra : v.totalCompra;

                        db.SaveChanges();
                        return Ok();
                    }else{
                       Response.StatusCode = 404;
                        return new ObjectResult("Venda não encontrado"); 
                    }
                }catch{
                    Response.StatusCode = 404;
                    return new ObjectResult("Venda não encontrado"); 
                }
            }
            else{
                Response.StatusCode = 404;
                return new ObjectResult("Venda não encontrado"); 
                }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try{
                Venda venda = db.Vendas.First(v => v.Id == id);
                db.Vendas.Remove(venda);
                db.SaveChanges();
                return Ok();
            }catch(Exception)
            {
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }

    }
}