using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NuGet.Protocol;
using System;
using System.Security.Cryptography;

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


       
        [HttpPost]
        public IActionResult CreatOrderDetail([FromBody] List<ProductViewModel> request,int orderId)
        {

            foreach (var item in request)
            {
                var CatagoryName = _db.ProductCatagory.Where(x => x.ProductCatagoryId == item.ProductCatagoryId).Select(x => x.ProductCatagoryName).SingleOrDefault();
                var result = new ProductOrderDetail()
                {
                    Quantity = item.Amount,
                    ProductName = item.ProductName,
                    ProductContent = item.ProductContent,
                    UnitPrice = item.UnitPrice,
                    ProductId = item.ProductId,
                    DetailStatus = true,
                    ProductCatagoryName = CatagoryName,
                    OrderId= orderId,
                };
                _db.ProductOrderDetail.Add(result);
                _db.SaveChanges();
            }
            return Ok();
        }

        public IActionResult ProductDetail([FromQuery] int id)
        {
            var detail = _db.Product.FirstOrDefault(x => x.ProductId == id);
            if (detail != null)
            {
                var result = new ProductDetailViewModel
                {
                    ProductId= id,
                    ProductName= detail.ProductName,
                    ProductContent= detail.ProductContent,
                    ProductSpecification= detail.ProductSpecification,
                    UnitPrice= detail.UnitPrice,
                    Stock= detail.Stock,
                    ProductTypeId = detail.ProductTypeId,
                    Amount=1
                };
                return Ok(result);
            }
            return NoContent();

        }
        public IActionResult MaybeLikProoduct([FromQuery] int id)
        {
            //隨機五筆
            var count = 4;
            var randomProducts = _db.Product.Where(p => p.ProductTypeId == id)
            .OrderBy(x => Guid.NewGuid()).Take(count).Select(x=> new ProductDetailViewModel
            {
                ProductId= x.ProductId,
                ProductName= x.ProductName,
                ProductContent= x.ProductContent,
                ProductSpecification= x.ProductSpecification,
                UnitPrice= x.UnitPrice,
                ProductTypeId= x.ProductTypeId
            });

            return Ok(randomProducts);

        }






    }
}
