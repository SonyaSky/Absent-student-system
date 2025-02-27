using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Faculty
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Guid FacultyId { get; set; }

    }
}