using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Absence
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Reason { get; set; } = string.Empty;
        public AbsenceStatus Status { get; set; }
        

        public List<ConfirmationFile> Files { get; set; } = new List<ConfirmationFile>();
        public Student Student { get; set; }        
    }
}