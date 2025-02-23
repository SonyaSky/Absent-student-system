using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController: ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public StudentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }
    }
}