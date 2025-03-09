using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [Route("api/faculty")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a faculty (do not use for app)")]
        public async Task<IActionResult> CreateFaculty([FromBody] string name) {
            if (name == string.Empty)
            {
                return BadRequest();
            }
            var faculty = new Faculty()
            {
               Id = Guid.NewGuid(), 
               Name = name, 
               Groups = []
            };
            await _facultyService.CreateFaculty(faculty);
            return Ok(faculty);
        }

        [HttpPost("{id}/group")]
        [SwaggerOperation(Summary = "Create a group (do not use for app)")]
        public async Task<IActionResult> CreateGroup([FromBody] string name, [FromRoute] Guid id) {
            if (name == string.Empty)
            {
                return BadRequest();
            }
            var isIdValid = await _facultyService.DoesFacultyExist(id);
            if (!isIdValid) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = $"Faculty with id={id} doesn't exists"
                }
                );
            }
            var group = new Group(name, id);
            await _facultyService.CreateGroup(group);
            return Ok(group);
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all faculties")]
        public async Task<IActionResult> GetAllFaculties() {
            var faculties = await _facultyService.GetFacultiesAsync();
            return Ok(faculties);
        }

        [HttpGet("{id}/group")]
        [SwaggerOperation(Summary = "Get groups of a faculty")]
        public async Task<IActionResult> GetGroups([FromRoute] Guid id) {
            var isIdValid = await _facultyService.DoesFacultyExist(id);
            if (!isIdValid) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = $"Faculty with id={id} doesn't exists"
                }
                );
            }
            var groups = await _facultyService.GetGroupsAsync(id);
            return Ok(groups);
        }
    }
}