using Berrevoets.TutorialPlatform.Models.Certificates;

using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Berrevoets.TutorialPlatform.Services
{
    public class CertificateService
    {
        public byte[] GenerateCertificate(CertificateInfo info)
        {
            Document document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);

                    page.Content().Border(2).Padding(30).Column(col =>
                    {
                        col.Spacing(20);

                        // Logo
                        col.Item().AlignCenter().Height(120).Element(container =>
                        {
                            string imagePath = Path.Combine("wwwroot", "images", "logo.png");
                            if (File.Exists(imagePath))
                            {
                                byte[] imageData = File.ReadAllBytes(imagePath);
                                container.Image(imageData).FitHeight();
                            }
                        });

                        // Header
                        col.Item().AlignCenter().Text("Certificate of Completion")
                            .FontSize(32).Bold().FontColor(Colors.Blue.Darken2);

                        col.Item().AlignCenter().Text("This certifies that").FontSize(18);

                        col.Item().AlignCenter().Text(info.UserName).FontSize(24).Bold();

                        col.Item().AlignCenter().Text("has successfully completed the tutorial").FontSize(18);
                        col.Item().AlignCenter().Text(info.TutorialTitle).FontSize(22).Italic();

                        col.Item().AlignCenter().Text($"Date: {info.CompletionDate:yyyy-MM-dd}").FontSize(14);
                        col.Item().AlignCenter().Text($"Certificate ID: {info.SerialNumber}").FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
            });

            using MemoryStream stream = new();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}