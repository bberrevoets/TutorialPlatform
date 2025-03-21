using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

using Prometheus;

using Serilog;

using TutorialPlatform.Data;
using TutorialPlatform.Services;
using TutorialPlatform.Settings;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                          throw new InvalidOperationException(
                              "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
    {
        // Use SplitQuery to avoid Cartesian explosion and improve performance
        // when including multiple collection navigations (e.g., Chapters and Tags)
        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(10);
    setup.MaximumHistoryEntriesPerEndpoint(50);
    setup.AddHealthCheckEndpoint("TutorialPlatform", "/health");
}).AddInMemoryStorage();

string seqHealthUri = builder.Configuration["HealthChecks:Seq:Uri"]
                      ?? throw new InvalidOperationException("Seq health check URI not configured.");

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString, name: "sql", tags: ["db", "sql"])
    .AddUrlGroup(
        new Uri(seqHealthUri),
        "seq-server",
        tags: ["logging", "seq"]
    );

WebApplication app = builder.Build();

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

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.MapMetrics();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    Seeder.Seed(db);
}

app.Run();