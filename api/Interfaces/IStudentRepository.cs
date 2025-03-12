using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Faculty;
using api.Dtos.Student;
using api.Models;

namespace api.Interfaces
{
    public interface IStudentRepository
    {
        Task AddGroups(List<Guid> groups, Guid id);
        Task<List<GroupDto>> FindGroups(Guid studentId);
        Task<Student?> FindStudent(string username);
        Task<User?> FindUser(string username);
        Task<StudentProfileDto?> GetProfileAsync(string username);
        Task<TokenResponse?> CreateStudentAsync(RegisterStudentDto registerStudentDto);
        Task<StudentEditProfileDto?> EditProfileAsync(User student, String username, StudentEditProfileDto editProfileDto);
        
    }
}