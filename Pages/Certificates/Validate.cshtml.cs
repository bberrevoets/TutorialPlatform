using Berrevoets.TutorialPlatform.Models.Certificates;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TutorialPlatform.Data;

namespace Berrevoets.TutorialPlatform.Pages.Certificates
{
    public class ValidateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ValidateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SerialNumber { get; set; }

        public IssuedCertificate? Certificate { get; set; }

        public bool IsSearched => !string.IsNullOrWhiteSpace(SerialNumber);
        public bool IsValid => Certificate != null;

        public async Task OnGetAsync()
        {
            if (IsSearched == false)
            {
                return;
            }

            Certificate = await _context.IssuedCertificates
                .Include(c => c.User)
                .Include(c => c.Tutorial)
                .FirstOrDefaultAsync(c => c.SerialNumber == SerialNumber);
        }
    }
}