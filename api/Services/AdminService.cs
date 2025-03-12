using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public AdminService(ApplicationDBContext context, UserManager<User> userManager, ITokenService tokenService) 
        {
            _context = context;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse?> CreateAdminAsync(RegisterUserDto userDto)
        {
            var user = userDto.ToUserFromRegisterDto();
            user.Roles.Add(Role.Admin);
            var createdUser = await _userManager.CreateAsync(user, userDto.Password);
            if (createdUser.Succeeded)
            {
                var admin = new Admin
                {
                    Id = new Guid(),
                    UserId = user.Id,
                    User = user
                };
                await _context.Admins.AddAsync(admin);
                await _context.SaveChangesAsync();
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

        public async Task<Admin?> FindAdmin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == user.Id);
            return admin;
        }

        public async Task<User?> FindUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<User?> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task GiveRole(string userId, Faculty faculty)
        {
            var user = await FindUser(userId);
            if (!user.Roles.Contains(Role.Department)) {
                user.Roles.Add(Role.Department);
            }
            var department = _context.Departments.Any(d => d.UserId == userId && d.Faculty.Id == faculty.Id);
            if (!department)
            {
                var dep = new Department
                {
                    Id = new Guid(),
                    User = user,
                    UserId = user.Id,
                    Faculty = faculty
                };

                _context.Departments.Add(dep);
            }

            await _context.SaveChangesAsync();
        }

    }
}