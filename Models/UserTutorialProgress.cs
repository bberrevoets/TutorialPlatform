using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berrevoets.TutorialPlatform.Data;

namespace Berrevoets.TutorialPlatform.Models;

public class UserTutorialProgress
{
    public int Id { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = default!;

    [Required]
    public int TutorialId { get; set; }

    public Tutorial Tutorial { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletionDate { get; set; }
    public ICollection<UserChapterProgress> Chapters { get; set; } = new List<UserChapterProgress>();
}