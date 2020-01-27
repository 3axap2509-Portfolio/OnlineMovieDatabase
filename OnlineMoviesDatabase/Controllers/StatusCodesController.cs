using Microsoft.AspNetCore.Mvc;

namespace OnlineMovieDatabase.Controllers
{
    public class StatusCodesController : Controller
    {
        public IActionResult Index(int statusCode)
        {
            return View(statusCode);
        }
    }
}