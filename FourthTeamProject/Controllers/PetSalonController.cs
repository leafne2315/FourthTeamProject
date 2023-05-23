using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
    public class PetSalonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
