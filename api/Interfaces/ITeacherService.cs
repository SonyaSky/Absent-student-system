using api.Dtos;
using api.Dtos.Absence;
using api.Models;

namespace api.Interfaces
{
    public interface ITeacherService
    {
        Task<StudentAbsenceDto> GetStudentAbsences(string authorizationString, Guid studentId, int year, int month);
        Task<List<StudentAbsenceDto>> GetStudentList(string authorizationString, int year, int month);
    }
}
