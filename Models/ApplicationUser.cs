using BB.APIs.BeardBoss.Monolithic.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BB.APIs.BeardBoss.Monolithic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? Role { get; set; }

        [Required]
        [RegularExpression(@"\d{11}", ErrorMessage = "Invalid CPF. CPF must have 11 digits.")]
        public string? CPF { get; set; }
    }
}
