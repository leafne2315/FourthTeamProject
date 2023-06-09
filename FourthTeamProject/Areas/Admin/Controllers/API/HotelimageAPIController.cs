using FourthTeamProject.Areas.Admin.ViewModels;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Areas.Admin.Controllers.API
{
    [Route("api/HotelimageEnterprise/[action]")]
    [ApiController]
    public class HotelimageAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public HotelimageAPIController(PetHeavenDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IEnumerable<HotelimageViewModel> GetHoteltimage()
        {

            var temp = _context.HotelImage.Include(x => x.Hotel)
                .Select(option => new HotelimageViewModel
                {
                    HotelID = option.HotelId,
                    HotelImageID = option.HotelImageId,
                    HotelName = option.Hotel.HotelName,
                    HotelImagePath = option.HotelImagePath,
                });
            return temp;
        }

        [HttpPut("{hotelImageID}")]
        public async Task<string> UploadImage(int hotelImageID, [FromForm] HotelimageViewModel HotelImageData)
        {
            try
            {
                HotelImage DTO = await _context.HotelImage.FindAsync(hotelImageID);
                if (Request.Form.Files["HotelImagePath"] != null)
                {
                    IFormFile file = Request.Form.Files["HotelImagePath"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "HotelRoomimage");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        DTO.HotelImagePath = "/HotelRoomimage/" + uniqueFileName;
                    }
                }
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelimageExists(HotelImageData.HotelID))
                {
                    return "房型編號不存在!!";
                }
                else
                {
                    throw;
                }
            }
            return "更改存檔完成!!";
        }

        private bool HotelimageExists(int hotelID)
        {
            return (_context.HotelImage?.Any(e => e.HotelId == hotelID)).GetValueOrDefault();
        }

        [HttpDelete("{hotelImageID}")]
        public async Task<string> DeleteProductimage(int hotelImageID)
        {
            var Hotelimage = await _context.HotelImage.FindAsync(hotelImageID);
            if (hotelImageID == null)
            {
                return "無此圖片，不可刪除，請洽談工程師處理!!";
            }
            _context.HotelImage.Remove(Hotelimage);
            await _context.SaveChangesAsync();

            return "刪除成功!!";
        }

        public IEnumerable<HotelimageViewModel> HotelName()
        {

            var temp = _context.Hotel
                .Select(option => new HotelimageViewModel
                {
                    HotelName = option.HotelName,
                });
            return temp;
        }


        [HttpPost]
        public async Task<string> CreateHotel([FromForm] HotelimageViewModel HotelImageData)
        {

            try
            {
                int HotelId = GetHotelId(HotelImageData.HotelName);
                HotelImage data = new HotelImage
                {
                    HotelId = HotelId,
                };

                if (Request.Form.Files["HotelImagePath"] != null)
                {
                    IFormFile file = Request.Form.Files["HotelImagePath"];
                    if (file.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "HotelRoomimage");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        data.HotelImagePath = "/HotelRoomimage/" + uniqueFileName;
                    }
                }
                else
                {
                    return "圖片不存在，請確認圖片!!";
                }
                _context.HotelImage.Add(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelimageExists(HotelImageData.HotelID))
                {
                    return "房型圖示新增失敗!!";
                }
                else
                {
                    throw;
                }
            }
            return "房型圖示新增完成!!";
        }

        private int GetHotelId(string HotelName)
        {
            var Hotel = _context.Hotel.FirstOrDefault(s => s.HotelName == HotelName);
            return Hotel.HotelId;
        }
    }
}
