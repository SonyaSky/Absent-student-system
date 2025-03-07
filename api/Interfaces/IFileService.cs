using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile);
        void DeleteFile(string fileNameWithExtension);
    }
}