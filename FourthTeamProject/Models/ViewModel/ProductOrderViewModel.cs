using FourthTeamProject.PetHeavenModels;


namespace FourthTeamProject.Models.ViewModel
{
    public class ProductOrderViewModel
    {
        public IEnumerable<ProductOrder> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}

