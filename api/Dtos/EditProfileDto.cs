using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Validations;

namespace api.Dtos
{
    public class EditProfileDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty; 
        [Required]
        [MaxLength(1000, ErrorMessage = " The field Surname must be a string or array type with a maximum length of '1000'.")]
        public string Surname { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000, ErrorMessage = " The field Name must be a string or array type with a maximum length of '1000'.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000, ErrorMessage = " The field Patronymic must be a string or array type with a maximum length of '1000'.")]
        public string Patronymic { get; set; } = string.Empty;
        [PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}