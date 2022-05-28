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

        public async Task<IEnumerable<Product>> Get(ProductFilterType filterType, bool descendingOrder) 
        {
            if (descendingOrder && filterType == ProductFilterType.Name)
            {
                return await _context.Products.OrderByDescending(x => x.Name).ToListAsync();
            }
            else if (descendingOrder && filterType == ProductFilterType.Price)
            {
                return await _context.Products.OrderByDescending(x => x.Price).ToListAsync();
            }
            else if (descendingOrder && filterType == ProductFilterType.Quantity)
            {
                return await _context.Products.OrderByDescending(x => x.Quantity).ToListAsync();
            }
            else if (descendingOrder && filterType == ProductFilterType.Type)
            {
                return await _context.Products.OrderByDescending(x => x.Type).ToListAsync();
            }
            else if (!descendingOrder && filterType == ProductFilterType.Name)
            {
                return await _context.Products.OrderBy(x => x.Name).ToListAsync();
            }
            else if (!descendingOrder && filterType == ProductFilterType.Price)
            {
                return await _context.Products.OrderBy(x => x.Price).ToListAsync();
            }
            else if (!descendingOrder && filterType == ProductFilterType.Quantity)
            {
                return await _context.Products.OrderBy(x => x.Quantity).ToListAsync();
            }
            else if (!descendingOrder && filterType == ProductFilterType.Type)
            {
                return await _context.Products.OrderBy(x => x.Type).ToListAsync();
            }
            else
            {
                return null;
            }

        }

        public async Task<IEnumerable<Product>> Get(string phrase)
        {
            var _products = await _context.Products.Where( //only numbers working. No idea why
                x => /*x.Name.ToLower().Contains(phrase.ToLower())
                || x.Type.ToLower().Contains(phrase.ToLower())
                || x.Description.ToLower().Contains(phrase.ToLower())
                ||*/ x.Price == Int32.Parse(phrase)
                || x.Quantity == Int32.Parse(phrase)).ToListAsync();
            return _products;
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
