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
        Task<User> Get(int id);
        Task<IEnumerable<UserVM>> Get(UserFilterType filterType, bool descendingOrder);
        Task<IEnumerable<User>> Get(string name);
        Task<Product> AddProductToCart(int productId, int userId);
        Task Update(int id, UserVM user);
        Task Delete();
        Task Delete(int id);
    }
}
