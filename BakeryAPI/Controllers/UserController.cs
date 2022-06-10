using BakeryAPI.Models;
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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Administrator")]
        public async Task<IEnumerable<UserVM>> Get()
        {
            return await _userRepository.Get();
        }

        [HttpGet("get/{id}")]
        [Authorize(Roles = "Client,Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public async Task<IEnumerable<UserVM>> Get(string name)
        {
            return await _userRepository.Get(name);
        }

        [HttpDelete("delete-all")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete()
        {
            if (Get() == null)
            {
                return NotFound();
            }

            await _userRepository.Delete();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            if (Get(id) == null)
            {
                return NotFound();
            }
            await _userRepository.Delete(id);
            return NoContent();
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UserVM>> Update(int id, [FromBody] UserRegisterVM user)
        {
            if (Get(id) == null)
            {
                return NotFound();
            }
            return await _userRepository.Update(id, user);
        }

        [HttpPost("add-product-to-cart/{productId}/{userId}")]
        [Authorize(Roles = "Client,Administrator")]
        public async Task<Product> AddProductToCart(int productId, int userId)
        {
            return await _userRepository.AddProductToCart(productId, userId);
        }
        
    }
}
