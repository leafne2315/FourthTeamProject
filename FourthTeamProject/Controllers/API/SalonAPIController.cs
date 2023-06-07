using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/SalonEnterprise/[action]")]
    [ApiController]
    public class SalonAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public SalonAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SalonViewModel> GetSalon()
        {

            var temp = _context.Salon.Include(x => x.SalonCatagory)
                .Select(option => new SalonViewModel
                {
                    SalonCatagoryId = option.SalonCatagoryId,
                    SalonCatagoryName = option.SalonCatagory.SalonCatagoryName,
                    SalonName = option.SalonName,
                    SalonImagePath = option.SalonImagePath,
                    SalonId = option.SalonId,
                });
            return temp;
        }

        //<-----------------------美容項目圖片更新----------------------------->
        [HttpPut("{id}")]
        public async Task<String> UploadImage(int id, [FromForm] SalonImageViewModel SalonImageData)
        {
            try
            {
                int SalonCatagoryId = GetSalonCatagoryId(SalonImageData.SalonCatagoryName);
                Salon DTO = await _context.Salon.FindAsync(id);
                DTO.SalonCatagoryId = SalonCatagoryId;
                DTO.SalonId = SalonImageData.SalonId;
                DTO.SalonName = SalonImageData.SalonName;
                if (Request.Form.Files["SalonImagePath"] != null)
                {
                    byte[] image = null;
                    using (BinaryReader br = new BinaryReader(Request.Form.Files["SalonImagePath"].OpenReadStream()))
                    {
                        image = br.ReadBytes((int)Request.Form.Files["SalonImagePath"].Length);
                    }
                    DTO.SalonImagePath = image;
                }
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(id))
                {
                    return "美容項目編號不存在";
                }
                else
                {
                    throw;
                }
            }
            return "更改存檔完成!!";
        }

        //<-----------------------美容項目非圖片更新----------------------------->
        [HttpPut("{id}")]
        public async Task<string> UploadSalontext(int id, [FromBody] SalonViewModel Salontext)
        {
            try
            {
                int SalonCatagoryId = GetSalonCatagoryId(Salontext.SalonCatagoryName);
                Salon DTO = await _context.Salon.FindAsync(id);
                DTO.SalonId = Salontext.SalonId;
                DTO.SalonName = Salontext.SalonName;
                DTO.SalonCatagoryId = SalonCatagoryId;
                _context.Update(DTO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(id))
                {
                    return "美容項目編號不存在";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        private bool SalonExists(int id)
        {
            return (_context.Salon?.Any(e => e.SalonId == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IEnumerable<SalonViewModel> InsertPageSalon()
        {
            var temp = _context.SalonCatagory
            .Select(option => new SalonViewModel
            {
                SalonCatagoryId = option.SalonCatagoryId,
                SalonCatagoryName = option.SalonCatagoryName,
            });
            return temp;
        }

        [HttpPost]
        public async Task<String> CreateSalon([FromForm] SalonImageViewModel SalonData)
        {

            try
            {
                bool salonCatagoryExists = _context.SalonCatagory.Any(sc => sc.SalonCatagoryName == SalonData.SalonCatagoryName);

                if (salonCatagoryExists)
                {
                    bool salonExists = _context.Salon.Any(s => s.SalonName == SalonData.SalonName && s.SalonCatagory.SalonCatagoryName == SalonData.SalonCatagoryName);

                    if (salonExists)
                    {
                        return "已經存在相同寵物的項目，不可新增!!";
                    }
                }

                int SalonCatagoryId = GetSalonCatagoryId(SalonData.SalonCatagoryName);
                Salon data = new Salon
                {
                    SalonCatagoryId = SalonCatagoryId,
                    SalonId = SalonData.SalonId,
                    SalonName = SalonData.SalonName,
                };

                if (Request.Form.Files["SalonImagePath"] != null)
                {
                    byte[] image = null;
                    using (BinaryReader br = new BinaryReader(Request.Form.Files["SalonImagePath"].OpenReadStream()))
                    {
                        image = br.ReadBytes((int)Request.Form.Files["SalonImagePath"].Length);
                    }
                    data.SalonImagePath = image;
                }
                else
                {
                    return "圖片不存在，請確認圖片!!";
                }
                _context.Salon.Add(data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(SalonData.SalonId))
                {
                    return "美容項目不存在!!";
                }
                else
                {
                    throw;
                }
            }
            return "美容項目新增完成!!";
        }


        private int GetSalonCatagoryId(string salonCatagoryName)
        {
            var SalonCatagory = _context.SalonCatagory.FirstOrDefault(s => s.SalonCatagoryName == salonCatagoryName);
            return SalonCatagory.SalonCatagoryId;
        }

        [HttpDelete("{salonId}")]
        public async Task<string> DeleteSalon(int salonId)
        {
            try
            {
                var salon = await _context.Salon.FindAsync(salonId);
                if (salon == null)
                {
                    return "無此美容項目，不可刪除，請洽談工程師處理!!";
                }
                _context.Salon.Remove(salon);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆美容項目已存在於其他美容服務中，不可刪除!!";
            }

            return "美容服務刪除成功!!";
        }


    }
}
