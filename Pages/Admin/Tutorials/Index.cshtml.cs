using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Berrevoets.TutorialPlatform.Pages.Admin.Tutorials;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Tutorial> Tutorials { get; set; } = [];

    public async Task OnGetAsync()
    {
        Tutorials = await _context.Tutorials
            .Include(t => t.Category)
            .ThenInclude(c => c.Translations)
            .Include(t => t.Chapters)
            .OrderBy(t => t.Title)
            .ToListAsync();
    }
}