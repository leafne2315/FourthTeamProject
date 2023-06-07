using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/ProductimageEnterprise/[action]")]
    [ApiController]
    public class ProductimageAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public ProductimageAPIController(PetHeavenDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IEnumerable<ProductimageViewModel> GetProductimage()
        {

            var temp = _context.ProductImage.Include(x => x.Product)
                .Select(option => new ProductimageViewModel
                {
                    ProductID = option.ProductId,
                    ProductImageID = option.ProductImageId,
                    ProductName = option.Product.ProductName,
                    ProductImagePath = option.ProductImagePath,
                });
            return temp;
        }

        [HttpPut("{productImageID}")]
        public async Task<String> UploadImage(int productImageID, [FromForm] ProductimageViewModel ProductImageData)
        {
            try
            {
                ProductImage DTO = await _context.ProductImage.FindAsync(productImageID);
                if (Request.Form.Files["ProductImagePath"] != null)
                {
                    IFormFile file = Request.Form.Files["ProductImagePath"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "Productimage");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        DTO.ProductImagePath = "/Productimage/" + uniqueFileName;
                    }
                }
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductimageExists(ProductImageData.ProductID))
                {
                    return "商品編號不存在!!";
                }
                else
                {
                    throw;
                }
            }
            return "更改存檔完成!!";
        }

        private int GetProductId(string? productName)
        {
            var Product = _context.Product.FirstOrDefault(s => s.ProductName == productName);
            return Product.ProductId;
        }

        private bool ProductimageExists(int? id)
        {
            return (_context.ProductImage?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        [HttpDelete("{ProductImageID}")]
        public async Task<string> DeleteProductimage(int ProductImageID)
        {
            var Productimage = await _context.ProductImage.FindAsync(ProductImageID);
            if (ProductImageID == null)
            {
                return "無此圖片，不可刪除，請洽談工程師處理!!";
            }
            _context.ProductImage.Remove(Productimage);
            await _context.SaveChangesAsync();

            return "刪除成功!!";
        }

        [HttpPost]
        public async Task<String> CreateProduct([FromForm] ProductimageViewModel ProductImageData)
        {

            try
            {
                int ProductId = GetProductId(ProductImageData.ProductName);
                ProductImage data = new ProductImage
                {
                    ProductId = ProductId,
                };

                if (Request.Form.Files["ProductImagePath"] != null)
                {
                    IFormFile file = Request.Form.Files["ProductImagePath"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "ProductImagePath");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        data.ProductImagePath = "/Productimage/" + uniqueFileName;
                    }
                }
                else
                {
                    return "圖片不存在，請確認圖片!!";
                }
                _context.Update(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductimageExists(ProductImageData.ProductID))
                {
                    return "商品圖示新增失敗!!";
                }
                else
                {
                    throw;
                }
            }
            return "商品圖示新增完成!!";
        }

        public IEnumerable<CreateProductimage> GetSalonByCatagory()
        {
            var temp = _context.ProductCatagory
            .Select(option => new CreateProductimage
            {
                ProductCatagoryId=option.ProductCatagoryId,
                ProductCatagoryName = option.ProductCatagoryName,
            });

            return temp;
        }

        public IEnumerable<CreateProductimage> GetSalonByProductType()
        {
            var temp = _context.ProductType
            .Select(option => new CreateProductimage
            {
                ProductTypeId=option.ProductTypeId,
                ProductTypeName = option.ProductTypeName,
            });

            return temp;
        }

        public IEnumerable<CreateProductimage> GetSalonByProduct()
        {
            var temp = _context.Product
            .Select(option => new CreateProductimage
            {
                ProductCatagoryId=option.ProductCatagoryId,
                ProductTypeId=option.ProductTypeId,
                ProductName = option.ProductName,
            });

            return temp;
        }

        public IEnumerable<CreateProductimage> FilterProducts([FromBody]CreateProductimage Product)
        {
            //int ProductTypeId = GetProductTypeId(Product.ProductTypeName);
            //int ProductCatagoryId = GetProductCatagoryId(Product.ProductCatagoryName);
            var selectProducts = _context.Product
                .Where(p => p.ProductTypeId == Product.ProductTypeId && p.ProductCatagoryId == Product.ProductCatagoryId)
                .Select(p => new CreateProductimage { ProductName=p.ProductName });
            return selectProducts;
        }

        private int GetProductCatagoryId(string? productCatagoryName)
        {
            var ProductCatagoryName = _context.ProductCatagory.FirstOrDefault(s => s.ProductCatagoryName == productCatagoryName);
            return ProductCatagoryName.ProductCatagoryId;
        }

        private int GetProductTypeId(string? productTypeName)
        {
            var ProductTypeName = _context.ProductType.FirstOrDefault(s => s.ProductTypeName == productTypeName);
            return ProductTypeName.ProductTypeId;
        }
    }
}
