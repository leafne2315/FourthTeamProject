using FourthTeamProject.Areas.Admin.ViewModels;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace FourthTeamProject.Areas.Admin.Controllers.API
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
            var temp = _context.Product.OrderByDescending(x => x.ProductStatus)
                .Select(option => new ProductEnterpriseViewModel
                {
                    ProductCatagoryName = option.ProductCatagory.ProductCatagoryName,
                    ProductTypeName = option.ProductType.ProductTypeName,
                    ProductID = option.ProductId,
                    ProductName = option.ProductName,
                    ProductSpecification = option.ProductSpecification,
                    ProductContent = option.ProductContent,
                    UnitPrice = option.UnitPrice,
                    Stock = option.Stock,
                    ProductStatus = option.ProductStatus,
                });
            return temp;
        }

        [HttpDelete("{ProductID}")]
        public async Task<string> DeleteProduct(int ProductID)
        {
            try
            {
                var Product = await _context.Product.FindAsync(ProductID);
                if (Product == null)
                {
                    return "商品錯誤，請洽談工程師處理!!";
                }
                _context.Product.Remove(Product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆商品有其他項目資料，不可刪除!!";
            }

            return "商品刪除成功!!";
        }

        [HttpPut("{ProductID}")]
        public async Task<string> DiscontinuedProduct(int ProductID)
        {
            var Product = await _context.Product.FindAsync(ProductID);
            if (Product == null)
            {
                return "無此商品，請洽談工程師處理!!";
            }
            Product.ProductStatus = false;
            _context.Update(Product);
            await _context.SaveChangesAsync();

            return "商品已停售!!";
        }

        [HttpPut("{ProductID}")]
        public async Task<string> ShelvesProduct(int ProductID)
        {
            var Product = await _context.Product.FindAsync(ProductID);
            if (Product == null)
            {
                return "無此商品，請洽談工程師處理!!";
            }
            Product.ProductStatus = true;
            _context.Update(Product);
            await _context.SaveChangesAsync();

            return "商品已重新上架!!";
        }

        [HttpPut("{ProductID}")]
        public async Task<string> PutProduct(int ProductID, [FromBody] ProductEnterpriseViewModel Product)
        {
            try
            {

                if (ProductID != Product.ProductID)
                {
                    return "商品編號錯誤";
                }
                int ProductCatagoryId = GetProductCatagoryId(Product.ProductCatagoryName);
                int ProductTypeId = GetProductTypeId(Product.ProductTypeName);
                Product DTO = await _context.Product.FindAsync(ProductID);
                DTO.ProductId = Product.ProductID;
                DTO.ProductName = Product.ProductName;
                DTO.ProductSpecification = Product.ProductSpecification;
                DTO.ProductContent = Product.ProductContent;
                DTO.UnitPrice = Product.UnitPrice;
                DTO.Stock = Product.Stock;
                DTO.ProductCatagoryId = ProductCatagoryId;
                DTO.ProductTypeId = ProductTypeId;
                _context.Update(DTO);
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

        
        public IEnumerable<ProductEnterpriseViewModel> GetProductCatagoryPage()
        {


            var temp = _context.ProductCatagory
            .Select(option => new ProductEnterpriseViewModel
            {
                ProductCatagoryName = option.ProductCatagoryName,
            });
            return temp;
        }
        
        public IEnumerable<ProductEnterpriseViewModel> GetProductTypePage()
        {


            var temp = _context.ProductType
            .Select(option => new ProductEnterpriseViewModel
            {
                ProductTypeName = option.ProductTypeName,
            });
            return temp;
        }

        [HttpPost]
        public async Task<string> CreateProduct([FromBody] ProductEnterpriseViewModel ProductData)
        {

            try
            {

                int ProductCatagoryId = GetProductCatagoryId(ProductData.ProductCatagoryName);
                int ProductTypeId = GetProductTypeId(ProductData.ProductTypeName);

                Product data = new Product
                {
                    ProductCatagoryId = ProductCatagoryId,
                    ProductTypeId = ProductTypeId,
                    ProductName = ProductData.ProductName,
                    ProductSpecification = ProductData.ProductSpecification,
                    ProductContent = ProductData.ProductContent,
                    UnitPrice = ProductData.UnitPrice,
                    Stock = ProductData.Stock,
                    ProductStatus = true,
                };

                _context.Product.Add(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(ProductData.ProductID))
                {
                    return "商品新增失敗!!";
                }
                else
                {
                    throw;
                }
            }
            return "商品新增完成!!";
        }

        [HttpPost]
        public async Task<string> CreateProductType([FromBody] ProductEnterpriseViewModel ProductTypeData)
        {


            ProductType data = new ProductType
            {
                ProductTypeName = ProductTypeData.ProductTypeName,
            };
            _context.ProductType.Add(data);
            await _context.SaveChangesAsync();
            return $"商品{data.ProductTypeName}項目新增完成!!";
        }

        private int GetProductTypeId(string? productTypeName)
        {
            var ProductType = _context.ProductType.FirstOrDefault(s => s.ProductTypeName == productTypeName);
            return ProductType.ProductTypeId;
        }

        private int GetProductCatagoryId(string? productCatagoryName)
        {
            var ProductCatagory = _context.ProductCatagory.FirstOrDefault(s => s.ProductCatagoryName == productCatagoryName);
            return ProductCatagory.ProductCatagoryId;
        }


    }
}
