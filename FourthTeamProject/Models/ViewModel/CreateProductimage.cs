namespace FourthTeamProject.Models.ViewModel
{
    public class CreateProductimage
    {
        public int ProductID { get; set; }
        public int ProductCatagoryId { get; set; }
        public string? ProductCatagoryName { get; set; }
        public int ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }
        public string? ProductName { get; set; }
    }
}
