using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
	public class ProductController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
