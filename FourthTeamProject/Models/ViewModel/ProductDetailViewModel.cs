namespace FourthTeamProject.Models.ViewModel
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string ProductContent { get; set; }
        public int UnitPrice { get; set; }
        public int Stock { get; set; }
        public int ProductTypeId { get; set; }

        public int Amount { get; set; }

    }
}
