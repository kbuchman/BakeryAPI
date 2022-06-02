using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public interface IAccountRepository
    {
        Task<User> Create(RegisterUserVM user);
    }
}
