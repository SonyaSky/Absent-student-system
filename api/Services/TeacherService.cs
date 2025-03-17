using api.Data;
using api.Dtos;
using api.Dtos.Absence;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        public TeacherService(ApplicationDBContext context, UserManager<User> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<StudentAbsenceDto> GetStudentAbsences(string authorizationString, Guid studentId, int year, int month)
        {
            var teacherUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var userFound = await _userManager.FindByEmailAsync(teacherUserEmail);
            bool teacher = _context.Teachers.Any(t => t.UserId == userFound.Id);
            bool department = _context.Departments.Any(d => d.UserId == userFound.Id);

            if (!(teacher || department))
            {
                throw new Exception("You are not teacher or department worker");
            }

            var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                throw new Exception("Student not found");
            }
            
            var studentAbsences = await _context.Absences.Include(a => a.Student)
                .ThenInclude(s => s.Groups)
                .ThenInclude(g => g.Group).ThenInclude(g => g.Faculty)
                .Include(s => s.Student)
                .ThenInclude(s => s.User).AsSplitQuery()
                .Where(a => (year >= a.From.Year && year <=  a.To.Year)
                            && (month >= a.From.Month && month <= a.To.Month) && a.StudentId == studentId).OrderBy(a => a.From).ToListAsync();

            var transformedAbsentStudentDto = new StudentAbsenceDto()
            {
                Absences = studentAbsences.Select(a => a.ToAbsenceDto())
                    .ToList(),
                UserId = student.UserId,
                StudentId = student.Id,
                Name = student.User.Name,
                Surname = student.User.Surname,
                Patronymic = student.User.Patronymic,
                Faculties = student.Groups.Select(g => g.Group.Faculty.ToFacultyDto()).ToList(),
                Groups = student.Groups.Select(g => g.Group.ToGroupDto()).ToList()
            };

            return transformedAbsentStudentDto;
        }

        public async Task<List<StudentAbsenceDto>> GetStudentList(string authorizationString, int year, int month)
        {
            var teacherUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var userFound = await _userManager.FindByEmailAsync(teacherUserEmail);
            bool teacher = _context.Teachers.Any(t => t.UserId == userFound.Id);
            bool department = _context.Departments.Any(d => d.UserId == userFound.Id);

            if (!(teacher || department))
            {
                throw new Exception("You are not teacher or department worker");
            }

            var absentStudents = await _context.Absences.Include(a => a.Student)
                .ThenInclude(s => s.Groups)
                .ThenInclude(g => g.Group).ThenInclude(g => g.Faculty)
                .Include(s => s.Student)
                .ThenInclude(s => s.User).AsSplitQuery()
                .Where(a => (year >= a.From.Year && year <=  a.To.Year)
                && (month >= a.From.Month && month <= a.To.Month))
                .ToListAsync();

            var absentStudentsDtos = new List<StudentAbsenceDto>();

            foreach (var absentStudent in absentStudents)
            {
                absentStudentsDtos.Add(absentStudent.ToStudentAbsenceDto(absentStudent.Student.User, absentStudent.Student, absentStudent.Student.Groups));
            }

            var transformedAbsentStudentsDtos = absentStudentsDtos
            .GroupBy(s => new { s.StudentId, s.Name, s.Surname, s.Patronymic })
            .Select(g => new StudentAbsenceDto
            {
                StudentId = g.Key.StudentId,
                Name = g.Key.Name,
                Surname = g.Key.Surname,
                Patronymic = g.Key.Patronymic,
                Faculties = g.First().Faculties, 
                Groups = g.First().Groups,      
                Absences = g.Select(s => s.Absences[0]).ToList() 
            })
            .ToList();

            return transformedAbsentStudentsDtos;
        }
    }
}

