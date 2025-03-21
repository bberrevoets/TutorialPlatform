using System.ComponentModel.DataAnnotations;

namespace TutorialPlatform.Models;

public class Tutorial
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}