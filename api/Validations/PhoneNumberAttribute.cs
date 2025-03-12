using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace api.Validations
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private const string PhoneNumberPattern = @"^8\d{10}$";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var phoneNumber = value.ToString();
            if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
            {
                return new ValidationResult("Phone number must be in the format '89999999999'.");
            }

            return ValidationResult.Success;
        }
    }
}