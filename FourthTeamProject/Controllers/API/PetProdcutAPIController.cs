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












	}
}
