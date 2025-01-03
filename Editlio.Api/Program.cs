using Editlio.Domain;
using Editlio.Infrastructure;
using Editlio.Api.Hubs;
using Editlio.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Docker'da API'nin 7273 portunda �al��mas�n� sa�la
builder.WebHost.UseUrls("https://localhost:7273");

// Ba�lant� dizesini al ve kontrol et (�nce environment de�i�kenine bak)
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

// SignalR i�in ekleme
builder.Services.AddSignalR();

// Swagger/OpenAPI deste�i
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS politikas� ekle (Docker i�inden eri�ime izin ver)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // Docker i�in t�m originlere izin ver
    });
});

var app = builder.Build();

// Swagger konfig�rasyonu
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware konfig�rasyonu
app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<ExceptionMiddleware>();

// Routing ve Authorization s�ras�
app.UseRouting();
app.UseAuthorization();

// SignalR ve API rotalar�
app.MapControllers();
app.MapHub<RealTimeHub>("/hubs/realtime");

app.Run();
