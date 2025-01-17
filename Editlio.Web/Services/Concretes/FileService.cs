using Editlio.Shared.DTOs.File;
using Editlio.Web.Helpers;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Editlio.Web.Services.Concretes
{
    public class FileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseStoragePath;
        private readonly ILogger<FileService> _logger;

        public FileService(HttpClient httpClient, IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger)
        {
            _httpClient = httpClient;
            _baseStoragePath = Path.Combine(webHostEnvironment.WebRootPath, "share");
            EnsureStorageDirectoryExists();
            _logger = logger;
        }

        private void EnsureStorageDirectoryExists()
        {
            if (!Directory.Exists(_baseStoragePath))
            {
                Directory.CreateDirectory(_baseStoragePath);
                _logger.LogInformation($"Created directory: {_baseStoragePath}");
            }
        }

        // Dosya adını temizleme ve kısaltma
        private string CleanFileName(string fileName)
        {
            // Özel karakterleri ve boşlukları kaldır
            var cleanedName = Regex.Replace(fileName, @"[^a-zA-Z0-9\-_.]", "_");

            // Dosya adını kısalt (örneğin, 50 karakterle sınırla)
            return cleanedName.Length > 50 ? cleanedName.Substring(0, 50) : cleanedName;
        }

        // Benzersiz dosya adı oluşturma
        private string GenerateUniqueFileName(string fileName)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var random = new Random().Next(1000, 9999);
            var cleanedName = CleanFileName(fileName);
            return $"{timestamp}_{random}_{cleanedName}";
        }

        public async Task<List<FileViewModel>> GetPageFilesAsync(int pageId)
        {
            try
            {
                _logger.LogInformation($"Fetching files for pageId: {pageId}");
                var response = await _httpClient.GetAsync($"api/File/Pageid/{pageId}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned {response.StatusCode}");
                    return new List<FileViewModel>();
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<FileDto>>>();
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    var files = new List<FileViewModel>();

                    foreach (var fileDto in apiResponse.Data)
                    {
                        var slug = fileDto.FilePath.Split('/')[2]; // Örnek: /share/slug/filename
                        var physicalFileName = Path.GetFileName(fileDto.FilePath); // Fiziksel dosya adını al
                        var physicalPath = Path.Combine(_baseStoragePath, slug, physicalFileName);

                        if (System.IO.File.Exists(physicalPath))
                        {
                            files.Add(new FileViewModel
                            {
                                Id = fileDto.Id,
                                FileName = fileDto.FileName, // Orijinal dosya adı
                                FilePath = fileDto.FilePath, // Benzersiz dosya yolu
                                FileSize = fileDto.FileSize,
                                ContentType = fileDto.ContentType,
                                PageId = fileDto.PageId
                            });
                        }
                        else
                        {
                            _logger.LogInformation($"File not found physically: {fileDto.FilePath}. Deleting from database.");
                            await DeleteFileAsync(fileDto.Id, slug, fileDto.FilePath); // FilePath'e göre sil
                        }
                    }

                    return files;
                }

                return new List<FileViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetPageFilesAsync: {ex.Message}");
                return new List<FileViewModel>();
            }
        }

        public async Task<List<FileViewModel>> GetFilesBySlugAsync(string slug)
        {
            try
            {
                _logger.LogInformation($"Fetching files for slug: {slug}");
                var response = await _httpClient.GetAsync($"api/File/Slug/{slug}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned {response.StatusCode}");
                    return new List<FileViewModel>();
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<FileDto>>>();
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return apiResponse.Data.Select(fileDto => new FileViewModel
                    {
                        Id = fileDto.Id,
                        FileName = fileDto.FileName,
                        FilePath = fileDto.FilePath,
                        FileSize = fileDto.FileSize,
                        ContentType = fileDto.ContentType,
                        PageId = fileDto.PageId
                    }).ToList();
                }

                return new List<FileViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetFilesBySlugAsync: {ex.Message}");
                return new List<FileViewModel>();
            }
        }

        public async Task<FileViewModel> GetFileByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching file by id: {id}");
                var response = await _httpClient.GetAsync($"api/File/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned {response.StatusCode}");
                    return null;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<FileDto>>();
                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return new FileViewModel
                    {
                        Id = apiResponse.Data.Id,
                        FileName = apiResponse.Data.FileName,
                        FilePath = apiResponse.Data.FilePath,
                        FileSize = apiResponse.Data.FileSize,
                        ContentType = apiResponse.Data.ContentType,
                        PageId = apiResponse.Data.PageId
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetFileByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UploadFileAsync(string slug, FileViewModel file, IFormFile formFile)
        {
            try
            {
                _logger.LogInformation($"Starting file upload for slug: {slug}");

                // Dosyanın kaydedileceği klasörü oluştur
                var pageDirectory = Path.Combine(_baseStoragePath, slug);
                if (!Directory.Exists(pageDirectory))
                {
                    Directory.CreateDirectory(pageDirectory);
                    _logger.LogInformation($"Created directory: {pageDirectory}");
                }

                // Dosya adını temizle ve benzersiz hale getir
                var fileName = Path.GetFileName(formFile.FileName);
                var uniqueFileName = GenerateUniqueFileName(fileName);
                var filePath = Path.Combine(pageDirectory, uniqueFileName);

                // Dosyayı kalıcı olarak kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                _logger.LogInformation($"File saved to: {filePath}");

                // Dosya bilgilerini veritabanına kaydet
                var fileCreateDto = new FileCreateDto
                {
                    FileName = fileName, // Orijinal dosya adını kaydet
                    FilePath = $"/share/{slug}/{uniqueFileName}", // Benzersiz dosya yolunu kaydet
                    FileSize = formFile.Length,
                    ContentType = formFile.ContentType,
                    PageId = file.PageId
                };

                _logger.LogInformation($"Sending file info to API: {JsonSerializer.Serialize(fileCreateDto)}");

                var response = await _httpClient.PostAsJsonAsync("api/File", fileCreateDto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API Error: {response.StatusCode}, Content: {errorContent}");
                    return false;
                }

                _logger.LogInformation("File upload completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UploadFileAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateFileAsync(FileViewModel file)
        {
            try
            {
                var fileUpdateDto = new FileUpdateDto
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    FilePath = file.FilePath,
                    FileSize = file.FileSize,
                    ContentType = file.ContentType,
                    PageId = file.PageId
                };

                var response = await _httpClient.PutAsJsonAsync("api/File", fileUpdateDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateFileAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(int id, string slug, string filePath)
        {
            try
            {
                // Fiziksel dosya adını al
                var physicalFileName = Path.GetFileName(filePath);
                var physicalPath = Path.Combine(_baseStoragePath, slug, physicalFileName);

                // Fiziksel dosyayı sil
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                    _logger.LogInformation($"Physical file deleted: {physicalPath}");
                }
                else
                {
                    _logger.LogWarning($"Physical file not found: {physicalPath}");
                }

                // API'den sil
                var response = await _httpClient.DeleteAsync($"api/File/file/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API Error: {response.StatusCode}, Content: {errorContent}");
                    return false;
                }

                _logger.LogInformation($"File with ID {id} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteFileAsync: {ex.Message}");
                return false;
            }
        }

        public async Task SyncPhysicalFilesWithDatabase(string slug)
        {
            try
            {
                var directoryPath = Path.Combine(_baseStoragePath, slug);
                if (!Directory.Exists(directoryPath))
                {
                    _logger.LogInformation($"Directory not found: {directoryPath}");
                    return;
                }

                // Fiziksel dosyaları al
                var physicalFiles = Directory.GetFiles(directoryPath).Select(Path.GetFileName).ToList();

                // Veritabanındaki dosyaları al
                var dbFilesResponse = await _httpClient.GetAsync($"api/File/Slug/{slug}");
                if (!dbFilesResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to fetch files from database for slug: {slug}");
                    return;
                }

                var dbFiles = await dbFilesResponse.Content.ReadFromJsonAsync<ApiResponse<List<FileDto>>>();
                if (dbFiles?.Data == null)
                {
                    _logger.LogWarning($"No files found in database for slug: {slug}");
                    return;
                }

                var dbFilePaths = dbFiles.Data.Select(f => f.FilePath).ToList();

                // Fizikselde olup veritabanında olmayan dosyaları sil
                foreach (var physicalFile in physicalFiles)
                {
                    var physicalFilePath = $"/share/{slug}/{physicalFile}";
                    if (!dbFilePaths.Contains(physicalFilePath))
                    {
                        var filePath = Path.Combine(directoryPath, physicalFile);
                        File.Delete(filePath);
                        _logger.LogInformation($"Deleted orphaned file: {filePath}");
                    }
                }

                // Veritabanında olup fizikselde olmayan dosyaları sil
                foreach (var dbFile in dbFiles.Data)
                {
                    var physicalFileName = Path.GetFileName(dbFile.FilePath);
                    var physicalPath = Path.Combine(directoryPath, physicalFileName);
                    if (!System.IO.File.Exists(physicalPath))
                    {
                        await DeleteFileAsync(dbFile.Id, slug, dbFile.FilePath);
                        _logger.LogInformation($"Deleted database record for missing file: {dbFile.FilePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SyncPhysicalFilesWithDatabase: {ex.Message}");
            }
        }
    }
}