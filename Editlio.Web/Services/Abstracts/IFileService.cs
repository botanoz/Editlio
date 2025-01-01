using Editlio.Web.Models;

namespace Editlio.Web.Services.Abstracts
{
    public interface IFileService
    {
        Task<List<FileViewModel>> GetAllFilesAsync();
        Task<FileViewModel?> GetFileByIdAsync(int id);
        Task<bool> UploadFileAsync(FileViewModel file, Stream fileStream);
        Task<bool> UpdateFileAsync(FileViewModel file);
        Task<bool> DeleteFileAsync(int id);
    }
}
