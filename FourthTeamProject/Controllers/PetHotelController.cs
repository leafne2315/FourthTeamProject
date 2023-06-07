using FourthTeamProject.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

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
            return View(); 
        }

        public IActionResult SendDataToOrder(HotelOrderDetailViewModel orderDetailData)
        {
            string serializedData = JsonConvert.SerializeObject(orderDetailData);
            HttpContext.Session.SetString("OrderData", serializedData);

            return Ok(serializedData);
        }
    }
}
