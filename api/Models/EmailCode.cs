using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class EmailCode
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public EmailCode(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}