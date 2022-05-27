using BakeryAPI.Models;
using BakeryAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Repositories
{
    public enum ProductFilterType
    {
        name,
        type,
        price,
        quantity
    }

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> Get(); //get all products
        Task<Product> Get(int id); //get product by id
        Task<IEnumerable<Product>> Get(ProductFilterType filterType, bool descendingOrder);  //get products sorted by filter (while bool variable true sort in descending order)
        Task<Product> Create(ProductVM product);
        Task Update(int id, ProductVM product);
        Task Delete(int id);
    }
}
