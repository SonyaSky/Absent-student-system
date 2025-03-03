using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Absence;
using api.Models;

namespace api.Interfaces
{
    public interface IAbsenceService
    {
        Task<Absence> CreateAbsence(CreateAbsenceDto absenceDto, string id);
        Task AddFileToAbsence(ConfirmationFileDto fileDto, Guid absenceId);
        Task<List<AbsenceDto>?> GetAllAbsences(string id);
        Task<bool> DoesAbsenceExist(Guid id);
    }
}