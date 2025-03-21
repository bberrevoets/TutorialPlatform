using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TutorialPlatform.Areas.Identity.Data;
using TutorialPlatform.Models;

namespace TutorialPlatform.Pages.Tutorials;

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
        Chapter = await _context.Chapters
            .Include(c => c.Tutorial)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (Chapter == null) return NotFound();

        return Page();
    }
}