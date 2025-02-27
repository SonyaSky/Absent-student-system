using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Faculty;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly ApplicationDBContext _context;
        public FacultyService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Faculty> CreateFaculty(Faculty faculty)
        {
            await _context.Faculties.AddAsync(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task<Group> CreateGroup(Group group)
        {
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> DoesFacultyExist(Guid facultyId)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == facultyId);
            if (faculty == null) {
                return false;
            } 
            return true;
        }

        public async Task<List<FacultyDto>> GetFacultiesAsync()
        {
            var faculties = await _context.Faculties
                .Select(f => f.ToFacultyDto())
                .ToListAsync();
            return faculties;
        }

        public async Task<List<GroupDto>> GetGroupsAsync(Guid facultyId)
        {
            var groups = await _context.Groups
                .Where(g => g.FacultyId == facultyId)
                .Select(g => g.ToGroupDto())
                .ToListAsync();
            return groups;
        }

        public async Task<FacultyDto?> FindFaculty(Guid facultyId)
        {
            var faculty = await _context.Faculties.FirstOrDefaultAsync(f => f.Id == facultyId);
            return faculty.ToFacultyDto();
        }
    }
}