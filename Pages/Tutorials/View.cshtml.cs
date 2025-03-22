using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Berrevoets.TutorialPlatform.Pages.Tutorials;

public class ViewModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ViewModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Tutorial? Tutorial { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Tutorial = await _context.Tutorials
            .Include(t => t.Chapters)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (Tutorial != null)
            Tutorial.Chapters = Tutorial.Chapters
                .OrderBy(c => c.Order)
                .ToList();

        if (Tutorial == null) return NotFound();

        return Page();
    }
}