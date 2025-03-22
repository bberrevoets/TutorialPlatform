using Berrevoets.TutorialPlatform.Data;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialPlatform.Models
{
    public class UserChapterProgress
    {
        public int Id { get; set; }
        [Required] public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public ApplicationUser User { get; set; } = default!;
        [Required] public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } = default!;
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }
}