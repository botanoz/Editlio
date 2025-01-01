using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Editlio.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IUserService _userService;
        private readonly HubConnection _hubConnection;

        public PageController(IPageService pageService, IUserService userService)
        {
            _pageService = pageService;
            _userService = userService;

            // SignalR bağlantısını başlat
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7273/hubs/realtime") // Backend API SignalR Hub URL
                .WithAutomaticReconnect() // Otomatik yeniden bağlanma
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
                    OwnerUsername = defaultUser.Username
                };

                var createResult = await _pageService.CreatePageAsync(newPage);
                if (!createResult)
                {
                    return StatusCode(500, "Failed to create a new page.");
                }

                return RedirectToAction(nameof(Index), new { slug });
            }

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
