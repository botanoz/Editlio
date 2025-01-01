using Editlio.Domain.Services.Abstracts;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.DTOs.File;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Concretes
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<Result<FileDto>> GetFileByIdAsync(int fileId)
        {
            var fileResult = await _fileRepository.GetAsync(f => f.Id == fileId);
            return fileResult.Success
                ? Result<FileDto>.SuccessResult(fileResult.Data!.ToDto())
                : Result<FileDto>.FailureResult(new List<string> { "File not found" });
        }

        public async Task<Result<List<FileDto>>> GetFilesByPageIdAsync(int pageId)
        {
            var fileListResult = await _fileRepository.GetListAsync(f => f.PageId == pageId);
            return fileListResult.Success
                ? Result<List<FileDto>>.SuccessResult(fileListResult.Data!.ToDtoList())
                : Result<List<FileDto>>.FailureResult(new List<string> { "No files found for this page" });
        }

        public async Task<Result<FileDto>> CreateFileAsync(FileCreateDto fileDto)
        {
            var file = fileDto.ToEntity();
            file.UpdatedDate = System.DateTime.UtcNow;

            var createResult = await _fileRepository.AddAsync(file);
            return createResult.Success
                ? Result<FileDto>.SuccessResult(createResult.Data!.ToDto())
                : Result<FileDto>.FailureResult(new List<string> { "Failed to create file" });
        }

        public async Task<Result<FileDto>> UpdateFileAsync(FileUpdateDto fileDto)
        {
            var fileResult = await _fileRepository.GetAsync(f => f.Id == fileDto.Id);
            if (!fileResult.Success || fileResult.Data == null)
            {
                return Result<FileDto>.FailureResult(new List<string> { "File not found" });
            }

            var file = fileDto.ToEntity(fileResult.Data);

            var updateResult = await _fileRepository.UpdateAsync(file);
            return updateResult.Success
                ? Result<FileDto>.SuccessResult(updateResult.Data!.ToDto())
                : Result<FileDto>.FailureResult(new List<string> { "Failed to update file" });
        }

        public async Task<Result> DeleteFileAsync(int fileId)
        {
            var fileResult = await _fileRepository.GetAsync(f => f.Id == fileId);
            if (!fileResult.Success || fileResult.Data == null)
            {
                return Result.FailureResult(new List<string> { "File not found" });
            }

            var deleteResult = await _fileRepository.DeleteAsync(fileResult.Data);
            return deleteResult.Success
                ? Result.SuccessResult()
                : Result.FailureResult(new List<string> { "Failed to delete file" });
        }
    }
}
