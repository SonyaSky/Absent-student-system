using api.Data;
using api.Dtos.Absence;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;

        public DepartmentService(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task ApproveAbsence(Guid absenceId, string authorizationString)
        {
            var departmentUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var userFound = await _userManager.FindByEmailAsync(departmentUserEmail);
            bool department = _context.Departments.Any(d => d.UserId == userFound.Id);

            if (!department)
            {
                throw new Exception("You are not department worker");
            }

            var absentFound = _context.Absences.FirstOrDefault(a => a.Id == absenceId)!;

            if (absentFound == null)
            {
                throw new Exception("ABsent does not exist");
            }

            absentFound.Status = AbsenceStatus.Approved;

            await _context.SaveChangesAsync();
        }

        public async Task RejectAbsence(Guid absenceId, string authorizationString)
        {
            var departmentUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var userFound = await _userManager.FindByEmailAsync(departmentUserEmail);
            bool department = _context.Departments.Any(d => d.UserId == userFound.Id);

            if (!department)
            {
                throw new Exception("You are not department worker");
            }

            var absentFound = _context.Absences.FirstOrDefault(a => a.Id == absenceId)!;

            if (absentFound == null)
            {
                throw new Exception("ABsent does not exist");
            }

            absentFound.Status = AbsenceStatus.Rejected;

            await _context.SaveChangesAsync();
        }

        public async Task GiveRole(Guid userId, string authorizationString)
        {
            var departmentUserEmail = TokenService.GetUserIdFromToken(authorizationString);
            var departmentFound = await _userManager.FindByEmailAsync(departmentUserEmail);
            bool department = _context.Departments.Any(d => d.UserId == departmentFound.Id);

            if (!department)
            {
                throw new Exception("You are not department worker");
            }

            var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
            if (userFound == null)
            {
                throw new Exception("This user does not exist");
            }

            if (!userFound.Roles.Contains(Role.Teacher))
            {
                userFound.Roles.Add(Role.Teacher);
            }

            var teacherExists = _context.Teachers.Any(t => t.UserId == userId.ToString());
            if (!teacherExists)
            {
                var teacher = new Teacher
                {
                    Id = new Guid(),
                    User = userFound,
                    UserId = userFound.Id
                };

                userFound.Teacher = teacher;
                _context.Teachers.Add(teacher);
            }

            await _context.SaveChangesAsync();
        }
    }
}
