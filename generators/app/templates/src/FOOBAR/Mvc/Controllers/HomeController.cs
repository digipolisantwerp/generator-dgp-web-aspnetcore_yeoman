using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using Digipolis.Auth.Authorization.Attributes;

namespace FOOBAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException($"{nameof(HomeController)}.Ctr parameter {nameof(env)} cannot be null.");
        }

        [AuthorizeWith(Permission = "foobar-application")] //TODO uncomment this to enable authentication
        public IActionResult Index()
        {
            ViewBag.IsDevelopement = false; //_env.IsDevelopment();
            return View("~/Mvc/Views/Home/Index.cshtml");
        }

        public IActionResult Error()
        {
            return View("~/Mvc/Views/Shared/Error.cshtml");
        }
    }
}
