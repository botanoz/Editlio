using Editlio.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Editlio.Web.Services.Abstracts
{
    public interface IPageService
    {
        Task<List<PageViewModel>> GetAllPagesAsync();
        Task<PageViewModel?> GetPageByIdAsync(int id);
        Task<PageViewModel?> GetPageBySlugAsync(string slug);
        Task<bool> CreatePageAsync(PageViewModel page);
        Task<bool> UpdatePageAsync(PageViewModel page);
        Task<bool> DeletePageAsync(int id);

        // SignalR ile sayfa güncellemelerini bildirme
        Task NotifyPageUpdate(int pageId, string updatedContent);
    }
}
