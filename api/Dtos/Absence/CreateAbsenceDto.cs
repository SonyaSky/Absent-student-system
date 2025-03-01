using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Absence
{
    public class CreateAbsenceDto
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public string Reason { get; set; } = string.Empty;
        public List<ConfirmationFileDto> Files { get; set; } = new List<ConfirmationFileDto>();

    }
}