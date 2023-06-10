using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FourthTeamProject.Controllers
{


    public class EmployeesController : Controller
    {
        private readonly PetHeavenDbContext _db;


        public EmployeesController(PetHeavenDbContext context)
        {
            this._db = context;
        }


        public IActionResult EmployeeLogin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> EmployeeLogin(EmployeeLoginViewModel model)
        {

            var user = _db.Employees.FirstOrDefault(x => x.EmployeeEmail == model.EmployeeEmail &&
             x.EmployeePassword == model.EmployeePassword);

            if (user == null)
            {
                ViewBag.Error = "帳號密碼錯誤";
                return View("EmployeeLogin");
            }


            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Name, user.EmployeeName),
               new Claim(ClaimTypes.Role, user.EmployeeRole),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);  //夾帶一個cookie出去
            return RedirectToAction("ProductCRUD", "ProductManage", new { area = "Admin" });
        }
        public async Task<IActionResult> EmployeeLogout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}