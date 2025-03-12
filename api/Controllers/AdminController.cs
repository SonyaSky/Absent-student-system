using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IFacultyService _facultyService;

        public AdminController(IFacultyService facultyService, IAdminService adminService)
        {
            _facultyService = facultyService;
            _adminService = adminService;
        }

        [HttpPut("role/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Give a department role")]
        public async Task<IActionResult> GiveRole([FromRoute] string id, [FromBody] Guid facultyId)
        {
            var username = User.GetUsername();
            var admin = await _adminService.FindAdmin(username);
            if (admin == null)
            {
                return Unauthorized();
            }
            var faculty = await _facultyService.DoesFacultyExist(facultyId);
            if (faculty == null) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = $"Faculty with id={facultyId} doesn't exist"
                });
            }
            var user = await _adminService.FindUser(id);
            if (user == null) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = $"User with id={id} doesn't exist"
                });
            }
            await _adminService.GiveRole(id, faculty);
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterUserDto adminDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existingUser  = await _adminService.FindUserByEmail(adminDto.Email);
            if (existingUser!= null)
            {
                return BadRequest(
                    new Response
                    {
                        Status = "Error",
                        Message = $"Email '{adminDto.Email}' is already taken."
                    }
                );
            }
            var token = await _adminService.CreateAdminAsync(adminDto);
            if (token == null) {
                return BadRequest( new Response{
                    Status = "Error",
                    Message = "Something went wrong. Couldn't create an admin."
                });
            }
            return Ok(token);
        }
    }
}