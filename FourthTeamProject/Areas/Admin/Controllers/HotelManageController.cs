using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelManageController : Controller
    {
        public IActionResult HotelCRUD()
        {
            return View();
        }
        public IActionResult HotelimageCRUD()
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
    }
}
