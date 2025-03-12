using api.Dtos.Absence;
using api.Models;

namespace api.Interfaces
{
    public interface IDepartmentService
    {
        Task ApproveAbsence(Guid absenceId, string authorizationString);
        Task RejectAbsence(Guid absenceId, string authorizationString);
        Task GiveRole(Guid userId, string authorizationString);
    }
}
