using Microsoft.AspNetCore.Mvc;

namespace CaloryCalcApp.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "You are welcome to our application.";
            return View();
        }
    }
}
