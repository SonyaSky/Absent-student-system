using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Faculty;
using api.Models;

namespace api.Dtos
{
    public class ProfileDto
    {
        public string Surname { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}