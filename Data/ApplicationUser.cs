using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace Berrevoets.TutorialPlatform.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;
        [MaxLength(100)] public string? MiddleName { get; set; } = string.Empty;
        [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;

        public string FullName => string.IsNullOrWhiteSpace(MiddleName)
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {MiddleName} {LastName}";
    }
}