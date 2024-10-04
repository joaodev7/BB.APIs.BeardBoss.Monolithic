using BB.APIs.BeardBoss.Monolithic.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BB.APIs.BeardBoss.Monolithic.DTOs.Auth
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"\d{11}", ErrorMessage = "Invalid CPF. CPF must have 11 digits.")]
        public string CPF { get; set; }
    }
}
