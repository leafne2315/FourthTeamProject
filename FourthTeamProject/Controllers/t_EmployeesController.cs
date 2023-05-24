using FourthTeamProject.Models;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FourthTeamProject.Controllers
{
    public class t_EmployeesController : Controller
    {
        private readonly PetHeavenDbContext _db;

        public t_EmployeesController(PetHeavenDbContext context)
        {
            this._db = context;
        }


        public IActionResult EmployeeLogin()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult EmployeeSystem()
        {

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Productmanagement()
        {
            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> EmployeeLogin(EmployeeLoginViewModel model)
        //{
        
        //    var user = _db.TEmployees.FirstOrDefault(x => x.CEmployeeEmail == model.EmployeeEmail &&
        //     x.CEmployeePassword == model.EmployeePassword);

        //    if (user == null)
        //    {
        //        ViewBag.Error = "帳號密碼錯誤";
        //        return View("EmployeeLogin");
        //    }


        //    var claims = new List<Claim>()
        //    {
        //         new Claim(ClaimTypes.Name, user.CEmployeeName),
        //       new Claim(ClaimTypes.Role, user.CEmployeeRole),
        //    };

        //    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //    await HttpContext.SignInAsync(claimsPrincipal);  //夾帶一個cookie出去
        //    return RedirectToAction("EmployeeSystem", "t_Employees");
        //}
        public async Task<IActionResult> EmployeeLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
