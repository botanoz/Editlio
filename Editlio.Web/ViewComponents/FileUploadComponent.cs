using Editlio.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Web.ViewComponents
{
    public class FileUploadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PageViewModel model)
        {
            return View(model);
        }
    }
}
