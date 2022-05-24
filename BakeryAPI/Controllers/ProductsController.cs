using BakeryAPI.Models;
using BakeryAPI.Repositories;
using BakeryAPI.ViewModels;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.Get();
        }
        
        [HttpGet("{id}")]
        public async Task<Product> GetProduct(int id)
        {
            return await _productRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductVM product)
        {
            var _product = await _productRepository.Create(product);

            return CreatedAtAction(nameof(GetProduct), new { id = _product.Id }, _product);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductVM product)
        {
            if (GetProduct(id) == null)
            {
                return NotFound();
            }

            await _productRepository.Update(id, product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (GetProduct(id) == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);
            return NoContent();
        }
    }
}
