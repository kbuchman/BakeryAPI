using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public enum UserFilterType
    {
        Name,
        Role
    }

    public interface IUserRepository
    {
        Task<IEnumerable<UserVM>> Get();
        Task<UserVM> Get(int id);
        Task<IEnumerable<UserVM>> Get(UserFilterType filterType, bool descendingOrder);
        Task<IEnumerable<UserVM>> Get(string name);
        Task<Product> AddProductToCart(int productId, int userId);
        Task<UserVM> Update(int id, RegisterUserVM user);
        Task Delete();
        Task Delete(int id);
    }
}
