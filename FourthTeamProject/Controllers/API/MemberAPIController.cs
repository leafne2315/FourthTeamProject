using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NuGet.Packaging.PackagingConstants;

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

				var member =_db.Member.Where(m => m.MemberName == memberId).Select(m => new MemberCentreViewModel
				{
					Id = m.MemberId,
					Account = m.MemberAccount,
					Name = m.MemberName,
					Email = m.MemberEmail,
					Address = m.MemberAddress,
					Password = m.MemberPassword,
					Birthday = m.MemberBirthday,
					Phone=m.MemberPhone
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
		[HttpGet]
		public object GetMemberOrder()
		{
			var member = User.FindFirstValue(ClaimTypes.Name);
			var memberId = _db.Member.Where(o => o.MemberName == member).ToList();            
			if (memberId != null)
			{
				var order = _db.ProductOrder.Include(x => x.ProductOrderDetail).Where(x=>x.MemberId== memberId[0].MemberId).Select(x => new
				{
					order = new
					{
						orderid = x.OrderId,
						ordername = x.OrderMemberName,
						orderdate = x.OrderDate,
						orderstatus = x.OrderStatus,
						orderaddress = x.OrderAddress,
					},
					OrderDetail = x.ProductOrderDetail
				}).ToList();
				return Ok(order);
			}
			return Unauthorized();
		}

		[HttpGet]
		public object GetMemberOrderDetail()
		{
            //var member = User.FindFirstValue(ClaimTypes.Name);
            //var memberId = _db.Member.Where(o => o.MemberName == member).ToList();
			var dtail = _db.ProductOrderDetail.Include(x => x.Order).Select(x => new
				{
					orderdtail = new
					{
						orderid = x.OrderId,
						detailname=x.ProductName,
						detailcontent=x.ProductContent,
						detailcatagoryname=x.ProductCatagoryName,
						detailunitprice=x.UnitPrice,
						detailquantity=x.Quantity
					},
					//order=x.Order
					
					
				}).ToList();
				return Ok(dtail);			
		}

		[HttpGet]
		public object GetMemberPet()
		{
            var member = User.FindFirstValue(ClaimTypes.Name);
            var memberId = _db.Member.Where(o => o.MemberName == member).ToList();           
            var pet = _db.Pet.Include(x => x.Member).Include(x => x.PetType).Where(x => x.MemberId == memberId[0].MemberId).Select(x => new
			{
				pet = new
				{
					petId = x.PetId,
					petName = x.PetName,
					petGender = x.PetGender,
					petBirthday = x.PetBirthday,
					petVariety = x.PetVariety
				},
				pettype = x.PetType,
				member = x.Member
			}).ToList();

			return Ok(pet);
        }        

            public bool deletePet([FromBody] DeleteDto model){
			var pet= _db.Pet.FirstOrDefault(x => x.PetName == model.Id);
			if (pet == null) return false;
			_db.Pet.Remove(pet);
			_db.SaveChanges();
			return true;
		}
	}
	public class DeleteDto
	{
		public string Id { get; set; }
	}
}
