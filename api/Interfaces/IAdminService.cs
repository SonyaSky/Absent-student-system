using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface IAdminService
    {
        Task GiveRole(string userId, Faculty faculty);
        Task<TokenResponse?> CreateAdminAsync(RegisterUserDto userDto);
        Task<Admin?> FindAdmin(string username);
        Task<User?> FindUser(string id);
        Task<User?> FindUserByEmail(string email);
    }
}