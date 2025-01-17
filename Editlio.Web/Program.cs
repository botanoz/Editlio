using Editlio.Web.Constraints;
using Editlio.Web.Services.Abstracts;
using Editlio.Web.Services.Concretes;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Basic service configurations
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

// Get API base address from configuration
var apiBaseAddress = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")
    ?? Environment.GetEnvironmentVariable("API_URL")
    ?? throw new InvalidOperationException("API URL not configured");

// Register HTTP clients for services
builder.Services.AddHttpClient<IUserService, UserService>(client =>
    client.BaseAddress = new Uri(apiBaseAddress));
builder.Services.AddHttpClient<IPageService, PageService>(client =>
    client.BaseAddress = new Uri(apiBaseAddress));
builder.Services.AddHttpClient<IFileService, FileService>(client =>
    client.BaseAddress = new Uri(apiBaseAddress));
builder.Services.AddHttpClient();
// Configure route constraints
builder.Services.Configure<RouteOptions>(options =>
    options.ConstraintMap.Add("slug", typeof(SlugConstraint)));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(apiBaseAddress)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

// Configure health checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString()
            })
        });
    }
});

// Configure routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "PageRoute",
    pattern: "{slug}",
    defaults: new { controller = "Page", action = "Index" });


app.Run();