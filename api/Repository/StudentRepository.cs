using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Faculty;
using api.Dtos.Student;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDBContext _context;
        public StudentRepository(ApplicationDBContext context, UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }
        public async Task AddGroups(List<Guid> groups, Guid id)
        {
            var newStudentGroups = groups
                .Select(g => new StudentGroup(id, g))
                .ToList();
            await _context.StudentGroup.AddRangeAsync(newStudentGroups);
            await _context.SaveChangesAsync();
        }

        public async Task<TokenResponse?> CreateStudentAsync(RegisterStudentDto registerStudentDto)
        {
            var user = registerStudentDto.ToUserFromRegisterDto();
            var createdUser = await _userManager.CreateAsync(user, registerStudentDto.Password);
            var student = new Student(user.Id);
            await _context.Students.AddAsync(student);
            await AddGroups(registerStudentDto.Groups, student.Id);
            if (createdUser.Succeeded)
            {
                return new TokenResponse
                {
                    Token = _tokenService.CreateToken(user)
                };
            } 
            else 
            {
                return null;
            }
        }

        public async Task<StudentEditProfileDto?> EditProfileAsync(User user, String username, StudentEditProfileDto editProfileDto)
        {
            user.Email = editProfileDto.Email;
            user.UserName = editProfileDto.Email;
            user.Name = editProfileDto.Name;
            user.Surname = editProfileDto.Surname;
            user.Patronymic = editProfileDto.Patronymic;
            user.PhoneNumber = editProfileDto.PhoneNumber;

            var student = await FindStudent(username);
            var groupsToDelete = await _context.StudentGroup
                .Where(s => s.StudentId.ToString() == student.Id.ToString())
                .ToListAsync();
            _context.StudentGroup.RemoveRange(groupsToDelete);
            await AddGroups(editProfileDto.Groups, student.Id);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return editProfileDto;
        }

        public async Task<List<GroupDto>> FindGroups(Guid studentId)
        {
            var groups = await _context.StudentGroup
                .Where(s => s.StudentId == studentId)
                .Select(s => s.Group.ToGroupDto()) 
                .ToListAsync();
            return groups;
        }

        public async Task<Student?> FindStudent(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);
            return student;
        }

        public async Task<User?> FindUser(string username)
        {
            var student = await _userManager.FindByNameAsync(username);
            return student;
        }

        public async Task<StudentProfileDto?> GetProfileAsync(string username)
        {
            var user = await FindUser(username);
            if (user == null)
            {
                return null;
            }
            var student = await FindStudent(username);
            if (student == null)
            {
                return null;
            }
            var groups = await FindGroups(student.Id);
            return new StudentProfileDto{
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Groups = groups
            };
        }
    }
}