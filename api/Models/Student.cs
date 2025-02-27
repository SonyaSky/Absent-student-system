using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Student : User
    {
        public List<StudentGroup> Groups { get; set; } = new List<StudentGroup>();
    }
}