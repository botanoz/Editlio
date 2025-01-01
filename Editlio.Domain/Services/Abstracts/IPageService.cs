using Editlio.Shared.DTOs.Page;
using Editlio.Shared.Entities;
using Editlio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editlio.Domain.Services.Abstracts
{
    public interface IPageService
    {
        Task<Result<PageDto>> GetPageByIdAsync(int pageId);
        Task<Result<PageDto>> GetPageBySlugAsync(string slug);
        Task<Result<List<PageDto>>> GetUserPagesAsync(int userId);
        Task<Result<PageDto>> CreatePageAsync(PageCreateDto pageDto);
        Task<Result<PageDto>> UpdatePageAsync(PageUpdateDto pageDto);
        Task<Result> DeletePageAsync(int pageId);
    }
}
