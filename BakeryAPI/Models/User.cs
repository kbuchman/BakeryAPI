using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreationDate { get; set; }
        public int? CartId { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
