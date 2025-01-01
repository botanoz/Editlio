using Editlio.Domain.Services.Abstracts;
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.DTOs.Page;
using Editlio.Shared.Entities;
using Editlio.Shared.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Concretes
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<Result<PageDto>> GetPageByIdAsync(int pageId)
        {
            var pageResult = await _pageRepository.GetAsync(p => p.Id == pageId);
            return pageResult.Success
                ? Result<PageDto>.SuccessResult(pageResult.Data!.ToDto())
                : Result<PageDto>.FailureResult(new List<string> { "Page not found" });
        }

        public async Task<Result<PageDto>> GetPageBySlugAsync(string slug)
        {
            var pageResult = await _pageRepository.GetAsync(p => p.Slug == slug);
            return pageResult.Success
                ? Result<PageDto>.SuccessResult(pageResult.Data!.ToDto())
                : Result<PageDto>.FailureResult(new List<string> { "Page not found" });
        }

        public async Task<Result<List<PageDto>>> GetUserPagesAsync(int userId)
        {
            var pageListResult = await _pageRepository.GetListAsync(p => p.OwnerId == userId);
            return pageListResult.Success
                ? Result<List<PageDto>>.SuccessResult(pageListResult.Data!.ToDtoList())
                : Result<List<PageDto>>.FailureResult(new List<string> { "No pages found for this user" });
        }

        public async Task<Result<PageDto>> CreatePageAsync(PageCreateDto pageDto)
        {
            var page = pageDto.ToEntity();
            page.CreatedDate = System.DateTime.UtcNow;

            var createResult = await _pageRepository.AddAsync(page);
            return createResult.Success
                ? Result<PageDto>.SuccessResult(createResult.Data!.ToDto())
                : Result<PageDto>.FailureResult(new List<string> { "Failed to create page" });
        }

        public async Task<Result<PageDto>> UpdatePageAsync(PageUpdateDto pageDto)
        {
            var pageResult = await _pageRepository.GetAsync(p => p.Id == pageDto.Id);
            if (!pageResult.Success || pageResult.Data == null)
            {
                return Result<PageDto>.FailureResult(new List<string> { "Page not found" });
            }

            var page = pageDto.ToEntity(pageResult.Data);
            page.UpdatedDate = System.DateTime.UtcNow;

            var updateResult = await _pageRepository.UpdateAsync(page);
            return updateResult.Success
                ? Result<PageDto>.SuccessResult(updateResult.Data!.ToDto())
                : Result<PageDto>.FailureResult(new List<string> { "Failed to update page" });
        }

        public async Task<Result> DeletePageAsync(int pageId)
        {
            var pageResult = await _pageRepository.GetAsync(p => p.Id == pageId);
            if (!pageResult.Success || pageResult.Data == null)
            {
                return Result.FailureResult(new List<string> { "Page not found" });
            }

            var deleteResult = await _pageRepository.DeleteAsync(pageResult.Data);
            return deleteResult.Success
                ? Result.SuccessResult()
                : Result.FailureResult(new List<string> { "Failed to delete page" });
        }
    }
}
