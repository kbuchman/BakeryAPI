using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BakeryContext _context;

        public CartRepository(BakeryContext context)
        {
            _context = context;
        }

        public async  Task<IEnumerable<CartVM>> Get()
        {
            var carts = await _context.Carts.Select(x => new CartVM()
            {
                Id = x.Id,
                UserId = x.UserId,
                Products = x.Products
            }).ToListAsync();
            return carts;
        }

        public async Task<CartVM> Get(int id)
        {
            var _cart = await _context.Carts.FindAsync(id);
            if (_cart != null)
            {
                var cart = new CartVM()
                {
                    Id = _cart.Id,
                    UserId = _cart.UserId,
                    Products = _cart.Products
                };
                return cart;
            }
            return null;
        }
    }
}
