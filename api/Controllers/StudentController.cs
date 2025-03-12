using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Student;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController: ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IFacultyService _facultyService;

        public StudentController(IFacultyService facultyService, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _facultyService = facultyService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Register a new student")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentDto registerDto) {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var existingUser  = await _studentRepository.FindUser(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(
                        new Response
                        {
                            Status = "Error",
                            Message = $"Email '{registerDto.Email}' is already taken."
                        }
                    );
                }
                foreach (var group in registerDto.Groups) {
                    if (!await _facultyService.DoesGroupExist(group)) {
                        return BadRequest( new Response{
                            Status = "Error",
                            Message = $"Group with id={group} doesn't exist"
                        });
                    }
                }
                var token = await _studentRepository.CreateStudentAsync(registerDto);
                if (token == null) {
                    return BadRequest(new Response{
                        Status = "Error",
                        Message = "Something went wrong. Couldn't create a student."
                    });
                }
                return Ok(token);

            } catch (Exception e) 
            {
                return BadRequest(new Response{
                    Status = "Error",
                    Message = "Something went wrong. Couldn't create a student." + e
                });
            }
        }


        [HttpGet("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Get student's profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.GetUsername();
            if (username == null) {
                return Unauthorized();
            }
            var profile = await _studentRepository.GetProfileAsync(username);
            if (profile == null) {
                return Unauthorized();
            }
            return Ok(profile);
        }

        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Edit student's profile")]
        public async Task<IActionResult> EditProfile([FromBody] StudentEditProfileDto profileDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindUser(username);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.Email != profileDto.Email)
            {
                var existingUser  = await _studentRepository.FindUser(profileDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(
                        new Response
                        {
                            Status = null,
                            Message = $"Email '{profileDto.Email}' is already taken."
                        }
                    );
                }
            }
            foreach (var group in profileDto.Groups) {
                if (!await _facultyService.DoesGroupExist(group)) {
                    return BadRequest( new Response{
                        Status = "Error",
                        Message = $"Group with id={group} doesn't exist"
                    });
                }
            }
            var result = await _studentRepository.EditProfileAsync(user, username, profileDto);
            if (result == null)
            {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = "An Error occurred. Couldn't edit a student"
                });
            }

            return Ok(result);

        } 


    }
}