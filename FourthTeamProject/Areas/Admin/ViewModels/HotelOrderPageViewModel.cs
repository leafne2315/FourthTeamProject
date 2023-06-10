using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
    public class HotelOrderPageViewModel
    {
        public IEnumerable<HotelOrder> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
