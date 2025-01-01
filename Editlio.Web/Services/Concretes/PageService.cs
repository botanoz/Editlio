using Editlio.Shared.DTOs.Page;
using Editlio.Web.Helpers;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using System.Net.Http.Json;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Editlio.Shared.Helpers;

namespace Editlio.Web.Services.Concretes
{
    public class PageService : IPageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public PageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            EnsureStorageDirectoryExists();
        }

        private void EnsureStorageDirectoryExists()
        {
            if (!Directory.Exists(_baseStoragePath))
            {
                Directory.CreateDirectory(_baseStoragePath);
            }
        }

        public async Task<List<PageViewModel>> GetAllPagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/page");
                if (response.IsSuccessStatusCode)
                {
                    var pageDtos = await response.Content.ReadFromJsonAsync<List<PageDto>>();
                    return pageDtos?.ToViewModelList() ?? new List<PageViewModel>();
                }
                Console.WriteLine("Failed to fetch all pages.");
                return new List<PageViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all pages: {ex.Message}");
                return new List<PageViewModel>();
            }
        }

        public async Task<PageViewModel?> GetPageByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/page/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var pageDto = await response.Content.ReadFromJsonAsync<PageDto>();
                    return pageDto?.ToViewModel();
                }
                Console.WriteLine($"Failed to fetch the page by ID: {id}.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching page by ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<PageViewModel?> GetPageBySlugAsync(string slug)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/page/slug/{slug}");
                if (response.IsSuccessStatusCode)
                {
                    // Gelen JSON: { success, data, errors }
                    var result = await response.Content.ReadFromJsonAsync<Result<PageDto>>();
                    if (result != null && result.Success && result.Data != null)
                    {
                        // Data -> PageDto -> PageViewModel
                        return result.Data.ToViewModel();
                    }
                    // success=false ya da data=null durumu
                    Console.WriteLine($"Failed to fetch the page by slug: {slug} (Success: {result?.Success}).");
                    return null;
                }
                // HTTP 404 veya başka durum
                Console.WriteLine($"Failed to fetch the page by slug: {slug}, StatusCode: {response.StatusCode}.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching page by slug {slug}: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> CreatePageAsync(PageViewModel page)
        {
            try
            {
                var pageDto = page.ToDto();
                var response = await _httpClient.PostAsJsonAsync("api/page", pageDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating page: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePageAsync(PageViewModel page)
        {
            try
            {
                var pageDto = page.ToDto();
                var response = await _httpClient.PutAsJsonAsync($"api/page/{page.Id}", pageDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating page ID {page.Id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletePageAsync(int id)
        {
            try
            {
                // Fetch the page details to locate associated files
                var page = await GetPageByIdAsync(id);
                if (page == null)
                {
                    Console.WriteLine($"Page with ID {id} not found.");
                    return false;
                }

                // Delete associated files
                var pageDirectory = Path.Combine(_baseStoragePath, page.Id.ToString());
                if (Directory.Exists(pageDirectory))
                {
                    Directory.Delete(pageDirectory, true); // Delete directory and its contents
                }

                // Delete the page
                var response = await _httpClient.DeleteAsync($"api/page/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting page ID {id}: {ex.Message}");
                return false;
            }
        }

        // Example SignalR usage (to be expanded based on actual implementation needs)
        public async Task NotifyPageUpdate(int pageId, string updatedContent)
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("/hubs/page")
                    .Build();

                await connection.StartAsync();
                await connection.InvokeAsync("SendPageUpdate", pageId, updatedContent);
                await connection.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error notifying page update: {ex.Message}");
            }
        }
    }
}
