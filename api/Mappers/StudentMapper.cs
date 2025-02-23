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
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                Group = registerDto.Group,
                Faculty = registerDto.Faculty,
                Role = Role.Student
            };
        }
    }
}