using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using TutorialPlatform.Data;
using TutorialPlatform.Models;

namespace TutorialPlatform.Pages.Tutorials
{
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            List<Tutorial> tutorials = await _context.Tutorials
                .Include(t => t.Chapters)
                .Include(t => t.Category)
                .Include(t => t.Tags)
                .ToListAsync();

            List<UserChapterProgress> userChapterProgress = await _context.UserChapterProgresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            List<UserTutorialProgress> userTutorialProgress = await _context.UserTutorialProgresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            Tutorials = tutorials.Select(t =>
            {
                int total = t.Chapters.Count;
                int completed = userChapterProgress.Count(p => t.Chapters.Select(c => c.Id).Contains(p.ChapterId));
                bool isCompleted = userTutorialProgress.Any(p => p.TutorialId == t.Id && p.IsCompleted);

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
            public string CategoryName => Tutorial.Category?.Name ?? "Uncategorized";
            public List<string> TagNames => Tutorial.Tags.Select(t => t.Name).ToList();

        }
    }
}