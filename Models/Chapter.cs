using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialPlatform.Models;

public class Chapter
{
    public int Id { get; set; }
    [Required] public int TutorialId { get; set; }
    [ForeignKey(nameof(TutorialId))] public Tutorial Tutorial { get; set; } = default!;
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Content { get; set; } = string.Empty;
    public int Order { get; set; }
}