using Editlio.Domain;
using Editlio.Infrastructure;
using Editlio.Api.Hubs;
using Editlio.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ba�lant� dizesini al ve kontrol et
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");
}

// Add services to the container.
builder.Services.AddDataLayer(connectionString);
builder.Services.AddBusinessLayer();

builder.Services.AddControllers();

// SignalR i�in ekleme
builder.Services.AddSignalR();

// Swagger/OpenAPI deste�i
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS politikas� ekle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true); // Geli�tirme a�amas�nda t�m originlere izin ver
    });
});

var app = builder.Build();

// Swagger konfig�rasyonu
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware konfig�rasyonu
app.UseHttpsRedirection();

// CORS Middleware
app.UseCors();

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Routing ve Authorization s�ras�
app.UseRouting();

// SignalR hub ve controller rotalar�
app.MapControllers();
app.MapHub<RealTimeHub>("/hubs/realtime");

// Authorization
app.UseAuthorization();

app.Run();
