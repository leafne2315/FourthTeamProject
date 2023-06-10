using FourthTeamProject.Controllers.Extensions;
using FourthTeamProject.Models.DTO;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Immutable;
using System.Drawing;
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

        [HttpPut]
        public IActionResult updataMember([FromBody]Member member)
        {
            var membername = User.FindFirstValue(ClaimTypes.Name);
            var memberId = _db.Member.Where(o => o.MemberName == membername).ToList();
            var user = _db.Member.FirstOrDefault(x => x.MemberEmail == member.MemberEmail);
            try
            {
                // 根據會員的識別資訊（如會員ID或唯一標識）從資料庫中獲取會員物件
                var existingMember = _db.Member.FirstOrDefault(m => m.MemberId == memberId[0].MemberId);

                if (existingMember == null)
                {
                    return NotFound();
                }

                // 更新會員資訊                
                existingMember.MemberEmail = member.MemberEmail;
                existingMember.MemberAddress = member.MemberAddress;
                existingMember.MemberPhone=member.MemberPhone;
                existingMember.MemberBirthday = member.MemberBirthday;

                // 保存變更
                _db.SaveChanges();

                return Ok(existingMember);
            }
            catch (Exception ex)
            {
                // 處理錯誤，回傳錯誤訊息
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
		
        public ApiResultDTO createPet([FromBody] createDto model)
        {
            var memberName = User.FindFirstValue(ClaimTypes.Name);
            var member = _db.Member.FirstOrDefault(o => o.MemberName == memberName);
			if (member != null) {
                var memberId = member.MemberId;
                _db.Pet.Add(new Pet
                {

					MemberId = memberId,
                    PetName = model.petName,
                    PetGender = model.petGender.ToString(),
                    PetBirthday = model.petBirthday,
                    PetVariety = model.petVariety,
                    PetTypeId = model.petTypeId,
                });
                _db.SaveChanges();
				var maxnum = _db.Pet.Max(x=>x.PetId);
				var data = _db.Pet.Include(x => x.Member).Include(x => x.PetType).First(x=>x.PetId == maxnum);


                return new ApiResultDTO
				{
                    Status = true,
					Message= new
                    {
                        pet = new
                        {
                            petId = data.PetId,
                            petName = data.PetName,
                            petGender = data.PetGender,
                            petBirthday = data.PetBirthday,
                            petVariety = data.PetVariety
                        },
                        pettype = data.PetType,
                        member = data.Member
                    }
            };
            }

            return new ApiResultDTO
            {
                Status = false,
                Message = "新增有誤"
            };

        }


        public bool deletePet([FromBody] DeleteDto model){
			var pet= _db.Pet.FirstOrDefault(x => x.PetName == model.Id);
			if (pet == null) return false;
			_db.Pet.Remove(pet);
			_db.SaveChanges();
			return true;
		}


        public ApiResultDTO Edit([FromBody] EdutDto model)
        {
            var memberName = User.FindFirstValue(ClaimTypes.Name);
            var member = _db.Member.FirstOrDefault(o => o.MemberName == memberName);
            var memberId = member.MemberId;
            var result = _db.Pet.FirstOrDefault(x => x.MemberId == memberId);
            if (result == null)
            {
                return new ApiResultDTO() { Status = false, Message = "修改有誤" };
            }
            result.PetName = model.petName;
            result.PetGender = model.petGender.ToString();
            result.PetVariety = model.petVariety;
            result.PetBirthday = model.petBirthday;
            result.PetTypeId = model.petTypeId;
            _db.SaveChanges();
            return new ApiResultDTO() { Status = true, Message = "成功" };

        }
        //public ApiRequestDto Edit([FromBody] CreateDto model)
        //{

        //}

        [HttpGet]
        public List<SelectListItem> PetTypeEnum()
        {

            var xxx= Enum.GetValues(typeof(Models.Enums.PetTypeEnum))
                .Cast<Models.Enums.PetTypeEnum>()
                .Select(petType => new SelectListItem
                {
                    Text = petType.GetDescription(),
                    Value = ((int)petType).ToString(),
                })
                .ToList();
			return xxx;
        }
        [HttpGet]
        public List<SelectListItem> TypeName()
        {

            var zzz= _db.PetType.Select(x => new SelectListItem
            {
                Text = x.PetTypeName,
                Value = x.PetTypeId.ToString()
            })
                .ToList();
			return zzz;
        }




    }






    

   
}
