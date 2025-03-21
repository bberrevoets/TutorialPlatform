using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TutorialPlatform.Areas.Identity.Data;
using TutorialPlatform.Models;

namespace TutorialPlatform.Pages.Tutorials
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Tutorial> Tutorials { get; set; } = new();

        public async Task OnGetAsync()
        {
            Tutorials = await _context.Tutorials
                .AsNoTracking()
                .ToListAsync();
        }
    }
}