﻿using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
    public class PetSalonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DogSalon()
        {
            return View();
        }

		public IActionResult CatSalon()
		{
			return View();
		}
	}


}
