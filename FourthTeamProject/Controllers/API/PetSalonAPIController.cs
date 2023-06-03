using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/PetSalon/[action]")]
    [ApiController]
    public class PetSalonAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public PetSalonAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }


        //<---------------頁面顯示------------------------>
        public IEnumerable<SalonViewModel> GetSalonSolution()
        {
            var temp = _context.SalonSolution.Select(option => new SalonViewModel
            {
                SalonSolutionId = option.SalonSolutionId,
                SalonSolutionName = option.SalonSolutionName,
                SalonSolutionDiscription = option.SalonSolutionDiscription,
                SalonSolutionPrice = option.SalonSolutionPrice,
            });
            return temp;
        }

        public IEnumerable<SalonViewModel> GetSalon()
        {

            var temp = _context.Salon.Include(x => x.SalonCatagory)
                .Select(option => new SalonViewModel
                {
                    SalonCatagoryName = option.SalonCatagory.SalonCatagoryName,
                    SalonName = option.SalonName,
                    SalonImagePath = option.SalonImagePath,
                    SalonId = option.SalonId,
                });
            return temp;
        }

        public IEnumerable<SalonViewModel> GetAddSalon()
        {
            var temp = _context.SalonSolution
            .Join(_context.SalonSolutionSalon, a => a.SalonSolutionId, ab => ab.SalonSolutionId, (a, ab) => new { SalonSolution = a, SalonSolutionSalon = ab })
            .Join(_context.Salon, ab => ab.SalonSolutionSalon.SalonId, b => b.SalonId, (ab, b) => new { SalonSolution = ab.SalonSolution, Salon = b })
            .Select(option => new SalonViewModel
            {
             SalonSolutionId = option.SalonSolution.SalonSolutionId,
             SalonSolutionName = option.SalonSolution.SalonSolutionName,
             SalonId = option.Salon.SalonId,
             SalonCatagoryName = option.Salon.SalonCatagory.SalonCatagoryName,
             SalonName = option.Salon.SalonName,
             SalonImagePath = option.Salon.SalonImagePath,
            }).ToList();
             var distinctTemp = temp
             .GroupBy(s => new { s.SalonSolutionName }) // 以 SalonSolutionName 和 SalonCatagoryName 進行分組
             .Select(g => g.First()) // 選取每個分組中的第一筆資料
             .ToList();
            var dTemp = temp
.GroupBy(s => new { s.SalonCatagoryName }) // 以 SalonSolutionName 和 SalonCatagoryName 進行分組
.Select(g => g.First()) // 選取每個分組中的第一筆資料
.ToList();
            return dTemp;
        }


        //<-----------------------美容項目圖片更新----------------------------->
        [HttpPut("{id}")]
        public async Task<String> UploadImage(int id, [FromForm] SalonImageViewModel SalonImageData)
        {
            if (id != SalonImageData.SalonId)
            {
                return "美容項目編號錯誤";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Salon data = new Salon
                    {
                        SalonId = SalonImageData.SalonId,
                        SalonName = SalonImageData.SalonName,
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
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonExists(SalonImageData.SalonId))
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
            return "圖片不正確!!";
        }

        //<-----------------------美容項目非圖片更新----------------------------->
        [HttpPut("{id}")]
        public async Task<string> UploadSalontext(int id, [FromBody] SalonViewModel Salontext)
        {
            if (id != Salontext.SalonId)
            {
                return "美容項目編號錯誤";
            }
            Salon DTO = await _context.Salon.FindAsync(id);
            DTO.SalonId = Salontext.SalonId;
            DTO.SalonName = Salontext.SalonName;
            _context.Entry(DTO).State = EntityState.Modified;

            try
            {
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

        //<-----------------------美容服務更新----------------------------->
        [HttpPut("{id}")]
        public async Task<string> PutSalonSolution(int id, [FromBody] SalonViewModel SalonSolutionIdText)
        {
            if (id != SalonSolutionIdText.SalonSolutionId)
            {
                return "美容服務編號錯誤";
            }
            SalonSolution DTO = await _context.SalonSolution.FindAsync(id);
            DTO.SalonSolutionId = SalonSolutionIdText.SalonSolutionId;
            DTO.SalonSolutionName = SalonSolutionIdText.SalonSolutionName;
            DTO.SalonSolutionDiscription = SalonSolutionIdText.SalonSolutionDiscription;
            DTO.SalonSolutionPrice = SalonSolutionIdText.SalonSolutionPrice;
            _context.Entry(DTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonSolutionExists(id))
                {
                    return "美容服務編號不存在";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        private bool SalonSolutionExists(int id)
        {
            return (_context.SalonSolution?.Any(e => e.SalonSolutionId == id)).GetValueOrDefault();
        }


        //<-----------------------美容服務取消----------------------------->
        [HttpPost]
        public async Task<string> CteateSalonSolution(SalonViewModel SalonSolutionDTO)
        {
            SalonSolution DTO = new SalonSolution
            {
                SalonSolutionId = SalonSolutionDTO.SalonSolutionId,
                SalonSolutionName = SalonSolutionDTO.SalonSolutionName,
                SalonSolutionDiscription = SalonSolutionDTO.SalonSolutionDiscription,
                SalonSolutionPrice = SalonSolutionDTO.SalonSolutionPrice
            };
            _context.SalonSolution.Add(DTO);
            await _context.SaveChangesAsync();
            return $"服務類型:{DTO.SalonSolutionName}";
        }

        //<-----------------------美容項目取消----------------------------->
        [HttpPost]
        public async Task<string> CteateSalon(SalonViewModel SalonDTO)
        {
            Salon DTO = new Salon
            {
                SalonId = SalonDTO.SalonId,
                SalonName = SalonDTO.SalonName,
                SalonImagePath = SalonDTO.SalonImagePath,
            };
            _context.Salon.Add(DTO);
            await _context.SaveChangesAsync();
            return $"服務類型:{DTO.SalonName}";
        }
    }
}
