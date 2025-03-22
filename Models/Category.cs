namespace Berrevoets.TutorialPlatform.Models;

public class Category
{
    public int Id { get; set; }

    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Tutorial> Tutorials { get; set; } = new List<Tutorial>();
}