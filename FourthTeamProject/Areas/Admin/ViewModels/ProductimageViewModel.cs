using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Areas.Admin.ViewModels
{
    public class ProductimageViewModel
    {
        public int ProductImageID { get; set; }
        public string? ProductImagePath { get; set; }
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
    }
}
