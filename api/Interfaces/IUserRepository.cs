using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Absence;
using api.Dtos.Faculty;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email);
        Task<TokenResponse?> CreateUserAsync(RegisterUserDto registerStudentDto);
        Task<TokenResponse?> LoginUserAsync(LoginDto loginDto);
        Task<ProfileDto?> GetProfileAsync(string username);
        Task<EditProfileDto?> EditProfileAsync(User student, EditProfileDto editProfileDto);
        Task<List<GroupDto>> FindGroups(Guid studentId);
        Task<User?> FindUser(string username);
        //Task AddGroups(List<Guid> groups, string id);
    }
}