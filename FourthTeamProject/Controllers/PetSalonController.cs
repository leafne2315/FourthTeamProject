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
      
    }


}
