using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF.Entities
{
    public class UserEntity : IdentityUser<int>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}
