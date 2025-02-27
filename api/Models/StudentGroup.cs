using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class StudentGroup
    {
        public string StudentId { get; set; } = string.Empty;
        public Guid GroupId { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public StudentGroup(string studentId, Guid groupId)
        {
            StudentId = studentId;
            GroupId = groupId;
        }
    }
}