using Microsoft.AspNetCore.Mvc;

namespace Editlio.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string randomSlug = GenerateRandomSlug(8);


            TempData["RandomSlug"] = randomSlug;

            return View();
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
