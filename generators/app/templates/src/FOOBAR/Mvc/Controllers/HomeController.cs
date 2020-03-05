using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FOOBAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException($"{nameof(HomeController)}.Ctr parameter {nameof(env)} cannot be null.");
        }

        // [AuthorizeWith(Permission = "foobar-application")] //TODO uncomment this to enable authentication
        public IActionResult Index()
        {
            ViewBag.IsDevelopement = false; //_env.IsDevelopment();
            return View("~/Mvc/Views/Home/Index.cshtml");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
