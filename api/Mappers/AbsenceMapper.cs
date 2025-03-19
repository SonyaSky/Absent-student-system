using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Absence;
using api.Dtos.Faculty;
using api.Models;

namespace api.Mappers
{
    public static class AbsenceMapper
    {
        public static Absence ToAbsence(this CreateAbsenceDto absenceDto, Student student) {
            return new Absence {
                Id = new Guid(),
                From = absenceDto.From,
                To = absenceDto.To,
                Reason = absenceDto.Reason,
                Status = AbsenceStatus.Checking,
                StudentId = student.Id,
                Student = student,
            };
        }
        public static AbsenceDto ToAbsenceDto(this Absence absence) {
            return new AbsenceDto {
                Id = absence.Id,
                From = absence.From,
                To = absence.To,
                Reason = absence.Reason,
                Status = absence.Status,
                Files = absence.Files.Select(f => f.ToConfirmationFileDto()).ToList()
            };
        }

        public static StudentAbsenceDto ToStudentAbsenceDto(this Absence absence, User user, Student student, List<StudentGroup> groups)
        {
            var groupsDto = new List<GroupDto>();
            var facultiesDto = new List<FacultyDto>();
            var absencesDto = new List<AbsenceDto>();

            foreach (var studentGroup in groups)
            {
                groupsDto.Add(studentGroup.Group.ToGroupDto());
                facultiesDto.Add(studentGroup.Group.Faculty.ToFacultyDto());
            }



            return new StudentAbsenceDto
            {
                StudentId = student.Id,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Surname = user.Surname,
                Faculties = facultiesDto,
                Groups = groupsDto,
                Absences = { absence.ToAbsenceDto() },
            };
        }

        public static ConfirmationFile ToConfirmationFile(this CreateConfirmationFileDto fileDto, Guid absenceId) {
            return new ConfirmationFile {
                Id = new Guid(),
                AbsenceId = absenceId,
                Name = fileDto.Name,
                Description = fileDto.Description,
            };
        }

        public static ConfirmationFileDto ToConfirmationFileDto(this ConfirmationFile file) {
            return new ConfirmationFileDto {
                Id = file.Id,
                Name = file.Name,
                Description = file.Description,
                File = file.File
            };
        }
    }
}