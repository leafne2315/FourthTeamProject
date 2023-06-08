namespace FourthTeamProject.Models.ViewModel
{
    public class CreateOrderViewModel
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int InvoiceId { get; set; }
        public int PayId { get; set; }
        public bool OrderStatus { get; set; }
        public string OrderAddress { get; set; }
        public string OrderMemberName { get; set; }
        public string OrderMemberPhone { get; set; }
        public string OrderMemberEmail { get; set; }
        public string OrderNo { get; set; }
    }
    public class OrderPaymentStatusModel
    {
        public string OrderNo { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
