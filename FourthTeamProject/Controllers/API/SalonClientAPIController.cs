using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var temp = _context.SalonSolution.Where(s=>s.SalonSolutionId== salonSolutionId)
                .Select(option => new SalonViewModel
            {
                SalonSolutionId = option.SalonSolutionId,
                SalonSolutionName = option.SalonSolutionName,
                SalonSolutionDiscription = option.SalonSolutionDiscription,
                SalonSolutionPrice=option.SalonSolutionPrice,
            });
            return temp;
        }
    }
}
