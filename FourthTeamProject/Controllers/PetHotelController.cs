﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FourthTeamProject.Controllers
{
    public class PetHotelController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}