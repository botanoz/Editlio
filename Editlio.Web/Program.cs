using Editlio.Web.Hubs;
using Editlio.Web.Services.Abstracts;
using Editlio.Web.Services.Concretes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add SignalR
builder.Services.AddSignalR();

// Base API URL ayarýný al ve doðrula
var apiBaseAddress = builder.Configuration.GetSection("ApiSettings:BaseUrl").Value;

if (string.IsNullOrEmpty(apiBaseAddress))
{
    throw new InvalidOperationException("API BaseUrl is not configured in appsettings.json.");
}

// HttpClient servislerini BaseAddress ile ekle
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

// CORS ayarlarý
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

// Middleware yapýlandýrmasý
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

app.MapHub<PageHub>("/hubs/page");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Index}/{id?}");

app.Run();
