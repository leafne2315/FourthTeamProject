using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var petHotelDB = petHeavenDb.HotelService.ToList();
            var petHotelList = petHotelDB.Select(data => new PetHotelViewModel()
            {
                Service = data.CService,
                RoomTypeId = data.CRoomTypeId,
                PetSizeId = data.CPetSizeId,
                UnitPrice = data.CUnitPrice,
            });


            return Ok(petHotelList);
        }


        //public IActionResult getRoomDataById(int roomId)
        //{

        //}
    }
}
