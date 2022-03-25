using Microsoft.AspNetCore.Mvc;

namespace itr.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
