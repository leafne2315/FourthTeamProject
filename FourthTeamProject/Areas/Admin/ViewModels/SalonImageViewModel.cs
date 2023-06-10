namespace FourthTeamProject.Areas.Admin.ViewModels
{
    public class SalonImageViewModel
    {
        public int SalonId { get; set; }
        public string? SalonName { get; set; }
        public string? SalonCatagoryName { get; set; }
        public byte[]? SalonImagePath { get; set; }
    }
}
