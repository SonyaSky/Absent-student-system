using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public List<Role> Roles { get; set; } = new List<Role>();
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public Student Student { get; set; } = null;
        public Teacher Teacher { get; set; } = null;
        public Department Department { get; set; } = null;
    }
}