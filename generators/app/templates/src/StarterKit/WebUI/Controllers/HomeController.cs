using Microsoft.AspNet.Mvc;

namespace StarterKit.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/WebUI/Views/Home/Index.cshtml");
        }
    }
}
