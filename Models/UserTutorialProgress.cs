using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialPlatform.Models
{
    public class UserTutorialProgress
    {
        public int Id { get; set; }
        [Required] public string UserId { get; set; } = string.Empty;
        [ForeignKey(nameof(UserId))] public IdentityUser User { get; set; } = default!;
        [Required] public int TutorialId { get; set; }
        public Tutorial Tutorial { get; set; } = default!;
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ICollection<UserChapterProgress> Chapters { get; set; } = new List<UserChapterProgress>();
    }
}