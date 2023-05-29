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
    }
}
