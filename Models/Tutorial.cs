using System.ComponentModel.DataAnnotations;

namespace Berrevoets.TutorialPlatform.Models;

public class Tutorial
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();

    [Required]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}