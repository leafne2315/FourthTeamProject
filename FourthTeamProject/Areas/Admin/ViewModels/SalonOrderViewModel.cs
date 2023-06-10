using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
    public class SalonOrderViewModel
    {
        public IEnumerable<SalonOrder> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
