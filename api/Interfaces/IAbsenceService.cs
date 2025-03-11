using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Absence;
using api.Models;
using api.Models.Queries;

namespace api.Interfaces
{
    public interface IAbsenceService
    {
        Task<Absence> CreateAbsence(CreateAbsenceDto absenceDto, User user);
        Task<ConfirmationFileDto> AddFileToAbsence(CreateConfirmationFileDto fileDto, Guid absenceId);
        Task DeleteFile(Guid fileId);
        Task<List<AbsenceDto>?> GetAllAbsences(string id, AbsenceQuery query);
        Task<Absence?> FindAbsence(Guid id);
        Task<ConfirmationFile?> FindFile(Guid id);
        Task<Absence?> EditAbsence(Guid id, EditAbsenceDto editAbsenceDto);
        Task DeleteAbsence(Guid id);
    }
}