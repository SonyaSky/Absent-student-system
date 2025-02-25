using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public EmailController(UserManager<User> userManager, IEmailService emailService, ITokenService tokenService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmEmail(EmailCode emailCode) {
            var confirmation = await _emailService.ConfirmCode(emailCode);
            if (confirmation == false) {
                return BadRequest(new Response
                {
                    Status = "Error",
                    Message = "Wrong code"
                });
            }
            var user = await _userManager.FindByEmailAsync(emailCode.Email);
            if (user == null) {
                return NotFound(new Response
                {
                    Status = "Error",
                    Message = $"User with email ${emailCode.Email} was not found in database"
                });
            }
            user.EmailConfirmed = true;
            await _emailService.DeleteEmailCodeAsync(emailCode.Email);
            return Ok(
                new TokenResponse {
                Token = _tokenService.CreateToken(user)
            });
        }
    }
}