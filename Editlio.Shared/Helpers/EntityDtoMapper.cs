using Editlio.Shared.Entities;
using Editlio.Shared.DTOs.File;
using Editlio.Shared.DTOs.Page;
using Editlio.Shared.DTOs.User;
using Editlio.Shared.DTOs;
using Editlio.Shared.DTOs.RealTimeUpdate;
using Editlio.Shared.DTOs.RefreshToken;
using System.Collections.Generic;
using System.Linq;

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

        // PageCreateDto -> Page
        public static Page ToEntity(this PageCreateDto dto)
        {
            return new Page
            {
                Slug = dto.Slug,
                Content = dto.Content,
                IsProtected = dto.IsProtected,
                PasswordHash = dto.Password
            };
        }

        // PageUpdateDto -> Page (Mevcut entity'yi güncellemek için)
        public static Page ToEntity(this PageUpdateDto dto, Page existingPage)
        {
            existingPage.Content = dto.Content;
            existingPage.IsProtected = dto.IsProtected;
            existingPage.PasswordHash = dto.Password;
            return existingPage;
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

        // UserRegisterDto -> User
        public static User ToEntity(this UserRegisterDto dto)
        {
            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.Password
            };
        }

        // UserDto -> User (Mevcut entity'yi güncellemek için)
        public static User ToEntity(this UserDto dto, User existingUser)
        {
            existingUser.Username = dto.Username;
            existingUser.Email = dto.Email;
            // Diğer güncellenebilir alanlar buraya eklenebilir
            return existingUser;
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

        // FileCreateDto -> File
        public static Shared.Entities.File ToEntity(this FileCreateDto dto)
        {
            return new Shared.Entities.File
            {
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                FileSize = dto.FileSize,
                ContentType = dto.ContentType,
                PageId = dto.PageId
            };
        }

        // FileUpdateDto -> File (Mevcut entity'yi güncellemek için)
        public static  Shared.Entities.File ToEntity(this FileUpdateDto dto, Shared.Entities.File existingFile)
        {
            existingFile.FileName = dto.FileName;
            existingFile.FilePath = dto.FilePath;
            existingFile.FileSize = dto.FileSize;
            existingFile.ContentType = dto.ContentType;
            existingFile.PageId = dto.PageId;
            return existingFile;
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

        // RealTimeUpdateCreateDto -> RealTimeUpdate
        public static RealTimeUpdate ToEntity(this RealTimeUpdateCreateDto dto)
        {
            return new RealTimeUpdate
            {
                PageId = dto.PageId,
                UpdatedBy = dto.UpdatedBy,
                ChangeDescription = dto.ChangeDescription
            };
        }

        // RealTimeUpdateUpdateDto -> RealTimeUpdate (Mevcut entity'yi güncellemek için)
        public static RealTimeUpdate ToEntity(this RealTimeUpdateUpdateDto dto, RealTimeUpdate existingUpdate)
        {
            existingUpdate.PageId = dto.PageId;
            existingUpdate.UpdatedBy = dto.UpdatedBy;
            existingUpdate.ChangeDescription = dto.ChangeDescription;
            return existingUpdate;
        }

        // RefreshToken -> RefreshTokenDto
        public static RefreshTokenDto ToDto(this RefreshToken entity)
        {
            return new RefreshTokenDto
            {
                Id = entity.Id,
                Token = entity.Token,
                ExpiresAt = entity.ExpiresAt,
                IsRevoked = entity.IsRevoked,
                UserId = entity.UserId
            };
        }

        // RefreshTokenCreateDto -> RefreshToken
        public static RefreshToken ToEntity(this RefreshTokenCreateDto dto)
        {
            return new RefreshToken
            {
                Token = dto.Token,
                ExpiresAt = dto.ExpiresAt,
                IsRevoked = dto.IsRevoked,
                UserId = dto.UserId
            };
        }

        // RefreshTokenUpdateDto -> RefreshToken (Mevcut entity'yi güncellemek için)
        public static RefreshToken ToEntity(this RefreshTokenUpdateDto dto, RefreshToken existingToken)
        {
            existingToken.Token = dto.Token;
            existingToken.ExpiresAt = dto.ExpiresAt;
            existingToken.IsRevoked = dto.IsRevoked;
            existingToken.UserId = dto.UserId;
            return existingToken;
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

        public static List<RefreshTokenDto> ToDtoList(this IEnumerable<RefreshToken> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }
    }
}
