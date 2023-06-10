namespace FourthTeamProject.Models.ViewModel
{
    public class PetSalonOrderViewModel
    {
        public int SalonOrderID { get; set; }
        public DateTime SalonOrderDate { get; set; }
        public int MemberID { get; set; }
        public int PayID { get; set; }
        public int InvoiceID { get; set; }
        public bool OrderStatus { get; set; }
        public string OrderMemberName { get; set; }
        public string OrderMemberPhone { get; set; }
        public string OrderMemberEmail { get; set; }
    }
}
