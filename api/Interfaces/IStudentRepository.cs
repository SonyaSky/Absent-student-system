using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Faculty;
using api.Models;

namespace api.Interfaces
{
    public interface IStudentRepository
    {
        Task<bool> StudentExists(string email);
        Task<TokenResponse?> CreateStudentAsync(RegisterStudentDto registerStudentDto);
        Task<TokenResponse?> LoginStudentAsync(LoginDto loginDto);
        Task<ProfileDto?> GetProfileAsync(string username);
        Task<EditProfileDto?> EditProfileAsync(User student, EditProfileDto editProfileDto);
        Task<List<GroupDto>> FindGroups(string studentId);
        Task<User?> FindStudent(string username);
        Task AddGroups(List<Guid> groups, string id);
    }
}