
using Editlio.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Editlio.Web.Services.Abstracts
{
    public interface IFileService
    {
        Task<List<FileViewModel>> GetPageFilesAsync(int pageId);
        Task<List<FileViewModel>> GetFilesBySlugAsync(string slug);
        Task<FileViewModel> GetFileByIdAsync(int id);
        Task<bool> UploadFileAsync(string slug, FileViewModel file, IFormFile formFile);
        Task<bool> UpdateFileAsync(FileViewModel file);
        Task<bool> DeleteFileAsync(int id, string slug, string fileName);
        Task SyncPhysicalFilesWithDatabase(string slug); // Yeni eklenen metod
    }
}