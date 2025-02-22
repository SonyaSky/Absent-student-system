using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Student : User
    {
        public string Group { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
    }
}