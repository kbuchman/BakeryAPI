using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public List<Product> Products { get; set; }
    }
}
