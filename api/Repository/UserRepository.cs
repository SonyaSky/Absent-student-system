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
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDBContext _context;
        public UserRepository(ApplicationDBContext context, UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<TokenResponse?> CreateUserAsync(RegisterUserDto registerUserDto)
        {
            var user = registerUserDto.ToUserFromRegisterDto();
            var createdUser = await _userManager.CreateAsync(user, registerUserDto.Password);
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

        public async Task<EditProfileDto?> EditProfileAsync(User student, EditProfileDto editProfileDto)
        {
            student.Email = editProfileDto.Email;
            student.UserName = editProfileDto.Email;
            student.Name = editProfileDto.Name;
            student.Surname = editProfileDto.Surname;
            student.Patronymic = editProfileDto.Patronymic;
            student.PhoneNumber = editProfileDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(student);
            if (!result.Succeeded)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return editProfileDto;
        }


        public async Task<User?> FindUser(string username)
        {
            var student = await _userManager.FindByNameAsync(username);
            return student;
        }

        public async Task<List<UserDto>?> GetAllUsers()
        {
            var allUsers = await _userManager.Users.Select(u => u.ToUserDto()).ToListAsync();
            var users = new List<UserDto>();
            foreach (var user in allUsers) {
                if (!user.Roles.Contains(Role.Admin) && !(user.Roles.Contains(Role.Teacher) && user.Roles.Contains(Role.Department))) {
                    users.Add(user);
                }
            }
            return users;
        }

        public async Task<ProfileDto?> GetProfileAsync(string username)
        {
            var user = await FindUser(username);
            if (user == null)
            {
                return null;
            }
            return new ProfileDto{
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles,
            };
        }

        public async Task<TokenResponse?> LoginUserAsync(LoginDto loginDto)
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

        public async Task<bool> UserExists(string email)
        {
            var existingUser  = await _userManager.FindByEmailAsync(email);
            if (existingUser == null) {
                return false;
            }
            return true;
        }
    }
}