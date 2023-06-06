using FourthTeamProject.PetHeavenModels;

namespace FourthTeamProject.Models.ViewModel
{
    public class ProductEnterpriseViewModel
    {
        public int ProductID { get; set; }
        public String? ProductCatagoryName { get; set; }
        public String? ProductTypeName { get; set; }
        public string? ProductName { get; set; }
        public string? ProductSpecification { get; set; }
        public string? ProductContent { get; set; }
        public int UnitPrice { get; set; }
        public int Stock { get; set; }

        public bool ProductStatus { get; set; }
        public ProductCatagory? ProductCatagory { get; set; }
        public ProductType? ProductType { get; set; }

    }
}
