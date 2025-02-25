using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDBContext _context;
        private readonly EmailSettings _emailSettings;


        public EmailService(ApplicationDBContext context, IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }
        public string CreateCode(string email)
        {
            byte[] emailBytes = Encoding.UTF8.GetBytes(email);

            var base64Code = Convert.ToBase64String(emailBytes);
            return base64Code.Substring(0, 8);
        }

        public Task SendEmail(string email)
        {
            var client = new SmtpClient(_emailSettings.Server, _emailSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            var subject = "Подтверждение почты";
            var code = CreateCode(email);
            var message = code + " - код для подтверждения почты. Никому не говорите код.";
            var emailCode = new EmailCode(email, code);
            _context.EmailCodes.Add(emailCode);
            _context.SaveChanges();

            return client.SendMailAsync(
            new MailMessage(from: _emailSettings.Username,
                            to: email,
                            subject,
                            message
                            ));
        }

        public async Task<bool> ConfirmCode(EmailCode emailCode)
        {
            var foundEmailCode = await FindEmailCodeAsync(emailCode.Email);
            if (foundEmailCode == null || foundEmailCode.Code != emailCode.Code) {
                return false;
            }
            return true;
        }

        public async Task<EmailCode?> FindEmailCodeAsync(string email)
        {
            return await _context.EmailCodes.FirstOrDefaultAsync(ec => ec.Email == email);
        }

        public async Task<EmailCode?> DeleteEmailCodeAsync(string email)
        {
            var emailCode = await FindEmailCodeAsync(email);
            if (emailCode == null) {
                return null;
            }
            _context.EmailCodes.Remove(emailCode);
            await _context.SaveChangesAsync();

            return emailCode;
        }
    }
}