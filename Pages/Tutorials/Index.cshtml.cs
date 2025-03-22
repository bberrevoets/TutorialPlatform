using System.Globalization;
using System.Security.Claims;
using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Berrevoets.TutorialPlatform.Pages.Tutorials;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<TutorialViewModel> Tutorials { get; set; } = [];

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var tutorials = await _context.Tutorials
            .Include(t => t.Chapters)
            .Include(t => t.Category).ThenInclude(c => c.Translations)
            .Include(t => t.Tags).ThenInclude(tag => tag.Translations)
            .ToListAsync();

        var userChapterProgress = await _context.UserChapterProgresses
            .Where(p => p.UserId == userId)
            .ToListAsync();

        List<UserTutorialProgress> userTutorialProgress = await _context.UserTutorialProgresses
            .Where(p => p.UserId == userId)
            .ToListAsync();

        Tutorials = tutorials.Select(t =>
        {
            var total = t.Chapters.Count;
            var completed = userChapterProgress.Count(p => t.Chapters.Select(c => c.Id).Contains(p.ChapterId));
            var isCompleted = userTutorialProgress.Any(p => p.TutorialId == t.Id && p.IsCompleted);

            return new TutorialViewModel
            {
                Tutorial = t, TotalChapters = total, CompletedChapters = completed, IsCompleted = isCompleted
            };
        }).ToList();
    }

    public class TutorialViewModel
    {
        public Tutorial Tutorial { get; set; } = default!;
        public int CompletedChapters { get; set; }
        public int TotalChapters { get; set; }
        public bool IsCompleted { get; set; }

        public string CategoryName =>
            Tutorial.Category?.Translations
                .FirstOrDefault(t => t.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                ?.Name ?? "(No translation)";

        public List<string> TagNames =>
            Tutorial.Tags.Select(t =>
                t.Translations.FirstOrDefault(tt => tt.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                    ?.Name ?? "(No translation)").ToList();
    }
}