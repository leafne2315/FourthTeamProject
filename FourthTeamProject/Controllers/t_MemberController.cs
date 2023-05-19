using FourthTeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FourthTeamProject.Controllers
{
    public class t_MemberController : Controller
    {
        private readonly PetHeavenDbContext _db;

        public t_MemberController(PetHeavenDbContext context)
        {
            this._db = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(MemberLoginViewModel model)
        {
            var user = _db.TMember.FirstOrDefault(x => x.CMemberAccount == model.MemberAccount &&
             x.CMemberPassword == model.MemberPassword);

            if (user == null)
            {
                ViewBag.Error = "帳號密碼錯誤";
                return View("Login");
            }

            var claims = new List<Claim>()
            {
            new Claim(ClaimTypes.Email, user.CMemberEmail),
        
             };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);  //夾帶一個cookie出去
            return RedirectToAction("Index", "Home");
        }
    }
}