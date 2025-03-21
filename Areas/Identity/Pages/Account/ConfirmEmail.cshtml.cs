#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace TutorialPlatform.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, ILogger<ConfirmEmailModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                _logger.LogWarning("❌ Email confirmation attempted with missing parameters: userId={UserId}, code={Code}", userId, code);
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("❌ Email confirmation failed. User with ID {UserId} not found.", userId);
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "❌ Confirming Email for {UserId} failed. Failed to Decode {Code}", userId, code);
            }
                
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            if (result.Succeeded)
            {
                _logger.LogInformation("✅ User with ID {UserId} successfully confirmed their email.", user.Id);
            }
            else
            {
                _logger.LogWarning("❌ Email confirmation failed for user ID {UserId}. Errors: {Errors}",
                    user.Id,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return Page();
        }
    }
}
