using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Models.Queries
{
    public class AbsenceQuery
    {
        [SwaggerSchema("filter by statuses of absence")]
        public List<AbsenceStatus>? Statuses { get; set; } = new List<AbsenceStatus>();
        [SwaggerSchema("sort absences be ascending date")]
        public bool? AscSorting {get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
    }
}