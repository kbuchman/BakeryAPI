using BakeryAPI.Repositories;
using BakeryAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrator")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<CartVM>> Get()
        {
            return await _cartRepository.Get();
        }

        [HttpGet("get/{id}")]
        [Authorize(Roles = "Client")]
        public async Task<CartVM> Get(int id)
        {
            return await _cartRepository.Get(id);
        }
    }
}
 