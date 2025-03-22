using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Models.Certificates;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TutorialPlatform.Models;

namespace TutorialPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<UserTutorialProgress> UserTutorialProgresses { get; set; } = default!;
        public DbSet<UserChapterProgress> UserChapterProgresses { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Tag> Tags { get; set; } = default!;
        public DbSet<IssuedCertificate> IssuedCertificates { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IssuedCertificate>()
                .HasIndex(c => c.SerialNumber)
                .IsUnique();
        }
    }
}