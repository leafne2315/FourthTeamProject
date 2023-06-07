using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetHotelAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext petHeavenDb;

        public PetHotelAPIController(PetHeavenDbContext petHeavenDb)
        {
            this.petHeavenDb = petHeavenDb;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var petHotelDB = petHeavenDb.Hotel.Include(h=>h.HotelCatagory).ToList();
            var petHotelList = petHotelDB.Select(data => new PetHotelViewModel()
            {
                ID = data.HotelId,
                CatagoryID = data.HotelCatagoryId,
                HotelName = data.HotelName,
                UnitPrice = data.UnitPrice,
                HotelContent = data.HotelContent,
                HotelContentDetail = data.HotelContentDetail,
                HotelImage = data.HotelImage,
                HotelTypeName = data.HotelCatagory.HotelCatagoryName,
            }) ;


            return Ok(petHotelList);
        }

        
        [HttpGet("HotelDetail/{hotelId}")]
        public IActionResult RoomDetail(int hotelId)
        {
            var petHotelDB = petHeavenDb.Hotel.Include(h => h.HotelCatagory).ToList();
            var roomDatas = petHotelDB.Where(h => h.HotelId == hotelId).Select(data => new PetHotelViewModel()
            {
                ID = data.HotelId,
                CatagoryID = data.HotelCatagoryId,
                HotelName = data.HotelName,
                UnitPrice = data.UnitPrice,
                HotelContent = data.HotelContent,
                HotelContentDetail = data.HotelContentDetail,
                HotelImage = data.HotelImage,
                HotelTypeName = data.HotelCatagory.HotelCatagoryName,
            });

            var HotelSelected = roomDatas.FirstOrDefault();
            return Ok(HotelSelected);
        }

        [HttpGet("HotelDetail/GetService/{hotelId}")]
        public IActionResult HotelService(int hotelId)
        {
            var services = petHeavenDb.HotelToService.Include(s => s.HotelService).ToList();
            var serviceOfHotel = services.Where(h => h.HotelId == hotelId).Select(data => new PetHotelServiceViewModel()
            {
                HotelId=data.HotelId,
                HotelServiceId=data.HotelServiceId,
                HotelService = data.HotelService,
            });

            return Ok(serviceOfHotel);
        }

        [HttpGet("HotelDetail/GetImages/{hotelId}")]
        public IActionResult HotelImagesGet(int hotelId)
        {
            var images = petHeavenDb.HotelImage.Where(h=>h.HotelId==hotelId).Select(data=>new PetHotelImageViewModel() 
            {
                HotelImageId=data.HotelImageId,
                HotelImagePath=data.HotelImagePath,
            });
            return Ok(images);
        }

        [HttpPost("HotelOrderCreate")]
        public IActionResult CreateHotelOrder([FromBody]HotelOrderViewModel orderData)
        {
            var hotelOrder = new HotelOrder()
            {
                HotelOrderDate = DateTime.Now,
                MemberId = orderData.MemberId,
                PayId = orderData?.PayId,
                InvoiceId = orderData.InvoiceId, 
                OrderStatus = true,
            };

            petHeavenDb.HotelOrder.Add(hotelOrder);
            petHeavenDb.SaveChanges();

            return Ok(hotelOrder);
        }

        [HttpPost("OrderDetailCreate")]
        public IActionResult CreateHotelOrderDetail([FromBody]HotelOrderDetailViewModel vModel)
        {
            var hotelOrderDetail = new HotelOrderDetail()
            {
                HotelOrderId = vModel.HotelOrderId,
                HotelId = vModel.HotelId,
                CheckIntime = vModel.CheckIntime,
                CheckOutTime = vModel.CheckOutTime,
                OrderAmount = vModel.OrderAmount,
                UnitPrice = vModel.UnitPrice,
                DetailStatus = vModel.DetailStatus,
                HotelName = vModel.HotelName,
                HotelCatagoryName = vModel.HotelCatagoryName,
            };

            petHeavenDb.HotelOrderDetail.Add(hotelOrderDetail);
            petHeavenDb.SaveChanges();
            return Ok(hotelOrderDetail);
        }


    }
}
