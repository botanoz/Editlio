using Microsoft.AspNetCore.Mvc;

namespace Editlio.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            // Landing page'i göster
            return View();
        }

        [HttpGet("/generate-slug")]
        public IActionResult GenerateSlug()
        {
            // Rastgele bir slug üret ve yönlendir
            string randomSlug = GenerateRandomSlug(8);
            return RedirectToAction("Index", "Page", new { slug = randomSlug });
        }

        private string GenerateRandomSlug(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}