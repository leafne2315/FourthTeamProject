using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
    public class ProductOrderDetailViewModel
    {
        public IEnumerable<ProductOrderDetail> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
