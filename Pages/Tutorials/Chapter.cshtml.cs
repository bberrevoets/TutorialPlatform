using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using TutorialPlatform.Data;
using TutorialPlatform.Models;

namespace TutorialPlatform.Pages.Tutorials
{
    [Authorize]
    public class ChapterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChapterModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Chapter? Chapter { get; set; }
        public string HtmlContent => Chapter?.HtmlContent ?? string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            Chapter = await _context.Chapters
                .Include(c => c.Tutorial)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Chapter == null)
            {
                return NotFound();
            }

            // 1. Track Chapter Progress
            bool alreadyCompleted = await _context.UserChapterProgresses
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
            List<int> tutorialChapterIds = await _context.Chapters
                .Where(c => c.TutorialId == Chapter.TutorialId)
                .Select(c => c.Id)
                .ToListAsync();

            List<int> completedChapters = await _context.UserChapterProgresses
                .Where(p => p.UserId == userId && tutorialChapterIds.Contains(p.ChapterId))
                .Select(p => p.ChapterId)
                .ToListAsync();

            if (completedChapters.Count == tutorialChapterIds.Count)
            {
                UserTutorialProgress? progress = await _context.UserTutorialProgresses
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
}