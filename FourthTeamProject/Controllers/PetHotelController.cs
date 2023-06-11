using FourthTeamProject.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FourthTeamProject.Controllers
{
    public class PetHotelController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult HotelDetail(int hotelId)
        {
            return View(hotelId);
        }
        
        public IActionResult HotelOrder(int hotelId) 
        {
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            // 產生測試資訊
            ViewData["MerchantID"] = Config.GetSection("MerchantID").Value;//商店代號
            ViewData["MerchantOrderNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");  //訂單編號
            ViewData["ExpireDate"] = DateTime.Now.AddDays(3).ToString("yyyyMMdd"); //繳費有效期限
            ViewData["ReturnURL"] = $"{Request.Scheme}://{Request.Host}/Product/OrderDone"; //支付完成返回商店網址
                                                                                            //ViewData["CustomerURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}Home/CallbackCustomer"; //商店取號網址
                                                                                            //ViewData["NotifyURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}Home/CallbackNotify"; //支付通知網址
                                                                                            //ViewData["ClientBackURL"] = $"{Request.Scheme}://{Request.Host}{Request.Path}"; //返回商店網址

            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                ViewBag.Email = email;
                return View();
            }
            return Redirect("/Member/Login");
        }

        
    }
}
