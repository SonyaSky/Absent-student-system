using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class StudentMapper
    {
        public static User ToUserFromRegisterDto(this RegisterUserDto registerDto) 
        {
            return new User
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Patronymic = registerDto.Patronymic,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };
        }
        public static User ToUserFromRegisterDto(this RegisterStudentDto registerDto) 
        {
            return new User
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Patronymic = registerDto.Patronymic,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };
        }

        public static ProfileDto ToProfileDto(this User user) 
        {
            return new ProfileDto{
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static UserDto ToUserDto(this User user) 
        {
            return new UserDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Email = user.Email,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}