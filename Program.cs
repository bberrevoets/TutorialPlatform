using Berrevoets.TutorialPlatform.Data;
using Berrevoets.TutorialPlatform.Services;
using Berrevoets.TutorialPlatform.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using QuestPDF;
using QuestPDF.Infrastructure;
using Serilog;

Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration["Serilog:SeqServerUrl"] ?? throw new InvalidOperationException(
        "Serilog 'SeqServerUrl' not found."))
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        // Use SplitQuery to avoid Cartesian explosion and improve performance
        // when including multiple collection navigations (e.g., Chapters and Tags)
        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<CertificateService>();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(10);
    setup.MaximumHistoryEntriesPerEndpoint(50);
    setup.AddHealthCheckEndpoint("TutorialPlatform", "/health");
}).AddInMemoryStorage();

var seqHealthUri = builder.Configuration["HealthChecks:Seq:Uri"]
                   ?? throw new InvalidOperationException("Seq health check URI not configured.");

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "sql", tags: ["db", "sql"])
    .AddUrlGroup(
        new Uri(seqHealthUri),
        "seq-server",
        tags: ["logging", "seq"]
    );

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.MapHealthChecksUI(options => { options.UIPath = "/health-ui"; });

app.MapMetrics();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    Seeder.Seed(db);
}

app.Run();