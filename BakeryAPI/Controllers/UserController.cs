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

        [HttpGet]
        public async Task<IEnumerable<UserVM>> Get()
        {
            return await _userRepository.Get();
        }

        [HttpPost("add-product-to-cart/{pId}/{uId}")]
        public async Task<Product> AddProductToCart(int pId, int uId)
        {
            return await _userRepository.AddProductToCart(pId, uId);
        }
        
    }
}
