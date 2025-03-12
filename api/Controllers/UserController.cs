using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Absence;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IFacultyService _facultyService;

        public UserController(IFacultyService facultyService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _facultyService = facultyService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Register a new user")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto) {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var existingUser  = await _userRepository.UserExists(registerDto.Email);
                if (existingUser)
                {
                    return BadRequest(
                        new Response
                        {
                            Status = "Error",
                            Message = $"Email '{registerDto.Email}' is already taken."
                        }
                    );
                }
                var token = await _userRepository.CreateUserAsync(registerDto);
                if (token == null) {
                    return BadRequest( new Response{
                        Status = "Error",
                        Message = "Something went wrong. Couldn't create a student."
                    });
                }
                return Ok(token);

            } catch (Exception e) 
            {
                return BadRequest( new Response{
                    Status = "Error",
                    Message = "Something went wrong. Couldn't create a student." + e
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login in to the system")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepository.UserExists(loginDto.Email);
            if (user == false) {
                return BadRequest(
                    new Response
                    {
                        Status = "Error",
                        Message = "Login failed"
                    }
                );
            }
            var token = await _userRepository.LoginUserAsync(loginDto);
            if (token == null) 
            {
                return BadRequest(
                    new Response
                    {
                        Status = "Error",
                        Message = "Login failed"
                    }
                );
            }

            return Ok(token);
        }

        [HttpGet("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Get user's profile (for teacher/admin)")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.GetUsername();
            if (username == null) {
                return Unauthorized();
            }
            var profile = await _userRepository.GetProfileAsync(username);
            if (profile == null) {
                return Unauthorized();
            }
            return Ok(profile);
        }

        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Edit user's profile (for teacher/admin)")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileDto profileDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _userRepository.FindUser(username);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.Email != profileDto.Email)
            {
                var existingUser  = await _userRepository.UserExists(profileDto.Email);
                if (existingUser)
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
            var result = await _userRepository.EditProfileAsync(user, profileDto);
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