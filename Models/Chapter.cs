using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialPlatform.Models
{
    public class Chapter
    {
        public int Id { get; set; }
        [Required] public int TutorialId { get; set; }
        [ForeignKey(nameof(TutorialId))] public Tutorial Tutorial { get; set; } = default!;
        [Required] [MaxLength(200)] public string Title { get; set; } = string.Empty;

        // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
        [Required] public string HtmlContent { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}