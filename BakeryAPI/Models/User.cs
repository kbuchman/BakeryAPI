using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Models
{
    public abstract class User //still in development
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
