using Editlio.Shared.DTOs.File;
using Editlio.Web.Models;
using Editlio.Web.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Editlio.Web.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Editlio.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IFileService _fileService;

        public FileUploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Route("FileUpload/Upload")]
        public async Task<IActionResult> Upload(List<IFormFile> files, [FromForm] int pageId, [FromForm] string slug)
        {
            if (files == null || !files.Any())
                return Json(new { success = false, message = "Dosya seçilmedi" });

            try
            {
                var results = new List<bool>();
                foreach (var file in files)
                {
                    var fileViewModel = new FileViewModel
                    {
                        FileName = file.FileName,
                        FileSize = file.Length,
                        ContentType = file.ContentType,
                        PageId = pageId
                    };

                    results.Add(await _fileService.UploadFileAsync(slug, fileViewModel, file));
                }

                // Tüm dosyalar başarıyla yüklendi mi?
                bool allFilesUploaded = results.All(r => r);
                return Json(new { success = allFilesUploaded, message = allFilesUploaded ? "Dosyalar başarıyla yüklendi." : "Bazı dosyalar yüklenemedi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("FileUpload/Delete/{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteFileRequest request)
        {
            try
            {
                var result = await _fileService.DeleteFileAsync(id, request.Slug, request.FileName);
                return Json(new { success = result, message = result ? "Dosya başarıyla silindi." : "Dosya silinirken bir hata oluştu." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("FileUpload/GetFiles")]
        public async Task<IActionResult> GetFiles(int pageId, string slug)
        {
            try
            {
                // Fiziksel dosyalar ile veritabanı kayıtlarını senkronize et
                await _fileService.SyncPhysicalFilesWithDatabase(slug);

                // Tüm dosyaları getir
                var files = await _fileService.GetPageFilesAsync(pageId);

                // ViewModel oluştur
                var viewModel = new PageViewModel
                {
                    Id = pageId,
                    Files = files,
                    TotalFiles = files.Count
                };

                // ViewComponent'i render et
                var html = await this.RenderViewComponentAsync("FileUpload", viewModel);
                return Json(new { success = true, html, totalFiles = files.Count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}