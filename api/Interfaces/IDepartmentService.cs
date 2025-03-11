using api.Dtos.Absence;

namespace api.Interfaces
{
    public interface IDepartmentService
    {
        Task ApproveAbsence(Guid absenceId, string authorizationString);
        Task RejectAbsence(Guid absenceId, string authorizationString);
    }
}
