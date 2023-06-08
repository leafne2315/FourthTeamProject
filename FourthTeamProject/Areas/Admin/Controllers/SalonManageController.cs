using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalonManageController : Controller
    {
        public IActionResult SalonCRUD()
        {
            return View();
        }
        public IActionResult SalonSolutionCRUD()
        {
            return View();
        }
        public IActionResult SalonSolutionSalonCRUD()
        {
            return View();
        }
    }
}
