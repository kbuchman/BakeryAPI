using BakeryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryAPI
{
    public class BakerySeeder
    {
        private readonly BakeryContext _context;

        public BakerySeeder(BakeryContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Database.CanConnect())
            {
                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>
            {
                new Role()
                {
                    Name = "Administrator"
                },
                new Role()
                {
                    Name = "Client"
                }
            };

            return roles;
        }
    }
}
