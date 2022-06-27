using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace FOOBAR.Controllers
{
  public class HomeController : Controller
	{
		private readonly IHostEnvironment _env;

		public HomeController(IHostEnvironment env)
		{
			_env = env ??
			       throw new ArgumentNullException(
				       $"{nameof(HomeController)}.Ctr parameter {nameof(env)} cannot be null.");
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
