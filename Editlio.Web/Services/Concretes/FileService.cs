using Editlio.Shared.DTOs.File;
using Editlio.Web.Helpers;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using System.Net.Http.Json;
using System.IO;

namespace Editlio.Web.Services.Concretes
{
    public class FileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public FileService(HttpClient httpClient)
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

        public async Task<List<FileViewModel>> GetAllFilesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/file");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<FileViewModel>();
                }

                var fileDtos = await response.Content.ReadFromJsonAsync<List<FileDto>>();
                return fileDtos?.ToViewModelList() ?? new List<FileViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching files: {ex.Message}");
                return new List<FileViewModel>();
            }
        }

        public async Task<FileViewModel?> GetFileByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/file/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var fileDto = await response.Content.ReadFromJsonAsync<FileDto>();
                return fileDto?.ToViewModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching file by ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UploadFileAsync(FileViewModel file, Stream fileStream)
        {
            try
            {
                var pageDirectory = Path.Combine(_baseStoragePath, file.PageId.ToString());
                if (!Directory.Exists(pageDirectory))
                {
                    Directory.CreateDirectory(pageDirectory);
                }

                var filePath = Path.Combine(pageDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await fileStream.CopyToAsync(stream);
                }

                // Optionally notify the API about the uploaded file
                var fileDto = file.ToDto();
                var response = await _httpClient.PostAsJsonAsync("api/file", fileDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateFileAsync(FileViewModel file)
        {
            try
            {
                var fileDto = file.ToDto();
                var response = await _httpClient.PutAsJsonAsync($"api/file/{file.Id}", fileDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating file ID {file.Id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(int id)
        {
            try
            {
                var file = await GetFileByIdAsync(id);
                if (file == null)
                {
                    return false;
                }

                var filePath = Path.Combine(_baseStoragePath, file.PageId.ToString(), file.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var response = await _httpClient.DeleteAsync($"api/file/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}
