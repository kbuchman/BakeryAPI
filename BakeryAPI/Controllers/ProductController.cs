using BakeryAPI.Models;
using BakeryAPI.Repositories;
using BakeryAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get-all")]
        //[AllowAnonymous]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _productRepository.Get();
        }
        
        [HttpGet("get/{id}")]
        public async Task<Product> Get(int id)
        {
            return await _productRepository.Get(id);
        }

        [HttpGet("get-all-sorted/{filterType}/{descendingOrder}")]
        public async Task<IEnumerable<Product>> Get(ProductFilterType filterType, bool descendingOrder)
        {
            return await _productRepository.Get(filterType, descendingOrder); 
        }

        [HttpGet("get-any-with/{phrase}")]
        public async Task<IEnumerable<Product>> Get(string phrase)
        {
            return await _productRepository.Get(phrase);  
        }


        [HttpPost("add")]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Product>> Create([FromBody] ProductVM product)
        { 
            var _product = await _productRepository.Create(product);

            return CreatedAtAction(nameof(Get), new { id = _product.Id }, _product);
        }

        [HttpPut("update/{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductVM product)
        {
            if (Get(id) == null)
            {
                return NotFound();
            }

            await _productRepository.Update(id, product);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            if (Get(id) == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);
            return NoContent();
        }
    }
}
