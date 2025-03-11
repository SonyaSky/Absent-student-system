using api.Dtos.Absence;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace api.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPut("approve")]
        [Authorize]
        [SwaggerOperation(Summary = "Approve absence")]
        public async Task<IActionResult> ApproveAbsence(
            [Required] Guid absenceId)
        {
            await _departmentService.ApproveAbsence(absenceId, Request.Headers.Authorization.ToString().Replace("Bearer ", ""));
            return Ok();
        }

        [HttpPut("reject")]
        [Authorize]
        [SwaggerOperation(Summary = "Reject absence")]
        public async Task<IActionResult> RejectAbsence(
            [Required] Guid absenceId)
        {
            await _departmentService.RejectAbsence(absenceId, Request.Headers.Authorization.ToString().Replace("Bearer ", ""));
            return Ok();
        }
    }
}
