using BakeryAPI.Exceptions;
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
        private readonly IAccountRepository _accountRepository;

        public UserRepository(BakeryContext context, IProductRepository productRepository, IAccountRepository accountRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Product> AddProductToCart(int productId, int userId)
        {
            var product = await _productRepository.Get(productId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _context.Carts
                .Include(x => x.Products)
                .FirstOrDefault(x => x.Id == user.CartId)
                .Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete()
        {
            var carts = await _context.Users
                .Include(x => x.Cart)
                .Select(y => y.Cart)
                .ToListAsync();

            _context.Users.RemoveRange(_context.Users.Include(x => x.Cart).Include(y => y.Cart.Products).Include(z => z.Role));
            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userToDelete = await _context.Users
                .Include(x => x.Cart)
                .Include(y => y.Cart.Products)
                .FirstOrDefaultAsync(z => z.Id == id);
            var userToDeleteCart = userToDelete.Cart;

            _context.Users.Remove(userToDelete);
            _context.Carts.Remove(userToDeleteCart);
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
                    UserId = x.Id,
                    Products = x.Cart.Products
                }
            }).ToListAsync();

            return users;
        }

        public async Task<UserVM> Get(int id)
        {
            var _user = await _context.Users
                .Include(x => x.Role)
                .Include(y => y.Cart)
                .Include(w => w.Cart.Products)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (_user != null)
            {
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
                        UserId = _user.Id,
                        Products = _user.Cart.Products
                    }
                };
                return user;
            }
            return null;
        }

        public async Task<IEnumerable<UserVM>> Get(UserFilterType filterType, bool descendingOrder)
        {
            if (descendingOrder && filterType == UserFilterType.Name)
            {
                return await _context.Users.Select(x => new UserVM()
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
                        UserId = x.Id,
                        Products = x.Cart.Products
                    }
                }).OrderByDescending(x => x.Name).ToListAsync();
            }
            else if (!descendingOrder && filterType == UserFilterType.Name)
            {
                return await _context.Users.Select(x => new UserVM()
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
                        UserId = x.Id,
                        Products = x.Cart.Products
                    }
                }).OrderBy(x => x.Name).ToListAsync();
            }
            else if (descendingOrder && filterType == UserFilterType.Role)
            {
                return await _context.Users.Select(x => new UserVM()
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
                        UserId = x.Id,
                        Products = x.Cart.Products
                    }
                }).OrderByDescending(x => x.Role).ToListAsync();
            }
            else if (!descendingOrder && filterType == UserFilterType.Role)
            {
                return await _context.Users.Select(x => new UserVM()
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
                        UserId = x.Id,
                        Products = x.Cart.Products
                    }
                }).OrderBy(x => x.Role).ToListAsync();
            }

            throw new BadRequestException("Something is not rigth!");
        }

        public async Task<IEnumerable<UserVM>> Get(string name)
        {
            var _users = await _context.Users
                .Select(x => x)
                .Include(z => z.Cart)
                .Where(y => y.Name == name)
                .ToListAsync();

            var users = _users.Select(x => new UserVM()
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
                    UserId = x.Id,
                    Products = x.Cart.Products
                }
            });

            return users;
        }

        public async Task RemoveProductFromCart(int productId, int userId)
        {
            var currentUser = await _context.Users
                .Include(x => x.Cart)
                .Include(y => y.Cart.Products)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (currentUser != null)
            {
                var product = currentUser.Cart.Products
                    .FirstOrDefault(x => x.Id == productId);

                if (product != null)
                {
                    _context.Users
                        .Include(x => x.Cart)
                        .FirstOrDefault(x => x.Id == userId)
                        .Cart
                        .Products
                        .Remove(product);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new BadRequestException("Non-existent product in user's cart.");
                }
            }
            else
            {
                throw new BadRequestException("Non-existent user.");
            }
        }

        public async Task<UserVM> Update(int id, UserRegisterVM user)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (currentUser != null)
            {
                var currentUserId = currentUser.Id;
                var currentUserDateAdded = currentUser.DateAdded;
                var currentUserCartId = currentUser.CartId;
                var currentUserCart = currentUser.Cart;

                await Delete(id);

                await _context.SaveChangesAsync();

                await _accountRepository.Register(user);

                var newUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
                newUser.Id = currentUserId;
                newUser.DateAdded = currentUserDateAdded;
                newUser.CartId = currentUserCartId;
                newUser.Cart = currentUserCart;
                newUser.DateModified = DateTime.Now;

                await _context.SaveChangesAsync();
            }

            return await Get(id);
        }

    }
}
