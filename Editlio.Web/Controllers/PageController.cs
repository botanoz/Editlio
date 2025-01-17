using Editlio.Shared.DTOs.File;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Security.Claims;


namespace Editlio.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IUserService _userService;
        private readonly HubConnection _hubConnection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PageController(
    IPageService pageService,
    IUserService userService,
    IConfiguration configuration,
    IWebHostEnvironment webHostEnvironment,
    IHttpClientFactory httpClientFactory)
        {
            _pageService = pageService;
            _userService = userService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _httpClientFactory = httpClientFactory;

            string hubUrl = configuration["AppSettings:RealtimeHubUrl"] ?? throw new ArgumentNullException("RealtimeHubUrl is missing from configuration.");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();
        }

        /// <summary>
        /// Belirli bir slug'a göre sayfa yükler veya yeni bir sayfa oluşturur.
        /// </summary>
        /// <param name="slug">Sayfa slug'ı (opsiyonel)</param>
        [HttpGet("{slug?}")]
        public async Task<IActionResult> Index(string? slug)
        {
            // SignalR bağlantısını başlat
            await EnsureHubConnectionAsync();

            if (string.IsNullOrEmpty(slug))
            {
                slug = await GenerateUniqueSlugAsync();
            }

            var existingPage = await _pageService.GetPageBySlugAsync(slug);
            if (existingPage == null)
            {
                // Örnek bir kullanıcı al
                var defaultUser = await _userService.GetUserByIdAsync(1);
                if (defaultUser == null)
                {
                    return StatusCode(500, "Default user not found.");
                }

                // Yeni sayfa oluştur
                var newPage = new PageViewModel
                {
                    Slug = slug,
                    Content = string.Empty,
                    IsProtected = false,
                    CreatedAt = DateTime.UtcNow,
                    OwnerId = defaultUser.Id,
                    OwnerUsername = defaultUser.Username,
                    Files = new List<FileViewModel>(), // Boş dosya listesi
                    TotalFiles = 0,                    // Başlangıçta dosya yok
                    IsEditable = true                  // Yeni sayfa düzenlenebilir
                };

                var createResult = await _pageService.CreatePageAsync(newPage);
                if (!createResult)
                {
                    return StatusCode(500, "Failed to create a new page.");
                }

                // Sayfa için wwwroot/share/ altında klasör oluştur
                var uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "share", slug);
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                return RedirectToAction(nameof(Index), new { slug });
            }

            try
            {
                // Dosyaları API'den getir
                var client = _httpClientFactory.CreateClient();
                var files = await client.GetFromJsonAsync<List<FileDto>>($"{_configuration["ApiSettings:BaseUrl"]}/api/File/{existingPage.Id}");

                if (files != null)
                {
                    existingPage.Files = files.Select(f => new FileViewModel
                    {
                        Id = f.Id,
                        FileName = f.FileName,
                        FilePath = f.FilePath,
                        FileSize = f.FileSize,
                        ContentType = f.ContentType,
                        PageId = f.PageId
                    }).ToList();

                    existingPage.TotalFiles = files.Count;
                }
            }
            catch (Exception ex)
            {
                // API hatası durumunda boş liste kullan
                existingPage.Files = new List<FileViewModel>();
                existingPage.TotalFiles = 0;
                // Hatayı loglayabilirsiniz
            }

            // Sayfa düzenleme yetkisi kontrolü
            existingPage.IsEditable = existingPage.OwnerId == null ||
                                     (User.Identity?.IsAuthenticated == true &&
                                      existingPage.OwnerId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"));

            // SignalR'dan mevcut slug için gruba katıl
            await _hubConnection.InvokeAsync("JoinPage", slug);
            ViewData["PageId"] = existingPage.Id;
            ViewData["Title"] = $"Edit - {existingPage.Slug}";

            return View(existingPage);
        }

        /// <summary>
        /// Sayfa içeriğini günceller ve SignalR ile diğer istemcilere gönderir.
        /// </summary>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdatePageContent(int pageId, string updatedContent)
        {
            // SignalR bağlantısını başlat
            await EnsureHubConnectionAsync();

            var existingPage = await _pageService.GetPageByIdAsync(pageId);
            if (existingPage == null)
            {
                return NotFound("Page not found.");
            }

            existingPage.Content = updatedContent;
            var updateResult = await _pageService.UpdatePageAsync(existingPage);
            if (!updateResult)
            {
                return StatusCode(500, "Failed to update the page.");
            }

            // SignalR ile gruba güncelleme gönder
            await _hubConnection.InvokeAsync("UpdatePageContent", existingPage.Slug, updatedContent);
            return Ok(new { success = true });
        }

        /// <summary>
        /// Rastgele bir slug üretir.
        /// </summary>
        private async Task<string> GenerateUniqueSlugAsync()
        {
            string slug;
            do
            {
                slug = GenerateRandomSlug(5);
            }
            while (await _pageService.GetPageBySlugAsync(slug) != null);

            return slug;
        }

        /// <summary>
        /// Belirtilen uzunlukta rastgele bir slug üretir.
        /// </summary>
        private string GenerateRandomSlug(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }

        /// <summary>
        /// SignalR bağlantısını kontrol eder ve bağlanmamışsa yeniden başlatır.
        /// </summary>
        private async Task EnsureHubConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        /// <summary>
        /// Controller sona erdiğinde SignalR bağlantısını kapatır.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hubConnection.DisposeAsync().GetAwaiter().GetResult();
            }
            base.Dispose(disposing);
        }
    }
}
