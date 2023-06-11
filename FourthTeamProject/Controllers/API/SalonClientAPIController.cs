using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SalonViewModel = FourthTeamProject.Models.ViewModel.SalonViewModel;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/SalonClient/[action]")]
    [ApiController]
    public class SalonClientAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public SalonClientAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SalonViewModel> GetSalonClient()
        {
            var temp = _context.SalonSolution.Select(option => new SalonViewModel
            {
                SalonSolutionId = option.SalonSolutionId,
                SalonSolutionName = option.SalonSolutionName,
                SalonSolutionDiscription = option.SalonSolutionDiscription,
                SalonSolutionPrice = option.SalonSolutionPrice,
            });
            return temp;
        }
        [HttpGet("{salonSolutionId}")]
        public IActionResult GetSalonData(int salonSolutionId)
        {
            var salonIds = _context.SalonSolutionSalon
                .Where(ss => ss.SalonSolutionId == salonSolutionId)
                .Select(ss => ss.SalonId)
                .ToList();

            var salons = _context.Salon
                .Where(s => salonIds.Contains(s.SalonId))
                .Select(s => new { s.SalonName, s.SalonImagePath })
                .ToList();

            return Ok(salons);
        }
        public IActionResult GetCurrentMember()
        {
            if (User.Identity.IsAuthenticated)
            {
                var memberId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                var member = _context.Member.Where(m => m.MemberName == memberId).Select(m => new MemberCentreViewModel
                {
                    Id = m.MemberId,
                    Account = m.MemberAccount,
                    Name = m.MemberName,
                    Email = m.MemberEmail,
                    Address = m.MemberAddress,

                    Birthday = m.MemberBirthday,
                    Phone = m.MemberPhone
                }).FirstOrDefault();
                return Ok(member);
            }
            else
            {
                return Ok(null);
            }
        }
        [HttpGet("{salonSolutionId}")]
        public IEnumerable<SalonViewModel> GetSalonSolutionOrder(int salonSolutionId)
        {
            var temp = _context.SalonSolution.Where(s => s.SalonSolutionId == salonSolutionId)
                .Select(option => new SalonViewModel
                {
                    SalonSolutionId = option.SalonSolutionId,
                    SalonSolutionName = option.SalonSolutionName,
                    SalonSolutionDiscription = option.SalonSolutionDiscription,
                    SalonSolutionPrice = option.SalonSolutionPrice,
                });
            return temp;
        }



        [HttpPost]
        public async Task<string> GetorderDetail([FromBody] SalonOrderDetailViewModel OrderDetail)
        {
            int SalonSolutionId = GetSalonSolutionId(OrderDetail.SalonSolutionName);
            var maxid = _context.SalonOrder.Max(x => x.SalonOrderId);
            var maxDetailID = _context.SalonOrderDetail.Max(x => x.SalonOrderDetailId);
            var SalonOrderDetail = new SalonOrderDetail()
            {
                SalonOrderDetailId = maxDetailID+1,
                Appointment = OrderDetail.Appointment,
                DetailStatus = true,
                UnitPrice = OrderDetail.UnitPrice,
                SalonOrderId = maxid + 1,
                SalonCatagoryName = OrderDetail.SalonCatagoryName,
                SalonSolutionName = OrderDetail.SalonSolutionName,
                SalonSolutionId = SalonSolutionId,
            };

            _context.SalonOrderDetail.Add(SalonOrderDetail);
            _context.SaveChanges();
            return "完成";

        }

        private int GetSalonSolutionId(String salonSolutionName)
        {
            var SalonSolution = _context.SalonSolution.FirstOrDefault(s => s.SalonSolutionName == salonSolutionName);
            return SalonSolution.SalonSolutionId;
        }

        [HttpPost]
        public async Task<string> Getorder([FromBody] PetSalonOrderViewModel Order)
        {

            var maxid = _context.SalonOrder.Max(x => x.SalonOrderId);
            var SalonOrder = new SalonOrder()
            {
                SalonOrderId = maxid+1,
                SalonOrderDate = DateTime.Now,
                MemberId = Order.MemberID,
                PayId = Order.PayID,
                InvoiceId = Order.InvoiceID,
                OrderStatus = true,
                OrderMemberName = Order.OrderMemberName,
                OrderMemberPhone = Order.OrderMemberPhone,
                OrderMemberEmail= Order.OrderMemberEmail,
            };

            _context.SalonOrder.Add(SalonOrder);
            _context.SaveChanges();
            return "預約完成!!";

        }
    }
}
