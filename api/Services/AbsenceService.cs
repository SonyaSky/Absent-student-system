using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Absence;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly ApplicationDBContext _context;
        private readonly IFileService _fileService;
        public AbsenceService(ApplicationDBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task AddFileToAbsence(CreateConfirmationFileDto fileDto, Guid absenceId)
        {
            var file = fileDto.ToConfirmationFile(absenceId);
            var fileName = await _fileService.SaveFileAsync(fileDto.File);
            file.File = fileName;
            await _context.ConfirmationFiles.AddAsync(file);
            await _context.SaveChangesAsync();
        }

        public async Task<Absence> CreateAbsence(CreateAbsenceDto absenceDto, string id)
        {
            var absence = absenceDto.ToAbsence(id);
            await _context.Absences.AddAsync(absence);
            await _context.SaveChangesAsync();
            return absence;
            //надо еще их в очередь ставить для деканата на проверку
        }

        public async Task DeleteFile(Guid fileId)
        {
            var file = await _context.ConfirmationFiles.FirstOrDefaultAsync(f => f.Id == fileId);
            _fileService.DeleteFile(file.File);
            _context.ConfirmationFiles.Remove(file);
            await _context.SaveChangesAsync();
        }

        public async Task<Absence?> EditAbsence(Guid id, EditAbsenceDto editAbsenceDto)
        {
            var absence = await FindAbsence(id);
            if (absence == null) {
                return null;
            }
            absence.To = editAbsenceDto.To;
            absence.Reason = editAbsenceDto.Reason;
            await _context.SaveChangesAsync();
            return absence;
        }

        public async Task<Absence?> FindAbsence(Guid id)
        {
            var absence = await _context.Absences.FirstOrDefaultAsync(a => a.Id == id);
            return absence;
        }

        public async Task<ConfirmationFile?> FindFile(Guid id)
        {
            var file = await _context.ConfirmationFiles.FirstOrDefaultAsync(f => f.Id == id);
            return file;
        }

        public async Task<List<AbsenceDto>?> GetAllAbsences(string id)
        {
            var absences = await _context.Absences
                .Where(a => a.StudentId == id)
                .Select(a => a.ToAbsenceDto())
                .ToListAsync();
            foreach (AbsenceDto a in absences) {
                var files = await _context.ConfirmationFiles
                    .Where(f => f.AbsenceId == a.Id)
                    .Select(f => f.ToConfirmationFileDto())
                    .ToListAsync();
                a.Files = files;
            }
            return absences;
        }
    }
}