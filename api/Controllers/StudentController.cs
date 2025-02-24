using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDBContext _context;
        private readonly ITokenService _tokenService;

        public StudentController(UserManager<User> userManager, ApplicationDBContext context, ITokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
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
                    return Ok(
                        new TokenResponse {
                        Token = _tokenService.CreateToken(student)
                    }
                    );
                } 
                else 
                {
                    return StatusCode(500, createdStudent.Errors);
                }

            } catch (Exception e) 
            {
                return StatusCode(500, e);
            }
        }
    }
}