using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
                    SalonCatagoryId= option.SalonCatagoryId,
                    SalonCatagoryName = option.SalonCatagory.SalonCatagoryName,
                    SalonName = option.SalonName,
                    SalonImagePath = option.SalonImagePath,
                    SalonId = option.SalonId,
                });
            return temp;
        }

        public IEnumerable<SalonViewModel> GetAddSalon()
        {
            var temp = _context.Salon.Include(x => x.SalonCatagory)
            .Select(option => new SalonViewModel
            {
             SalonId = option.SalonId,
             SalonCatagoryId=option.SalonCatagory.SalonCatagoryId,
             SalonCatagoryName = option.SalonCatagory.SalonCatagoryName,
             SalonName = option.SalonName,
             SalonImagePath = option.SalonImagePath,
            })
            .GroupBy(s => new { s.SalonCatagoryName })
            .Select(g => g.First())
            .ToList();
            return temp;
        }

        public IActionResult GetSalonByCatagoryName(string salonCatagoryName)
        {
            var filteredSalonSolutionNames = _context.SalonSolution
                .Where(s => s.SalonSolutionName.Contains(salonCatagoryName))
                .Select(s => s.SalonSolutionName)
                .ToList();

            return Ok(filteredSalonSolutionNames);
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


        [HttpPost]
        public async Task<string> PostAddSalon([FromForm] PostAddSalonImageViewModel SalonDTO)
        {
            int SalonSolutionId= GetSalonSolutionId(SalonDTO.SalonSolutionName);
            int SalonCatagoryId = GetSalonCatagoryId(SalonDTO.SalonCatagoryName);
            int SalonId = GetSalonId(SalonDTO.SalonName, SalonDTO.SalonCatagoryName);

            Salon CreateSalon = new Salon
            {
                SalonName = SalonDTO.SalonName,
                SalonCatagoryId = SalonDTO.SalonCatagoryId,

            };
            if (Request.Form.Files["SalonImagePath"] != null)
            {
                byte[] image = null;
                using (BinaryReader br = new BinaryReader(Request.Form.Files["SalonImagePath"].OpenReadStream()))
                {
                    image = br.ReadBytes((int)Request.Form.Files["SalonImagePath"].Length);
                }
                CreateSalon.SalonImagePath = image;
            }
            _context.Salon.Add(CreateSalon);
            _context.SaveChangesAsync();
            return "新增完成";


            //SalonSolutionSalon CreateSalonSolutionSalon = new SalonSolutionSalon
            //{
            //    SalonSolutionId = SalonDTO.SalonSolutionId,
            //    SalonId = SalonDTO.SalonId,
            //};
            //_context.SalonSolutionSalon.Add(CreateSalonSolutionSalon);
            //_context.SaveChangesAsync();
            //return "新增完成";


            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        Salon data = new Salon
            //        {
            //            SalonId = SalonDTO.SalonId,
            //            SalonName = SalonImageData.SalonName,
            //        };

            //        if (Request.Form.Files["SalonImagePath"] != null)
            //        {
            //            byte[] image = null;
            //            using (BinaryReader br = new BinaryReader(Request.Form.Files["SalonImagePath"].OpenReadStream()))
            //            {
            //                image = br.ReadBytes((int)Request.Form.Files["SalonImagePath"].Length);
            //            }
            //            data.SalonImagePath = image;
            //        }
            //        _context.Update(data);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!SalonExists(SalonImageData.SalonId))
            //        {
            //            return "美容項目編號不存在";
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return "更改存檔完成!!";
            //}
            return "圖片不正確!!";
        }

        private int GetSalonCatagoryId(string salonCatagoryName)
        {
            var SalonCatagory = _context.SalonCatagory.FirstOrDefault(s => s.SalonCatagoryName == salonCatagoryName);
            return SalonCatagory.SalonCatagoryId;
        }

        private int GetSalonId(string? salonName, string? salonCatagoryName)
        {
            var Salon = _context.Salon.FirstOrDefault(s => s.SalonName == salonName && s.SalonCatagory.SalonCatagoryName == salonCatagoryName);
            return Salon.SalonId;
        }
        

        private int GetSalonSolutionId(string salonSolutionName)
        {
            var SalonSolution = _context.SalonSolution.FirstOrDefault(s => s.SalonSolutionName == salonSolutionName);
            return SalonSolution.SalonSolutionId;
        }


        [HttpPost]
        public async Task<string> CreateSalonSolution(SalonViewModel SalonSolutionDTO)
        {
            SalonSolution DTO = new SalonSolution
            {
                SalonSolutionId = SalonSolutionDTO.SalonSolutionId,
                SalonSolutionName = SalonSolutionDTO.SalonSolutionName,
                SalonSolutionDiscription = SalonSolutionDTO.SalonSolutionDiscription,
                SalonSolutionPrice = SalonSolutionDTO.SalonSolutionPrice,
            };
            _context.SalonSolution.Add(DTO);
            await _context.SaveChangesAsync();

            return "美容服務新增成功";
        }

        [HttpDelete("{salonSolutionId}")]
        public async Task<string> DeleteSalonSolution(int salonSolutionId)
        {
            var salonSolution = await _context.SalonSolution.FindAsync(salonSolutionId);
            if (salonSolution == null)
            {
                return "無此美容服務，不可刪除，請洽談工程師處理!!";
            }

            _context.SalonSolution.Remove(salonSolution);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆美容服務已存有美容項目，不可刪除!!";
            }

            return "美容服務刪除成功!!";
        }

        [HttpPost]
        public async Task<String> CreateSalon([FromForm] SalonImageViewModel SalonData)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Salon data = new Salon
                    {
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
                    _context.Update(data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonExists(SalonData.SalonId))
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

    }
}
