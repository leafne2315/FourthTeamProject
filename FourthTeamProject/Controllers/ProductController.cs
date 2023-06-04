using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FourthTeamProject.Controllers
{
	public class ProductController : Controller
	{
        private readonly PetHeavenDbContext _db;

	
		public IActionResult Index()
		{
			var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			if (email != null)
			{
				ViewBag.Email = email;
			}
			return View();
		}

		public IActionResult Order() 
		{ 
			return View();
		}


		public IActionResult Check()
		{
			var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
			if (email != null)
			{
				ViewBag.Email = email;
				return View();
			}
			return Redirect("/Member/Login");

		}

        public IActionResult OrderDone()
        {
            return View();
        }



        public IActionResult ProductDetail()
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (Request.Query.TryGetValue("Id", out var productId))
            {
				ViewBag.Email = email;
                ViewBag.productId = productId;
                return View();
            }
            return RedirectToAction("Index");
        }

        





    }
}
