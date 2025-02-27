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
        public static Student ToStudentFromRegisterDto(this RegisterStudentDto registerDto) 
        {
            return new Student{
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Patronymic = registerDto.Patronymic,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Role = Role.Student
            };
        }

        public static ProfileDto ToProfileDto(this User student) 
        {
            return new ProfileDto{
                Id = new Guid(student.Id),
                Name = student.Name,
                Surname = student.Surname,
                Patronymic = student.Patronymic,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber
            };
        }
    }
}