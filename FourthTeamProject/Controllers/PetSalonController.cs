using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace FourthTeamProject.Controllers
{
    public class PetSalonController : Controller
    {
        
        public IActionResult SalonIndex()
        {
            return View();
        }
        public IActionResult SalonOrderCheck()
        {
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            // 產生測試資訊
            ViewData["MerchantID"] = Config.GetSection("MerchantID").Value;//商店代號
            ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
            ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限
            ViewData["ReturnURL"] = $"{Request.Scheme}://{Request.Host}/Product/OrderDone"; //支付完成返回商店網址

            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                ViewBag.Email = email;
                return View();
            }
            return Redirect("/Member/Login");
        }

        public IActionResult SalonDone()
        {
            return View();
        }

    }


}
