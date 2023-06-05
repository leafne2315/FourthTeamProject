using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("/api/ProductEnterprise/[action]")]
    [ApiController]
    public class ProductEnterpriseAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public ProductEnterpriseAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductEnterpriseViewModel> GetProduct()
        {
            var temp = _context.Product.Include(x=>x.ProductCatagory)
                .Select(option => new ProductEnterpriseViewModel
            {
                    ProductCatagoryName = option.ProductCatagory.ProductCatagoryName,
                    ProductTypeName=option.ProductType.ProductTypeName,
                    ProductID = option.ProductId,
                    ProductName = option.ProductName,
                    ProductSpecification = option.ProductSpecification,
                    ProductContent = option.ProductContent,
                    UnitPrice = option.UnitPrice,
                    Stock=option.Stock,
                });
            return temp;
        }

        [HttpDelete("{PoductID}")]
        public async Task<string> DeleteProduct(int ProductID)
        {
            var Product = await _context.Product.FindAsync(ProductID);
            if (Product == null)
            {
                return "無此商品，不可刪除，請洽談工程師處理!!";
            }

            _context.Product.Remove(Product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆商品已存有其他資訊，不可刪除!!";
            }

            return "商品刪除成功!!";
        }

        [HttpPut("{ProductID}")]
        public async Task<string> PutProduct(int ProductID, [FromBody] ProductEnterpriseViewModel Product)
        {
            if (ProductID != Product.ProductID)
            {
                return "商品編號錯誤";
            }
            Product DTO = await _context.Product.FindAsync(ProductID);
            DTO.ProductId = Product.ProductID;
            DTO.ProductName = Product.ProductName;
            DTO.ProductSpecification = Product.ProductSpecification;
            DTO.ProductContent = Product.ProductContent;
            DTO.UnitPrice = Product.UnitPrice;
            DTO.Stock = Product.Stock;
            _context.Entry(DTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(ProductID))
                {
                    return "商品編號不存在";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        private bool ProductExists(int ProductID)
        {
            return (_context.Product?.Any(e => e.ProductId == ProductID)).GetValueOrDefault();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetProductPage()
        //{
        //    var Temp = await _context.ProductCatagory
        //        .Select(option => new ProductEnterpriseViewModel
        //        {
        //            ProductCatagoryName = option.ProductCatagoryName,
        //            TypeName = _context.ProductType.FirstOrDefault(t => t.ProductTypeId == option.ProductCatagoryId)?.TypeName
        //        })
        //        .ToListAsync();

        //    return Ok(Temp);
        //}
    }
}
