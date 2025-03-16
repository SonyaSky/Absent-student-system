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
        public async Task<List<AbsenceDto>> GetStudentAbsences(string authorizationString, Guid studentId, int year, int month)
        {
            
            var teacherUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var userFound = await _userManager.FindByEmailAsync(teacherUserEmail);
            bool teacher = _context.Teachers.Any(t => t.UserId == userFound.Id);
            bool department = _context.Departments.Any(d => d.UserId == userFound.Id);

            if (!(teacher || department))
            {
                throw new Exception("You are not teacher or department worker");
            }

            var studentAbsences = await _context.Absences
                .Include(a => a.Student)
                .Where(a => a.Student.Id == studentId
                && (a.From.Year == year || a.To.Year == year || (year > a.From.Year && year < a.To.Year))
                && (a.From.Month == month || a.To.Month == month || (month > a.From.Month && month < a.To.Month)))
                .ToListAsync();

            var studentAbsenceDtos = new List<AbsenceDto>();
            foreach(var absence in studentAbsences)
            {
                studentAbsenceDtos.Add(absence.ToAbsenceDto());
            }

            return studentAbsenceDtos;
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

