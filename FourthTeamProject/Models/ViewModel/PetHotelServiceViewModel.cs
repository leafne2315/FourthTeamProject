using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
    public class PetHotelServiceViewModel
    {
        public int HotelId { get; set; }
        public int HotelServiceId { get; set; }
        public HotelService HotelService { get; set; }
    }
}
