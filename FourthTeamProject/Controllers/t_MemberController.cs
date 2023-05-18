using FourthTeamProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers
{
    public class t_MemberController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(MemberLoginViewModel model)
        {
            return Ok(model.t_MemberAccount+model.t_MemberPassword);
        }
    }
}
