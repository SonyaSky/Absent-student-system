using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ConfirmationFile
    {
        public Guid Id { get; set; }
        public Guid AbsenceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
    }
}