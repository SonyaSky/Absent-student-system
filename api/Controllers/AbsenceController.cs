using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Absence;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Models.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [ApiController]
    [Route("api/absence")]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;
        private readonly IStudentRepository _studentRepository;


        public AbsenceController(IAbsenceService absenceService, IStudentRepository studentRepository)
        {
            _absenceService = absenceService;
            _studentRepository = studentRepository;
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Create an absence")]
        public async Task<IActionResult> CreateAbsence([FromBody] CreateAbsenceDto absenceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var userRep = await _studentRepository.FindUser(username);
            if (userRep == null)
            {
                return Unauthorized();
            }
            var student = await _studentRepository.FindStudent(username);
            if (student == null)
            {
                return Unauthorized();
            }
            if (absenceDto.From > absenceDto.To) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = "The From date can't be later then To date"
                });
            }
            var absence = await _absenceService.CreateAbsence(absenceDto, userRep);
            return Ok(absence.ToAbsenceDto());
        }

        [HttpPost("{id}/file")]
        [Authorize]
        [SwaggerOperation(Summary = "Add file to an absence")]
        public async Task<IActionResult> AddFileToAbsence([FromForm] CreateConfirmationFileDto fileDto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }
            var absence = await _absenceService.FindAbsence(id);
            if (absence == null) {
                return NotFound(new Response {
                    Status = "Error",
                    Message = $"Absence with id={id} was not found in database"
                });
            }
            if (absence.Student.Id != user.Id) {
                return StatusCode(403, new Response {
                   Status = "Error",
                    Message = "You can only edit your absences" 
                });
            }

            var file = await _absenceService.AddFileToAbsence(fileDto, id);
            return Ok(file);
        }

        [HttpDelete("file/{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete file from an absence")]
        public async Task<IActionResult> DeleteFileFromAbsence([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }
            var file = await _absenceService.FindFile(id);
            if (file == null) {
                return NotFound(new Response {
                    Status = "Error",
                    Message = $"File with id={id} was not found in database"
                });
            }
            var absence = await _absenceService.FindAbsence(file.AbsenceId);
            if (absence != null && absence.Student.Id != user.Id) {
                return StatusCode(403, new Response {
                   Status = "Error",
                    Message = "You can only delete your files" 
                });
            }

            await _absenceService.DeleteFile(id);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Get all absences of a student")]
        public async Task<IActionResult> GetAbsences([FromQuery] AbsenceQuery query)
        {
            var username = User.GetUsername();
            var user = await _studentRepository.FindUser(username);
            if (user == null)
            {
                return Unauthorized();
            }
            var student = await _studentRepository.FindStudent(username);
            if (student == null)
            {
                return Unauthorized();
            }

            var absences = await _absenceService.GetAllAbsences(student.Id, query);
            return Ok(absences);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Delete absence")]
        public async Task<IActionResult> DeleteAbsence([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }
            var absence = await _absenceService.FindAbsence(id);
            if (absence == null) {
                return NotFound(new Response {
                    Status = "Error",
                    Message = $"Absence with id={id} was not found in database"
                });
            }
            if (absence.Student.Id != user.Id) {
                return StatusCode(403, new Response {
                   Status = "Error",
                    Message = "You can only edit your absences" 
                });
            }

            await _absenceService.DeleteAbsence(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Edit an absence")]
        public async Task<IActionResult> EditAbsence([FromRoute] Guid id, [FromBody] EditAbsenceDto editAbsenceDto)
        {
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }

            var absence = await _absenceService.FindAbsence(id);
            if (absence == null) {
                return NotFound(new Response {
                    Status = "Error",
                    Message = $"Absence with id={id} was not found in database"
                });
            }
            if (absence.Student.Id != user.Id) {
                return StatusCode(403, new Response {
                   Status = "Error",
                    Message = "You can only edit your absences" 
                });
            }
            var editedAbsence = await _absenceService.EditAbsence(id, editAbsenceDto);
            if (editedAbsence == null)
            {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = "Something went wrong - couldn't edit the absence"
                });
            }
            return Ok(editedAbsence.ToAbsenceDto());
        }
    }
}