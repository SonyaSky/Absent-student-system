using api.Dtos.Absence;
using api.Dtos.Faculty;

namespace api.Dtos
{
    public class StudentAbsenceDto
    {
        public Guid StudentId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public List<FacultyDto> Faculties { get; set; } = new List<FacultyDto>();
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public List<AbsenceDto> Absences { get; set; } = new List<AbsenceDto>();
    }
}
