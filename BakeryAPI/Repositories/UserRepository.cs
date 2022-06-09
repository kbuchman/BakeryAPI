using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BakeryContext _context;
        private readonly IProductRepository _productRepository;

        public UserRepository(BakeryContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task<Product> AddProductToCart(int productId, int userId)
        {
            var product = await _productRepository.Get(productId);
            var user = await _context.Users.FindAsync(userId);
            _context.Carts.Find(user.CartId).Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserVM>> Get()
        {
            var users = await _context.Users.Select(x => new UserVM() 
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateAdded = x.DateAdded,
                DateModified = x.DateModified,
                Role = x.Role,
                Cart = new CartVM()
                {
                    Id = x.Cart.Id,
                    Products = x.Cart.Products
                }
            }).ToListAsync();

            return users;
        }

        public async Task<UserVM> Get(int id)
        {
            var _user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            var user = new UserVM()
            {
                Id = _user.Id,
                Name = _user.Name,
                Email = _user.Email,
                DateAdded = _user.DateAdded,
                DateModified = _user.DateModified,
                Role = _user.Role,
                Cart = new CartVM()
                {
                    Id = _user.Cart.Id,
                    Products = _user.Cart.Products
                }
            };
            return user;
        }

        public async Task<IEnumerable<UserVM>> Get(UserFilterType filterType, bool descendingOrder)
        {
            var users = await Get();

            if (descendingOrder && filterType == UserFilterType.Name)
            {
                users.OrderByDescending(x => x.Name);
            }
            else if (!descendingOrder && filterType == UserFilterType.Name)
            {
                users.OrderBy(x => x.Name);
            }
            else if (descendingOrder && filterType == UserFilterType.Role)
            {
                users.OrderByDescending(x => x.Role);
            }
            else if (!descendingOrder && filterType == UserFilterType.Role)
            {
                users.OrderBy(x => x.Role);
            }

            return users;
        }

        public async Task<IEnumerable<User>> Get(string name)
        {
            return await _context.Users.Select(x => x).Where(y => y.Name == name).ToListAsync();
        }

        public Task Update(int id, UserVM user)
        {
            throw new NotImplementedException();
        }

        Task<User> IUserRepository.Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
