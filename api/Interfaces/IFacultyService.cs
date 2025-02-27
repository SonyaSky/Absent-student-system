using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Faculty;
using api.Models;

namespace api.Interfaces
{
    public interface IFacultyService
    {
        Task<List<FacultyDto>> GetFacultiesAsync();
        Task<List<GroupDto>> GetGroupsAsync(Guid facultyId);
        Task<Faculty> CreateFaculty(Faculty faculty);
        Task<Group> CreateGroup(Group group);
        Task<FacultyDto?> FindFaculty(Guid facultyId);
        Task<bool> DoesFacultyExist(Guid facultyId);
        Task<bool> DoesGroupExist(Guid groupId);
    }
}