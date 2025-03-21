namespace Berrevoets.TutorialPlatform.Models.Certificates
{
    public class CertificateInfo
    {
        public string UserName { get; set; } = string.Empty;
        public string TutorialTitle { get; set; } = string.Empty;
        public DateTime CompletionDate { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
    }
}