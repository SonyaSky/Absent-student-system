using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace api.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController (ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("absences")]
        [Authorize]
        [SwaggerOperation(Summary = "Get student's absences")]
        public async Task<IActionResult> GetStudentAbsences(
            [Required] Guid studentId,
            [Required] int year,
            [Required] int month)
        {
            return Ok(await _teacherService.GetStudentAbsences(Request.Headers.Authorization.ToString().Replace("Bearer ", ""), studentId, year, month));
        }

        [HttpGet("list")]
        [Authorize]
        [SwaggerOperation(Summary = "Get students' absences")]
        public async Task<IActionResult> GetStudentsAbsences(
            [Required] int year,
            [Required] int month)
        {
            return Ok(await _teacherService.GetStudentList(Request.Headers.Authorization.ToString().Replace("Bearer ", ""), year, month));
        }
    }
}
