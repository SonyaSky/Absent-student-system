using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public Guid FacultyId { get; set; }

        public Faculty Faculty {get; set; }
        public List<StudentGroup> Students { get; set; } = new List<StudentGroup>();

        public Group(string number, Guid facultyId)
        {
            Id = new Guid();
            Number = number;
            FacultyId = facultyId;
        }
    }
}