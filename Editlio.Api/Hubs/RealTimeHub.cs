using Microsoft.AspNetCore.SignalR;
using Editlio.Domain.Services.Abstracts;
using Editlio.Shared.DTOs.Page;
using Editlio.Shared.DTOs.User;
using System;
using System.Threading.Tasks;

namespace Editlio.Api.Hubs
{
    public class RealTimeHub : Hub
    {
        private readonly IPageService _pageService;

        // Sample kullanıcı
        private static readonly UserDto SampleUser = new UserDto
        {
            Id = 1,
            Username = "sampleUser",
            Email = "sample@editlio.com",
            CreatedAt = DateTime.UtcNow
        };

        public RealTimeHub(IPageService pageService)
        {
            _pageService = pageService;
        }

        // Client bir gruba (sayfaya) katılır
        public async Task JoinPage(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be empty.", nameof(slug));
            }

            // Sayfa bilgilerini getir
            var pageResult = await _pageService.GetPageBySlugAsync(slug);
            if (!pageResult.Success || pageResult.Data == null)
            {
                await Clients.Caller.SendAsync("Error", "Page not found.");
                return;
            }

            // Kullanıcıyı slug'a özel bir gruba ekle
            await Groups.AddToGroupAsync(Context.ConnectionId, slug);

            // Mevcut sayfa içeriğini client'a gönder
            await Clients.Caller.SendAsync("LoadPageContent", pageResult.Data.Content);
        }

        // Client bir sayfadan ayrılır
        public async Task LeavePage(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be empty.", nameof(slug));
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, slug);
        }

        // Sayfa içeriğini günceller ve gruptaki diğer client'lara gönderir
        public async Task UpdatePageContent(string slug, string updatedContent)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug cannot be empty.", nameof(slug));
            }

            if (string.IsNullOrWhiteSpace(updatedContent))
            {
                throw new ArgumentException("Updated content cannot be empty.", nameof(updatedContent));
            }

            // Sayfa bilgilerini kontrol et
            var pageResult = await _pageService.GetPageBySlugAsync(slug);
            if (!pageResult.Success || pageResult.Data == null)
            {
                await Clients.Caller.SendAsync("Error", "Page not found.");
                return;
            }

            // Güncellenen bilgileri veritabanına kaydet
            var pageUpdateDto = new PageUpdateDto
            {
                Id = pageResult.Data.Id,
                Content = updatedContent,
                IsProtected = pageResult.Data.IsProtected
            };

            var updateResult = await _pageService.UpdatePageAsync(pageUpdateDto);
            if (!updateResult.Success)
            {
                await Clients.Caller.SendAsync("Error", "Failed to update page.");
                return;
            }

            // Güncel içeriği gruptaki diğer kullanıcılara gönder
            await Clients.Group(slug).SendAsync("ReceiveUpdatedContent", updatedContent);
        }

        // Yeni bir sayfa oluşturur ve slug döner
        public async Task CreatePage(PageCreateDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Slug))
            {
                throw new ArgumentException("Slug cannot be empty.", nameof(createDto.Slug));
            }

            if (string.IsNullOrWhiteSpace(createDto.Content))
            {
                throw new ArgumentException("Initial content cannot be empty.", nameof(createDto.Content));
            }

            // Yeni sayfayı sample kullanıcı ile kaydet
            var createResult = await _pageService.CreatePageAsync(new PageCreateDto
            {
                Slug = createDto.Slug,
                Content = createDto.Content,
                IsProtected = createDto.IsProtected,
                Password = createDto.IsProtected ? createDto.Password : null
            });

            if (!createResult.Success)
            {
                await Clients.Caller.SendAsync("Error", "Failed to create page.");
                return;
            }

            await Clients.Caller.SendAsync("PageCreated", createDto.Slug);
        }
    }
}
