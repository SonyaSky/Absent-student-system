using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Validations;

namespace api.Dtos
{
    public class RegisterStudentDto
    {
        [Required]
        public string Surname{ get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Patronymic { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}