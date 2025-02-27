using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Faculty;
using api.Models;

namespace api.Mappers
{
    public static class FacultyMapper
    {
        public static FacultyDto ToFacultyDto(this Faculty faculty) {
            return new FacultyDto {
                Id = faculty.Id,
                Name = faculty.Name
            };
        }

        public static GroupDto ToGroupDto(this Group group) {
            return new GroupDto {
                Id = group.Id,
                FacultyId = group.FacultyId,
                Number = group.Number
            };
        }
    }
}