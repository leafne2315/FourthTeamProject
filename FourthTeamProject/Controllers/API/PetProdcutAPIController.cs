using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
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

        public IActionResult Index(ProductViewModel product)
        {
            var result = _db.Product.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductContent = p.ProductContent,
                ProductImage = p.ProductImage,
                ProductCatagory = p.ProductCatagory,
            });

            return Ok(result);



        }
        

        









    }
}
