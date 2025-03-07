using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Absence
{
    public class AbsenceDto
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Reason { get; set; } = string.Empty;
        public AbsenceStatus Status { get; set; }
        public List<ConfirmationFileDto> Files { get; set; } = new List<ConfirmationFileDto>();
    }
}