// Extensions/ControllerExtensions.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;

namespace Editlio.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewComponentAsync(this Controller controller, string componentName, object? model)
        {
            var viewContext = new ViewContext(
                controller.ControllerContext,
                new FakeView(),
                controller.ViewData,
                controller.TempData,
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            var viewComponentHelper = controller.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
            (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);

            var result = await viewComponentHelper.InvokeAsync(componentName, model);
            using var writer = new StringWriter();
            result.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        private class FakeView : IView
        {
            public string Path => string.Empty;
            public async Task RenderAsync(ViewContext context)
            {
                await Task.CompletedTask;
            }
        }
    }
}