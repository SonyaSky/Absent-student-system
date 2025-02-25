using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public StudentController(UserManager<User> userManager, IEmailService emailService, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentDto registerDto) {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var existingUser  = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser  != null)
                {
                    return BadRequest(
                        new Response
                        {
                            Status = "Error",
                            Message = $"Email '{registerDto.Email}' is already taken."
                        }
                    );
                }
                var student = registerDto.ToStudentFromRegisterDto();
                var createdStudent = await _userManager.CreateAsync(student, registerDto.Password);
                if (createdStudent.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(student, "Student");
                    //await _emailService.SendEmail(student.Email);
                    return Ok(
                        new TokenResponse {
                        Token = _tokenService.CreateToken(student)
                    });
                } 
                else 
                {
                    return StatusCode(500, createdStudent.Errors);
                }

            } catch (Exception e) 
            {
                return BadRequest( new Response{
                    Status = "Error",
                    Message = e.Message
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user == null) {
                return BadRequest(
                    new Response
                    {
                        Status = "Error",
                        Message = "Login failed"
                    }
                );
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) 
            {
                return BadRequest(
                    new Response
                    {
                        Status = "Error",
                        Message = "Login failed"
                    }
                );
            }

            return Ok(
                new TokenResponse
                {
                    Token = _tokenService.CreateToken(user)
                }
            );
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.GetUsername();
            if (username == null) {
                return Unauthorized();
            }
            var student = await _userManager.FindByNameAsync(username);
            if (student == null)
            {
                return Unauthorized();
            }
            return Ok(new ProfileDto{
                Id = new Guid(student.Id),
                Name = student.Name,
                Surname = student.Surname,
                Patronymic = student.Patronymic,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
            });
        }

        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation(Summary = "Edit user profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileDto profileDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.Email != profileDto.Email)
            {
                var existingUser  = await _userManager.FindByEmailAsync(profileDto.Email);
                if (existingUser  != null)
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
            user.Email = profileDto.Email;
            user.UserName = profileDto.Email;
            user.Name = profileDto.Name;
            user.Surname = profileDto.Surname;
            user.Patronymic = profileDto.Patronymic;
            user.PhoneNumber = profileDto.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();

        } 

    }
}