using Azure;
using Cinema.Models;
using Cinema.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace Cinema.Controllers
{
	public class HomeController : Controller
	{
		CinemaContext db = new CinemaContext();

		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index(int? page)
		{
			if (HttpContext.Session.GetString("UserName") != null)
			{
				ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
				ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
				ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
				try
				{
					ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
				}
				catch (Exception ex)
				{
					ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
				}
			}
			int pageSize = 8;
			int pageNumber = page == null || page < 0 ? 1 : page.Value;
			var lstMovie = db.Movies.AsNoTracking().OrderBy(x => x.Title);
			PagedList<Movie> lst = new PagedList<Movie>(lstMovie, pageNumber, pageSize);
			return View(lst);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}