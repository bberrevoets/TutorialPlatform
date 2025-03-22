using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berrevoets.TutorialPlatform.Models;

public class TagTranslation
{
    public int Id { get; set; }

    [Required]
    public int TagId { get; set; }

    [ForeignKey(nameof(TagId))]
    public Tag Tag { get; set; } = default!;

    [Required]
    [StringLength(10)]
    public string Language { get; set; } = "en";

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}