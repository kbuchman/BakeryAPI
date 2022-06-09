using BakeryAPI.Exceptions;
using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BakeryContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountRepository(BakeryContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<User> Register(UserRegisterVM user)
        {
            var _user = new User()
            {
                Name = user.Name,
                Email = user.Email,
                DateAdded = DateTime.Now,
                RoleId = user.RoleId,
                Role = _context.Roles.Find(user.RoleId)
            };

            var hashedPassword = _passwordHasher.HashPassword(_user, user.Password);
            _user.PasswordHash = hashedPassword;

            _context.Users.Add(_user);
            await _context.SaveChangesAsync();

            var newUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == _user.Email);
            if (newUser.CartId == null)
            {
                _context.Carts.Add(new Cart()
                {
                    UserId = newUser.Id
                });
                await _context.SaveChangesAsync();
                var newCart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == newUser.Id);
                _context.Users.Find(newUser.Id).CartId = newCart.Id;
                _context.Users.Find(newUser.Id).Cart = _context.Carts.Find(newCart.Id);
                await _context.SaveChangesAsync();
            }
            

            return _user;
        }

        public async Task<string> Login(UserLoginVM user)
        {
            var _user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Email == user.Email);

            if (_user == null)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(_user, _user.PasswordHash, user.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{_user.Name}"),
                new Claim(ClaimTypes.Role, $"{_user.Role.Name}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
