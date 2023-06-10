namespace FourthTeamProject.Models.ViewModel
{
    public class SalonOrderDetailViewModel
    {
        public int SalonOrderDetailID { get; set; }
        public DateTime Appointment { get; set; }
        public bool DetailStatus { get; set; }
        public int UnitPrice { get; set; }
        public int SalonOrderID { get; set; }
        public int SalonSolutionID { get; set; }
        public string? SalonCatagoryName { get; set; }
        public string? SalonSolutionName { get; set; }
    }
}
