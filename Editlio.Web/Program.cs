using Editlio.Web.Services.Abstracts;
using Editlio.Web.Services.Concretes;
using Editlio.Web.Constraints;

var builder = WebApplication.CreateBuilder(args);

// Set URL for Docker compatibility
builder.WebHost.UseUrls("http://0.0.0.0:80");

// Add services to the container
builder.Services.AddControllersWithViews();

// Add SignalR
builder.Services.AddSignalR();

// Configure API Base URL (Read from Environment Variable or appsettings.json)
var apiBaseAddress = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value
                     ?? Environment.GetEnvironmentVariable("ASPNETCORE_API_URL");

if (string.IsNullOrEmpty(apiBaseAddress))
{
    throw new InvalidOperationException("API BaseUrl is not configured in appsettings.json or environment variables.");
}

// Add HttpClient services with BaseAddress
builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddHttpClient<IPageService, PageService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddHttpClient<IFileService, FileService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

// Add custom route constraint
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("slug", typeof(SlugConstraint));
});

// Configure CORS (Allow API Calls from Docker Network)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

// Configure route mappings
app.MapControllerRoute(
    name: "PageRoute",
    pattern: "{slug:slug}",
    defaults: new { controller = "Page", action = "Index" });

app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
