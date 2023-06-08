using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManageController : Controller
    {
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
