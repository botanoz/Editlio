using Editlio.Shared.Entities;
using Editlio.Shared.DTOs.File;
using Editlio.Shared.DTOs.Page;
using Editlio.Shared.DTOs.User;
using Editlio.Shared.DTOs;

namespace Editlio.Shared.Helpers
{
    public static class EntityDtoMapper
    {
        // Page -> PageDto
        public static PageDto ToDto(this Page entity)
        {
            return new PageDto
            {
                Id = entity.Id,
                Slug = entity.Slug,
                Content = entity.Content,
                IsProtected = entity.IsProtected,
                OwnerId = entity.OwnerId,
                OwnerUsername = entity.Owner?.Username
            };
        }

        // PageDto -> Page
        public static Page ToEntity(this PageDto dto)
        {
            return new Page
            {
                Id = dto.Id,
                Slug = dto.Slug,
                Content = dto.Content,
                IsProtected = dto.IsProtected,
                OwnerId = dto.OwnerId
            };
        }

        // User -> UserDto
        public static UserDto ToDto(this User entity)
        {
            return new UserDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                CreatedAt = entity.CreatedDate
            };
        }

        // UserDto -> User
        public static User ToEntity(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Username = dto.Username,
                Email = dto.Email,
                CreatedDate = dto.CreatedAt
            };
        }

        // File -> FileDto
        public static FileDto ToDto(this Shared.Entities.File entity)
        {
            return new FileDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                FileSize = entity.FileSize,
                ContentType = entity.ContentType,
                PageId = entity.PageId
            };
        }

        // FileDto -> File
        public static Shared.Entities.File ToEntity(this FileDto dto)
        {
            return new Shared.Entities.File
            {
                Id = dto.Id,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                FileSize = dto.FileSize,
                ContentType = dto.ContentType,
                PageId = dto.PageId
            };
        }

        // RealTimeUpdate -> RealTimeUpdateDto
        public static RealTimeUpdateDto ToDto(this RealTimeUpdate entity)
        {
            return new RealTimeUpdateDto
            {
                Id = entity.Id,
                PageId = entity.PageId,
                UpdatedBy = entity.UpdatedBy,
                ChangeDescription = entity.ChangeDescription,
                UpdatedAt = entity.UpdatedDate ?? entity.CreatedDate
            };
        }

        // RealTimeUpdateDto -> RealTimeUpdate
        public static RealTimeUpdate ToEntity(this RealTimeUpdateDto dto)
        {
            return new RealTimeUpdate
            {
                Id = dto.Id,
                PageId = dto.PageId,
                UpdatedBy = dto.UpdatedBy,
                ChangeDescription = dto.ChangeDescription,
                UpdatedDate = dto.UpdatedAt
            };
        }

        // List Conversion Helpers
        public static List<PageDto> ToDtoList(this IEnumerable<Page> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static List<UserDto> ToDtoList(this IEnumerable<User> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static List<FileDto> ToDtoList(this IEnumerable<Shared.Entities.File> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static List<RealTimeUpdateDto> ToDtoList(this IEnumerable<RealTimeUpdate> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}
