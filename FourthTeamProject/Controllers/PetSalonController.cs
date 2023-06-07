using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
    public class PetSalonController : Controller
    {
        public IActionResult SalonIndex()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SalonSolutionCRUD()
        {
            return View();
        }
        public IActionResult SalonCRUD()
        {
            return View();
        }
        public IActionResult SalonSolutionSalonCRUD()
        {
            return View();
        }
        public IActionResult HotelCRUD()
        {
            return View();
        }

        public IActionResult HotelServiceCRUD()
        {
            return View();
        }

        public IActionResult HotelServiceHotelCRUD()
        {
            return View();
        }
        public IActionResult HotelimageCRUD()
        {
            return View();
        }
        public IActionResult ProductCRUD()
        {
            return View();
        }
        public IActionResult ProductimageCRUD()
        {
            return View();
        }
    }


}
