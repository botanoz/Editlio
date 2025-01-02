using Editlio.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Editlio.Web.ViewComponents
{
    public class PageNavbarViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
