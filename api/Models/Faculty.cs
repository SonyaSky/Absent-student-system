using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Faculty
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Group> Groups { get; set; } = new List<Group>();

        public Faculty(string name)
        {
            Id = new Guid();
            Name = name;
        }
    }
}