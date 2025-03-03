using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Absence;
using api.Dtos.Faculty;
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
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDBContext _context;
        public StudentRepository(ApplicationDBContext context, UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task AddGroups(List<Guid> groups, string id)
        {
            var newStudentGroups = groups
                .Select(g => new StudentGroup(id, g))
                .ToList();
            await _context.StudentGroup.AddRangeAsync(newStudentGroups);
            await _context.SaveChangesAsync();
        }

        public async Task<TokenResponse?> CreateStudentAsync(RegisterStudentDto registerStudentDto)
        {
            var student = registerStudentDto.ToStudentFromRegisterDto();
            var createdStudent = await _userManager.CreateAsync(student, registerStudentDto.Password);
            if (createdStudent.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(student, "Student");
                await AddGroups(registerStudentDto.Groups, student.Id);
                if (roleResult.Succeeded) {
                    return new TokenResponse {
                        Token = _tokenService.CreateToken(student)
                    };
                }
                return null;
            } 
            else 
            {
                return null;
            }
        }

        public async Task<EditProfileDto?> EditProfileAsync(User student, EditProfileDto editProfileDto)
        {
            student.Email = editProfileDto.Email;
            student.UserName = editProfileDto.Email;
            student.Name = editProfileDto.Name;
            student.Surname = editProfileDto.Surname;
            student.Patronymic = editProfileDto.Patronymic;
            student.PhoneNumber = editProfileDto.PhoneNumber;

            var groupsToDelete = await _context.StudentGroup
                .Where(s => s.StudentId == student.Id)
                .ToListAsync();
            _context.StudentGroup.RemoveRange(groupsToDelete);
            await AddGroups(editProfileDto.Groups, student.Id);

            var result = await _userManager.UpdateAsync(student);
            if (!result.Succeeded)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return editProfileDto;
        }

        public async Task<List<GroupDto>> FindGroups(string studentId)
        {
            var groups = await _context.StudentGroup
                .Where(s => s.StudentId == studentId)
                .Select(s => s.Group.ToGroupDto()) 
                .ToListAsync();
            return groups;
        }

        public async Task<User?> FindStudent(string username)
        {
            var student = await _userManager.FindByNameAsync(username);
            return student;
        }

        public async Task<ProfileDto?> GetProfileAsync(string username)
        {
            var student = await FindStudent(username);
            if (student == null)
            {
                return null;
            }
            var groups = await FindGroups(student.Id);
            return new ProfileDto{
                Id = new Guid(student.Id),
                Name = student.Name,
                Surname = student.Surname,
                Patronymic = student.Patronymic,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Groups = groups
            };
        }

        public async Task<TokenResponse?> LoginStudentAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) 
            {
                return null;
            }

            return new TokenResponse {
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<bool> StudentExists(string email)
        {
            var existingUser  = await _userManager.FindByEmailAsync(email);
            if (existingUser == null) {
                return false;
            }
            return true;
        }
    }
}