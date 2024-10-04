using System.ComponentModel.DataAnnotations;

namespace BB.APIs.BeardBoss.Monolithic.DTOs.Auth
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"\d{11}", ErrorMessage = "Invalid CPF. CPF must have 11 digits.")]
        public string CPF { get; set; }
    }
}
