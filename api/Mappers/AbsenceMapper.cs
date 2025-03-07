using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Absence;
using api.Models;

namespace api.Mappers
{
    public static class AbsenceMapper
    {
        public static Absence ToAbsence(this CreateAbsenceDto absenceDto, string studentId) {
            return new Absence {
                Id = new Guid(),
                From = absenceDto.From,
                To = absenceDto.To,
                Reason = absenceDto.Reason,
                Status = AbsenceStatus.Checking,
                StudentId = studentId
            };
        }
        public static AbsenceDto ToAbsenceDto(this Absence absence) {
            return new AbsenceDto {
                Id = absence.Id,
                From = absence.From,
                To = absence.To,
                Reason = absence.Reason,
                Status = absence.Status,
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