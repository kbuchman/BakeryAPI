using BakeryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.ViewModels
{
    public class CartVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>() { };
    }
}
