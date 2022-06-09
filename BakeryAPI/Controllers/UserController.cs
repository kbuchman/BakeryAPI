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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<UserVM>> Get()
        {
            return await _userRepository.Get();
        }

        [HttpGet("get/{id}")]
        public async Task<UserVM> Get(int id)
        {
            return await _userRepository.Get(id);
        }

        [HttpGet("get-all-sorted/{filterType}/{descendingOrder}")]
        public async Task<IEnumerable<UserVM>> Get(UserFilterType filterType, bool descendingOrder)
        {
            return await _userRepository.Get(filterType, descendingOrder);
        }

        [HttpGet("get-by-name/{name}")]
        public async Task<IEnumerable<UserVM>> Get(string name)
        {
            return await _userRepository.Get(name);
        }

        [HttpDelete("delete")]
        public async Task Delete()
        {
            await _userRepository.Delete();
        }

        [HttpDelete("delete/{id}")]
        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }

        [HttpPut("update/{id}")]
        public async Task<UserVM> Update(int id, [FromBody] UserRegisterVM user)
        {
            return await _userRepository.Update(id, user);
        }

        [HttpPost("add-product-to-cart/{productId}/{userId}")] //something not rigth
        public async Task<Product> AddProductToCart(int productId, int userId)
        {
            return await _userRepository.AddProductToCart(productId, userId);
        }
        
    }
}
