using System.ComponentModel.DataAnnotations;

namespace TutorialPlatform.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required] [MaxLength(50)] public string Name { get; set; } = string.Empty;
        public ICollection<Tutorial> Tutorials { get; set; } = new List<Tutorial>();
    }
}