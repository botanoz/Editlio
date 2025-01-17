using Editlio.Shared.DTOs.File;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Editlio.Web.ViewComponents
{
    public class FileUploadViewComponent : ViewComponent
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FileUploadViewComponent> _logger;

        public FileUploadViewComponent(IFileService fileService, ILogger<FileUploadViewComponent> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync(PageViewModel model)
        {
            try
            {
                _logger.LogInformation($"Getting files for page {model.Id}");

                // Fiziksel dosyalar ile veritabanı kayıtlarını senkronize et
                await _fileService.SyncPhysicalFilesWithDatabase(model.Slug);

                // Dosyaları getir
                var files = await _fileService.GetPageFilesAsync(model.Id);

                if (files == null || !files.Any())
                {
                    _logger.LogWarning($"No files found for page {model.Id}");
                    model.Files = new List<FileViewModel>();
                    model.TotalFiles = 0;
                }
                else
                {
                    _logger.LogInformation($"Retrieved {files.Count} files for page {model.Id}");

                    // Model'e dosyaları set et
                    model.Files = files;
                    model.TotalFiles = files.Count;

                    // Debug için dosya bilgilerini logla
                    foreach (var file in files)
                    {
                        _logger.LogDebug($"File: {file.FileName}, Path: {file.FilePath}, Size: {file.FileSize} bytes");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in FileUploadViewComponent for page {model.Id}: {ex.Message}");

                // Hata durumunda boş bir model döndür
                model.Files = new List<FileViewModel>();
                model.TotalFiles = 0;
                return View(model);
            }
        }
    }
}