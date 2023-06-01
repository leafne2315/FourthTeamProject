namespace FourthTeamProject.Models.ViewModel
{
    public class HotelOrderDetailViewModel
    {
        public int? HotelOrderId { get; set; }
        public int? HotelId { get; set; }
        public DateTime? CheckIntime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? OrderAmount { get; set; }
        public int? UnitPrice { get; set; }
        public bool? DetailStatus { get; set; }
        public string HotelName { get; set; }
        public string HotelCatagoryName { get; set; }
    }
}
