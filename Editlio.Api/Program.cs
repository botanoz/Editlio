using Editlio.Domain;
using Editlio.Infrastructure;
using Editlio.Api.Hubs;
using Editlio.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Baðlantý dizesini al ve kontrol et
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
}

// Add services to the container.
builder.Services.AddDataLayer(connectionString);
builder.Services.AddBusinessLayer();

builder.Services.AddControllers();

// SignalR için ekleme
builder.Services.AddSignalR();

// Swagger/OpenAPI desteði
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS politikasý ekle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // Geliþtirme aþamasýnda tüm originlere izin ver
    });
});

var app = builder.Build();

// Swagger konfigürasyonu
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware konfigürasyonu
app.UseHttpsRedirection();

// CORS Middleware
app.UseCors();

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Routing ve Authorization sýrasý
app.UseRouting();

// SignalR hub ve controller rotalarý
app.MapControllers();
app.MapHub<RealTimeHub>("/hubs/realtime");

// Authorization
app.UseAuthorization();

app.Run();
