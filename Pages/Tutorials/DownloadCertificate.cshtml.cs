using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models.Certificates;
using Berrevoets.TutorialPlatform.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using System.Text;

using TutorialPlatform.Data;
using TutorialPlatform.Models;

namespace Berrevoets.TutorialPlatform.Pages.Tutorials
{
    [Authorize]
    public class DownloadCertificateModel : PageModel
    {
        private readonly CertificateService _certificateService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DownloadCertificateModel(ApplicationDbContext context, CertificateService certificateService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _certificateService = certificateService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int tutorialId)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            string userName = user?.FullName ?? user?.Email ?? "Unknown User";

            Tutorial? tutorial = await _context.Tutorials.FindAsync(tutorialId);

            if (tutorial == null)
            {
                return NotFound();
            }

            UserTutorialProgress? progress = await _context.UserTutorialProgresses.FirstOrDefaultAsync(p =>
                p.TutorialId == tutorialId && p.UserId == userId && p.IsCompleted);

            if (progress == null || userId == null)
            {
                return Forbid();
            }

            IssuedCertificate? existingCert =
                await _context.IssuedCertificates.FirstOrDefaultAsync(c =>
                    c.UserId == userId && c.TutorialId == tutorial.Id);
            
            if (existingCert == null)
            {
                existingCert = new IssuedCertificate
                {
                    UserId = userId,
                    TutorialId = tutorial.Id,
                    IssuedAt = DateTime.UtcNow,
                    SerialNumber = GenerateSerialNumber(userId, tutorial.Id)
                };

                _context.IssuedCertificates.Add(existingCert);
                await _context.SaveChangesAsync();
            }

            CertificateInfo cert = new()
            {
                UserName = userName,
                TutorialTitle = tutorial.Title,
                CompletionDate = existingCert.IssuedAt,
                SerialNumber = existingCert.SerialNumber
            };

            byte[] pdf = _certificateService.GenerateCertificate(cert);

            return File(pdf, "application/pdf", $"Certificate - {tutorial.Title}.pdf");
        }

        private string GenerateSerialNumber(string? userId, int tutorialId)
        {
            string hash = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userId}:{tutorialId}"));
            return hash[..8].ToUpper();
        }
    }
}