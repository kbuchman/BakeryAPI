using BakeryAPI.Exceptions;
using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BakeryContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountRepository(BakeryContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Create(RegisterUserVM user)
        {
            var _user = new User()
            {
                Name = user.Name,
                Email = user.Email,
                CreationDate = DateTime.Now,
                RoleId = user.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(_user, user.Password);
            _user.PasswordHash = hashedPassword;

            _context.Users.Add(_user);
            await _context.SaveChangesAsync();

            return _user;
        }

        public async Task<string> GenerateJwt(LoginUserVM user)
        {
            var _user = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            if (_user == null)
            {
                throw new BadRequestException("Invalid email or password.");
            }

        }
    }
}
