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
        private readonly BakeryContext _context;

        public ProductRepository(BakeryContext context)
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
            var productToDelete = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task Delete()
        {
            _context.Products.RemoveRange(_context.Products);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync(); 
        }

        public async Task<Product> Get(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
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
            string _phrase = "%" + phrase.ToLower() + "%";
            var _products = await _context.Products.Where(x => 
                EF.Functions.Like(x.Name.ToLower(), _phrase)
                || EF.Functions.Like(x.Type.ToLower(), _phrase)
                || EF.Functions.Like(x.Description.ToLower(), _phrase)
                ).ToListAsync();

            double phraseAsDouble;
            int phraseAsInt;
            
            try
            {
                phraseAsDouble = Convert.ToDouble(phrase);
                phraseAsInt = Convert.ToInt32(phrase);
            }
            catch (Exception)
            {

                return _products;
            }

            var _productsNum = await _context.Products.Where(x =>
                x.Price == phraseAsDouble
                || x.Quantity == phraseAsInt
                ).ToListAsync();

            var num = _productsNum
                .Where(x => !_products.Any(y => y.Id == x.Id))
                .ToList();

            _products.AddRange(num);

            return _products;
        }

        public async Task Update(int id, ProductVM product)
        { 
            var _product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

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
