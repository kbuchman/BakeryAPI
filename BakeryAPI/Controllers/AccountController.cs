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
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPut("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterUserVM user)
        {
            var _user = await _accountRepository.Create(user);
            return _user;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserVM user)
        {
            string token = _accountRepository.GenerateJwt(user);
        }
    }
}
