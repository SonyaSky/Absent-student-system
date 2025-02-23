using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public abstract class User : IdentityUser
    {
        public Role Role {get; set;}
        public string FullName { get; set; } = string.Empty;

    }
}