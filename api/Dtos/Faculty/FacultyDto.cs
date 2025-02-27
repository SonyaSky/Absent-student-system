using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Faculty
{
    public class FacultyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}