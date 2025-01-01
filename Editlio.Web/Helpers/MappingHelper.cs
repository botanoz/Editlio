using Editlio.Shared.DTOs.File;
using Editlio.Shared.DTOs.Page;
using Editlio.Shared.DTOs.RealTimeUpdate;
using Editlio.Shared.DTOs.User;
using Editlio.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Editlio.Web.Helpers
{
    public static class MappingHelper
    {
        // UserViewModel -> UserDto
        public static UserDto ToDto(this UserViewModel viewModel)
        {
            return new UserDto
            {
                Id = viewModel.Id,
                Username = viewModel.Username,
                Email = viewModel.Email,
                CreatedAt = viewModel.CreatedAt
            };
        }

        // UserDto -> UserViewModel
        public static UserViewModel ToViewModel(this UserDto dto, int pageCount = 0)
        {
            return new UserViewModel
            {
                Id = dto.Id,
                Username = dto.Username ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                CreatedAt = dto.CreatedAt,
                LastLoginDate = null,
                IsActive = true,
                PageCount = pageCount
            };
        }

        // PageViewModel -> PageDto
        public static PageDto ToDto(this PageViewModel viewModel)
        {
            return new PageDto
            {
                Id = viewModel.Id,
                Slug = viewModel.Slug,
                Content = viewModel.Content,
                IsProtected = viewModel.IsProtected,
                OwnerId = viewModel.OwnerId,
                OwnerUsername = viewModel.OwnerUsername
            };
        }

        // PageDto -> PageViewModel
        public static PageViewModel ToViewModel(this PageDto dto, List<FileDto>? fileDtos = null)
        {
            return new PageViewModel
            {
                Id = dto.Id,
                Slug = dto.Slug ?? string.Empty,
                Content = dto.Content ?? string.Empty,
                IsProtected = dto.IsProtected,
                PasswordHint = dto.IsProtected ? "This page is protected." : null,
                OwnerId = dto.OwnerId,
                OwnerUsername = dto.OwnerUsername ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Files = fileDtos?.Select(f => f.ToViewModel()).ToList() ?? new List<FileViewModel>(),
                TotalFiles = fileDtos?.Count ?? 0,
                IsEditable = true
            };
        }

        // FileViewModel -> FileDto
        public static FileDto ToDto(this FileViewModel viewModel)
        {
            return new FileDto
            {
                Id = viewModel.Id,
                FileName = viewModel.FileName,
                FilePath = viewModel.FilePath,
                FileSize = viewModel.FileSize,
                ContentType = viewModel.ContentType,
                PageId = viewModel.PageId
            };
        }

        // FileDto -> FileViewModel
        public static FileViewModel ToViewModel(this FileDto dto)
        {
            return new FileViewModel
            {
                Id = dto.Id,
                FileName = dto.FileName ?? string.Empty,
                FilePath = dto.FilePath ?? string.Empty,
                FileSize = dto.FileSize,
                ContentType = dto.ContentType ?? string.Empty,
                PageId = dto.PageId,
                UploadedAt = DateTime.UtcNow,
                IsDownloadable = true
            };
        }

        // RealTimeUpdateViewModel -> RealTimeUpdateDto
        public static RealTimeUpdateDto ToDto(this RealTimeViewModel viewModel)
        {
            return new RealTimeUpdateDto
            {
                Id = viewModel.Id,
                PageId = viewModel.PageId,
                UpdatedBy = viewModel.UpdatedBy,
                ChangeDescription = viewModel.ChangeDescription,
                UpdatedAt = viewModel.UpdatedAt
            };
        }

        // RealTimeUpdateDto -> RealTimeUpdateViewModel
        public static RealTimeViewModel ToViewModel(this RealTimeUpdateDto dto)
        {
            return new RealTimeViewModel
            {
                Id = dto.Id,
                PageId = dto.PageId,
                UpdatedBy = dto.UpdatedBy ?? "Unknown User",
                ChangeDescription = dto.ChangeDescription ?? string.Empty,
                UpdatedAt = dto.UpdatedAt,
                UpdateType = "Content"
            };
        }

        // Liste dönüşümleri
        public static List<UserViewModel> ToViewModelList(this IEnumerable<UserDto> dtos, List<int>? pageCounts = null)
        {
            if (pageCounts == null || pageCounts.Count != dtos.Count())
            {
                pageCounts = Enumerable.Repeat(0, dtos.Count()).ToList();
            }

            return dtos.Select((dto, index) => dto.ToViewModel(pageCounts[index])).ToList();
        }

        public static List<PageViewModel> ToViewModelList(this IEnumerable<PageDto> dtos, List<List<FileDto>>? fileLists = null)
        {
            if (fileLists == null || fileLists.Count != dtos.Count())
            {
                fileLists = Enumerable.Repeat(new List<FileDto>(), dtos.Count()).ToList();
            }

            return dtos.Select((dto, index) => dto.ToViewModel(fileLists[index])).ToList();
        }

        public static List<FileViewModel> ToViewModelList(this IEnumerable<FileDto> dtos)
        {
            return dtos.Select(dto => dto.ToViewModel()).ToList();
        }

        public static List<RealTimeViewModel> ToViewModelList(this IEnumerable<RealTimeUpdateDto> dtos)
        {
            return dtos.Select(dto => dto.ToViewModel()).ToList();
        }
    }
}
