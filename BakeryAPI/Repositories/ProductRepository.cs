using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(ProductVM product)
        {
            var _product = new Product()
            {
                Name = product.Name,
                Type = product.Type,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                DateAdded = DateTime.Now
            };

            _context.Products.Add(_product);
            await _context.SaveChangesAsync();

            return _product;
        }

        public async Task Delete(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync(); 
        }

        public async Task<Product> Get(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public Task<IEnumerable<Product>> Get(ProductFilterType filterType, bool descending) //work in progress
        {
            throw new NotImplementedException();
        }

        public async Task Update(int id, ProductVM product)
        {
            var _product = await _context.Products.FindAsync(id);

            if (_product != null)
            {
                _product.Name = product.Name;
                _product.Type = product.Type;
                _product.Description = product.Description;
                _product.Price = product.Price;
                _product.Quantity = product.Quantity;
                _product.DateModefied = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
    }
}
