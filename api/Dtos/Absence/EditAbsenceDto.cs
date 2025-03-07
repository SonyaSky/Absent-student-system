using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Absence
{
    public class EditAbsenceDto
    {
        [Required]
        public DateTime To { get; set; }
        [Required]
        public string Reason { get; set; } = string.Empty;
    }
}