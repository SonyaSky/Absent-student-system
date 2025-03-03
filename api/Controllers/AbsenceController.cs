using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Absence;
using api.Extensions;
using api.Interfaces;
using api.Models;
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

        [HttpPost("absence")]
        [Authorize]
        [SwaggerOperation(Summary = "Create an absence")]
        public async Task<IActionResult> CreateAbsence([FromBody] CreateAbsenceDto absenceDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }
            if (absenceDto.From > absenceDto.To) {
                return BadRequest(new Response {
                    Status = "Error",
                    Message = "The From date can't be later then To date"
                });
            }
            await _absenceService.CreateAbsence(absenceDto, user.Id);
            return Created();
        }

        [HttpPost("absence/{id}/file")]
        [Authorize]
        [SwaggerOperation(Summary = "Add file to an absence")]
        public async Task<IActionResult> AddFileToAbsence([FromForm] ConfirmationFileDto fileDto, [FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }

            await _absenceService.AddFileToAbsence(fileDto, id);
            return Created();
        }

        [HttpGet("absence")]
        [Authorize]
        [SwaggerOperation(Summary = "Get all absences of a student")]
        public async Task<IActionResult> GetAbsences()
        {
            var username = User.GetUsername();
            var user = await _studentRepository.FindStudent(username);
            if (user == null)
            {
                return Unauthorized();
            }

            //здесь надо фильтры по хорошему сделать

            var absences = await _absenceService.GetAllAbsences(user.Id);
            return Ok(absences);
        }
    }
}