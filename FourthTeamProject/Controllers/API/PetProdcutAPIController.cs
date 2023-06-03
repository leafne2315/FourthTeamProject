using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FourthTeamProject.Controllers.API
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class PetProdcutAPIController : ControllerBase
	{

		readonly PetHeavenDbContext _db;

		public PetProdcutAPIController(PetHeavenDbContext db)
		{
			_db = db;
		}

		[HttpPost]
		public IActionResult GetProduct()
		{
            var result = _db.Product.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductTypeId = p.ProductTypeId,
                ProductStatus = p.ProductStatus,
                ProductName = p.ProductName,
                ProductContent = p.ProductContent,
                ProductCatagoryId = p.ProductCatagoryId,
                UnitPrice = p.UnitPrice,
                ProductImage = p.ProductImage,
                Amount = 1
            });
            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetMember([FromBody] GetMember request)
        {
            var member = _db.Member.FirstOrDefault(x => x.MemberEmail == request.MemberEmail);
            if (member != null)
            {
                var result = new MemberViewModel
                {
                    MemberId = member.MemberId,
                    MemberName = member.MemberName,
                    MemberPhone = member.MemberPhone,
                    MemberEmail = request.MemberEmail,
                    MemberAddress = member.MemberAddress,
                    MemberPoint = member.MemberPoint
                };
                return Ok(result);
            }
            return NoContent();
        }


        [HttpPost]
        public IActionResult CreatOrder([FromBody] ProductOrder request)
        {

            var order = _db.ProductOrder.Add(request);

            _db.SaveChanges();

            return Ok(request.OrderId);

        }










    }
}
