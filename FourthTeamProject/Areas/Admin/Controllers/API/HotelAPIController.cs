using FourthTeamProject.Areas.Admin.ViewModels;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Areas.Admin.Controllers.API
{
    [Route("api/HotelEnterprise/[action]")]
    [ApiController]
    public class HotelAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HotelAPIController(PetHeavenDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<HotelEnterpriseViewModel> GetHotel()
        {

            var temp = _context.Hotel.Include(x => x.HotelCatagory)
                .Select(option => new HotelEnterpriseViewModel
                {
                    HotelId = option.HotelId,
                    HotelCatagoryName = option.HotelCatagory.HotelCatagoryName,
                    HotelName = option.HotelName,
                    UnitPrice = option.UnitPrice,
                    HotelContent = option.HotelContent,
                    HotelImage = option.HotelImage,
                    HotelContentDetail = option.HotelContentDetail,

                });
            return temp;
        }

        [HttpPut("{hotelId}")]

        //<-----------------------圖片更新----------------------------->
        [HttpPost("{hotelId}")]
        public async Task<string> UploadImage(int hotelId, [FromForm] HotelEnterpriseViewModel HotelData)
        {
            try
            {
                int HotelCatagoryId = GetHotelCatagoryId(HotelData.HotelCatagoryName);
                Hotel DTO = await _context.Hotel.FindAsync(hotelId);
                DTO.HotelCatagoryId = HotelCatagoryId;
                DTO.HotelName = HotelData.HotelName;
                DTO.UnitPrice = HotelData.UnitPrice;
                DTO.HotelContent = HotelData.HotelContent;
                DTO.HotelContentDetail = HotelData.HotelContentDetail;

                if (Request.Form.Files["HotelImage"] != null)
                {
                    IFormFile file = Request.Form.Files["HotelImage"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "HotelImage");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        DTO.HotelImage = "/HotelImage/" + uniqueFileName;
                    }
                }
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(hotelId))
                {
                    return "房型編號不存在!!";
                }
                else
                {
                    throw;
                }
            }
            return "房型資訊更改存檔完成!!";
        }

        //<-----------------------非圖片更新----------------------------->
        [HttpPut("{hotelId}")]
        public async Task<string> Uploadtext(int hotelId, [FromBody] HotelEnterpriseViewModel Hoteltext)
        {
            try
            {
                int HotelCatagoryId = GetHotelCatagoryId(Hoteltext.HotelCatagoryName);
                Hotel DTO = await _context.Hotel.FindAsync(hotelId);
                DTO.HotelId = Hoteltext.HotelId;
                DTO.HotelName = Hoteltext.HotelName;
                DTO.UnitPrice = Hoteltext.UnitPrice;
                DTO.HotelContent = Hoteltext.HotelContent;
                DTO.HotelContentDetail = Hoteltext.HotelContentDetail;
                DTO.HotelCatagoryId = HotelCatagoryId;
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(hotelId))
                {
                    return "房型編號不存在";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        private bool HotelExists(int id)
        {
            return (_context.Hotel?.Any(e => e.HotelId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IEnumerable<HotelEnterpriseViewModel> InsertHotel()
        {
            var temp = _context.HotelCatagory
            .Select(option => new HotelEnterpriseViewModel
            {
                HotelCatagoryId = option.HotelCatagoryId,
                HotelCatagoryName = option.HotelCatagoryName,
            });
            return temp;
        }

        [HttpPost]
        public async Task<string> CreateHotel([FromForm] HotelEnterpriseViewModel HotelData)
        {

            try
            {
                int HotelCatagoryId = GetHotelCatagoryId(HotelData.HotelCatagoryName);
                Hotel data = new Hotel
                {
                    HotelCatagoryId = HotelCatagoryId,
                    HotelName = HotelData.HotelName,
                    UnitPrice = HotelData.UnitPrice,
                    HotelContent = HotelData.HotelContent,
                    HotelContentDetail = HotelData.HotelContentDetail,
                };

                if (Request.Form.Files["HotelImage"] != null)
                {
                    IFormFile file = Request.Form.Files["HotelImage"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "HotelImage");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        data.HotelImage = "/HotelImage/" + uniqueFileName;
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
                if (!HotelExists(HotelData.HotelId))
                {
                    return "房型新增失敗!!";
                }
                else
                {
                    throw;
                }
            }
            return "房型新增完成!!";
        }

        private int GetHotelCatagoryId(string? hotelCatagoryName)
        {
            var HotelCatagory = _context.HotelCatagory.FirstOrDefault(s => s.HotelCatagoryName == hotelCatagoryName);
            return HotelCatagory.HotelCatagoryId;
        }

        [HttpDelete("{hotelId}")]
        public async Task<string> DeleteHotel(int hotelId)
        {
            var hotel = await _context.Hotel.FindAsync(hotelId);
            if (hotel == null)
            {
                return "無此房型，不可刪除，請洽談工程師處理!!";
            }

            _context.Hotel.Remove(hotel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆房型已存在於其他房型服務中，不可刪除!!";
            }

            return "房型刪除成功!!";
        }

    }
}
