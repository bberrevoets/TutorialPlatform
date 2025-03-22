using System.Net.Mail;
using Berrevoets.TutorialPlatform.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace Berrevoets.TutorialPlatform.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;
    private readonly EmailSettings _settings;

    public EmailSender(ILogger<EmailSender> logger, IOptions<EmailSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        SmtpClient smtpClient = new(_settings.SmtpHost, _settings.SmtpPort)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network, EnableSsl = _settings.EnableSsl
        };

        MailMessage mailMessage = new(_settings.FromEmail, email, subject, htmlMessage) { IsBodyHtml = true };

        _logger.LogInformation("Sending email to {Email}: {Subject}", email, subject);

        return smtpClient.SendMailAsync(mailMessage);
    }
}