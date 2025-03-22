namespace Berrevoets.TutorialPlatform.Models;

public class Tag
{
    public int Id { get; set; }

    public ICollection<TagTranslation> Translations { get; set; } = new List<TagTranslation>();

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<Tutorial> Tutorials { get; set; } = new List<Tutorial>();
}