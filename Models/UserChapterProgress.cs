using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Berrevoets.TutorialPlatform.Data;

namespace Berrevoets.TutorialPlatform.Models;

public class UserChapterProgress
{
    public int Id { get; set; }

    [Required]
    [MaxLength(450)]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = default!;

    [Required]
    public int ChapterId { get; set; }

    public Chapter Chapter { get; set; } = default!;
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}