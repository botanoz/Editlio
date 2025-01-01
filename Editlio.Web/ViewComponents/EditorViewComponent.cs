using Editlio.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Editlio.Web.ViewComponents
{

    public class EditorViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(PageViewModel model)
        {
            return View(model);
        }
    }
}
