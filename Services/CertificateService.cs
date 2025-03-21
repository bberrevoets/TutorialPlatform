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
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.DefaultTextStyle(x => x.FontSize(16));
                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().AlignCenter().Text("Certificate of Completion")
                            .FontSize(28).Bold();

                        col.Item().Height(20); // spacer

                        col.Item().Text("This certifies that").AlignCenter();
                        col.Item().Text(info.UserName).FontSize(22).SemiBold().AlignCenter();

                        col.Item().Height(10); // spacer

                        col.Item().Text("has successfully completed the tutorial").AlignCenter();
                        col.Item().Text(info.TutorialTitle).FontSize(20).Italic().AlignCenter();

                        col.Item().Height(20); // spacer

                        col.Item().Text($"Date: {info.CompletionDate:yyyy-MM-dd}").AlignCenter();
                        col.Item().Text($"Certificate ID: {info.SerialNumber}")
                            .FontSize(10).AlignCenter();
                    });
                });
            });

            using MemoryStream stream = new();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}