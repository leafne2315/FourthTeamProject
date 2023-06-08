using FourthTeamProject.PetHeavenModels;


namespace FourthTeamProject.Areas.Admin.ViewModels
{
    public class ProductOrderViewModel
    {
        public IEnumerable<ProductOrder> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}

