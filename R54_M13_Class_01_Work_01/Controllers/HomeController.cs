using Microsoft.AspNetCore.Mvc;

namespace R54_M13_Class_01_Work_01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}
