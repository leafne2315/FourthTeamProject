using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/member/[action]")]
    [ApiController]
    public class MemberAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _db;
        public MemberAPIController(PetHeavenDbContext db)
        {
            _db = db;
        }        
            [HttpGet]
        public IActionResult GetCurrentMember()
        {
            // 檢查是否存在有效的會話或 JWT
            if (User.Identity.IsAuthenticated)
            {
                // 根據會員識別資訊檢索會員資訊
                var memberId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
				
				var member = _db.Member.Where(m => m.MemberName == memberId).Select(m => new MemberViewModel
				{
					Id = m.MemberId,
					Account = m.MemberAccount,
					Name = m.MemberName,
					Email = m.MemberEmail,
					Address = m.MemberAddress,
					Password=m.MemberPassword,
				}).FirstOrDefault();

				if (member != null)
                {
                    // 返回會員資訊
                    return Ok(member);
                }
            }

            // 如果會員未登入或未找到相應的會員資訊，返回未授權狀態
            return Unauthorized();
        }
    }
}
