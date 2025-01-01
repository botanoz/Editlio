using Editlio.Shared.DTOs.File;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Abstracts
{
    public interface IFileService
    {
        Task<Result<FileDto>> GetFileByIdAsync(int fileId);
        Task<Result<List<FileDto>>> GetFilesByPageIdAsync(int pageId);
        Task<Result<FileDto>> CreateFileAsync(FileCreateDto fileDto);
        Task<Result<FileDto>> UpdateFileAsync(FileUpdateDto fileDto);
        Task<Result> DeleteFileAsync(int fileId);
    }
}
