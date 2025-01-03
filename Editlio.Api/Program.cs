using Editlio.Domain;
using Editlio.Infrastructure;
using Editlio.Api.Hubs;
using Editlio.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Docker'da API'nin 7273 portunda çalýþmasýný saðla
builder.WebHost.UseUrls("https://localhost:7273");

// Baðlantý dizesini al ve kontrol et (Önce environment deðiþkenine bak)
var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_DB_CONNECTION")
                     ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in environment variables or appsettings.json.");
}

// Servisleri ekle
builder.Services.AddDataLayer(connectionString);
builder.Services.AddBusinessLayer();
builder.Services.AddControllers();

// SignalR için ekleme
builder.Services.AddSignalR();

// Swagger/OpenAPI desteði
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS politikasý ekle (Docker içinden eriþime izin ver)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // Docker için tüm originlere izin ver
    });
});

var app = builder.Build();

// Swagger konfigürasyonu
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware konfigürasyonu
app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();

// Routing ve Authorization sýrasý
app.UseRouting();
app.UseAuthorization();

// SignalR ve API rotalarý
app.MapControllers();
app.MapHub<RealTimeHub>("/hubs/realtime");

app.Run();
