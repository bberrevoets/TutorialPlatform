using System.ComponentModel.DataAnnotations;

namespace Berrevoets.TutorialPlatform.Models;

public class Tag
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Tutorial> Tutorials { get; set; } = new List<Tutorial>();
}