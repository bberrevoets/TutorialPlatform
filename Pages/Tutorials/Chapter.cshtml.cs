using System.Security.Claims;
using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Berrevoets.TutorialPlatform.Pages.Tutorials;

[Authorize]
public class ChapterModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ChapterModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Chapter? Chapter { get; set; }
    public string HtmlContent => Chapter?.HtmlContent ?? string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Challenge();

        Chapter = await _context.Chapters
            .Include(c => c.Tutorial)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (Chapter == null) return NotFound();

        // 1. Track Chapter Progress
        var alreadyCompleted = await _context.UserChapterProgresses
            .AnyAsync(p => p.UserId == userId && p.ChapterId == Chapter.Id);

        if (!alreadyCompleted)
        {
            _context.UserChapterProgresses.Add(new UserChapterProgress
            {
                UserId = userId, ChapterId = Chapter.Id, CompletedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        // 2. Check if all chapters are now complete
        var tutorialChapterIds = await _context.Chapters
            .Where(c => c.TutorialId == Chapter.TutorialId)
            .Select(c => c.Id)
            .ToListAsync();

        var completedChapters = await _context.UserChapterProgresses
            .Where(p => p.UserId == userId && tutorialChapterIds.Contains(p.ChapterId))
            .Select(p => p.ChapterId)
            .ToListAsync();

        if (completedChapters.Count == tutorialChapterIds.Count)
        {
            var progress = await _context.UserTutorialProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TutorialId == Chapter.TutorialId);

            if (progress == null)
            {
                _context.UserTutorialProgresses.Add(new UserTutorialProgress
                {
                    UserId = userId,
                    TutorialId = Chapter.TutorialId,
                    IsCompleted = true,
                    CompletionDate = DateTime.UtcNow
                });
            }
            else if (!progress.IsCompleted)
            {
                progress.IsCompleted = true;
                progress.CompletionDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        return Page();
    }
}