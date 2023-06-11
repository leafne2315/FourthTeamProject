using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult CreatOrder([FromBody] CreateOrderViewModel request)
        {

            var order = new ProductOrder
            {
                MemberId = request.MemberId,
                InvoiceId = request.InvoiceId,
                PayId = request.PayId,
                OrderStatus = request.OrderStatus,
                OrderAddress = request.OrderAddress,
                OrderMemberName= request.OrderMemberName,
                OrderMemberPhone= request.OrderMemberPhone,
                OrderMemberEmail= request.OrderMemberEmail,
                OrderNo= request.OrderNo,
                OrderDate= DateTime.Now,
            };
            _db.ProductOrder.Add(order);
            _db.SaveChanges();


            return Ok(order.OrderId);

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
                var result = new ProductViewModel
                {
                    ProductId = detail.ProductId,
                    ProductTypeId = detail.ProductTypeId,
                    ProductStatus = detail.ProductStatus,
                    ProductName = detail.ProductName,
                    ProductContent = detail.ProductContent,
                    ProductCatagoryId = detail.ProductCatagoryId,
                    UnitPrice = detail.UnitPrice,
                    ProductImage = detail.ProductImage,
                    Amount = 1
                };
                return Ok(result);
            }
            return NoContent();

        }
        public IActionResult MaybeLikProoduct([FromQuery] int id)
        {
            //隨機四筆
            var count = 4;
            var randomProducts = _db.Product.Where(p => p.ProductTypeId == id)
            .OrderBy(x => Guid.NewGuid()).Take(count).Select(x=> new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductTypeId = x.ProductTypeId,
                ProductStatus = x.ProductStatus,
                ProductName = x.ProductName,
                ProductContent = x.ProductContent,
                ProductCatagoryId = x.ProductCatagoryId,
                UnitPrice = x.UnitPrice,
                ProductImage = x.ProductImage,
                Amount = 1
            });

            return Ok(randomProducts);

        }

        public IActionResult getInvoice()
        {
            var invoice = _db.Invoice;
            return Ok(invoice);
        }
       
        public IActionResult getPayment()
        {
            var payment = _db.Payment;
            return Ok(payment);
        }

        [HttpPut("/api/PetProductAPI/UpdateOrderPaymentStatus/{orderNo}")]
        public IActionResult UpdateOrderPaymentStatus(string orderNo, [FromBody] OrderPaymentStatusModel paymentStatus)
        {
                var existingOrder = _db.ProductOrder.FirstOrDefault(o => o.OrderNo == paymentStatus.OrderNo);

                if (existingOrder == null)
                {
                    return NotFound("訂單不存在");
                }
                existingOrder.OrderStatus = paymentStatus.PaymentStatus;
                _db.ProductOrder.Update(existingOrder);
                _db.SaveChanges();
                return Ok("付款成功");
            
        }

        public IActionResult Recommended()
        {
            var count = 2;
            var randomProducts = _db.Product.OrderBy(x => Guid.NewGuid()).Take(count).Select(x => new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductTypeId = x.ProductTypeId,
                ProductStatus = x.ProductStatus,
                ProductName = x.ProductName,
                ProductContent = x.ProductContent,
                ProductCatagoryId = x.ProductCatagoryId,
                UnitPrice = x.UnitPrice,
                ProductImage = x.ProductImage,
                Amount = 1                
            });

            return Ok(randomProducts);

        }
        [HttpGet("/Product/GetImages/{productId}")]
        public IActionResult GetProductImages(int productId)
        {
            var productImages = _db.ProductImage.Where(p => p.ProductId == productId).Select(p => new
            {
                productId = p.ProductId,
                ImageId = p.ProductImageId,
                ImagePath = p.ProductImagePath,
            }) ;
            return Ok(productImages);
        }


    }
}
