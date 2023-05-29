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

        public async Task<IEnumerable<SalonViewModel>> GetSalonSolution()
        {
            var temp = _context.SalonSolution.Select(option => new SalonViewModel
                {
                    SalonSolutionId=option.SalonSolutionId,
                    SalonSolutionName = option.SalonSolutionName,
                    SalonSolutionDiscription = option.SalonSolutionDiscription,
                    SalonSolutionPrice = option.SalonSolutionPrice,
                });
            return await temp.ToArrayAsync();
        }

        [HttpPost("{id}")]
        public async Task<IEnumerable<SalonViewModel>> GetSalon(int id, [FromBody] SalonViewModel SalonSolutionId)
        {
            var temp = _context.SalonSolution
                .Join(_context.SalonSolutionSalon, a => a.SalonSolutionId, ab => ab.SalonSolutionId, (a, ab)=> new { SalonSolution = a, SalonSolutionSalon = ab })
                .Join(_context.Salon, ab => ab.SalonSolutionSalon.SalonId,b => b.SalonId,(ab, b) => new { SalonSolution = ab.SalonSolution, Salon = b })         
                .Select(option => new SalonViewModel
                {
                    SalonId=option.Salon.SalonId,
                    SalonCatagoryName = option.Salon.SalonCatagory.SalonCatagoryName,
                    SalonName = option.Salon.SalonName,
                    SalonImagePath = option.Salon.SalonImagePath,
                });
            return await temp.ToArrayAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UploadImage([FromBody] SalonViewModel data)
        {           
            //string salonId = data.SalonId.ToString();
            //string salonCatagoryName = data.SalonCatagoryName;
            //string salonName = data.SalonName;
            //string fileName = $"{salonCatagoryName}_{salonName}";


            //string filePath = Path.Combine("wwwroot", "Salonimage", fileName);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await data.SalonImagePath.CopyToAsync(stream);
            //}




            return Ok();

        }




        [HttpPut("{id}")]
        public async Task<string> PutSalonSolution(int id, [FromBody] SalonViewModel SalonDTO)
        {
            if (id != SalonDTO.SalonSolutionId)
            {
                return "商品編號錯誤";
            }
            SalonSolution DTO = await _context.SalonSolution.FindAsync(id);
            DTO.SalonSolutionId = SalonDTO.SalonSolutionId;
            DTO.SalonSolutionName = SalonDTO.SalonSolutionName;
            DTO.SalonSolutionDiscription = SalonDTO.SalonSolutionDiscription;
            DTO.SalonSolutionPrice = SalonDTO.SalonSolutionPrice;
            _context.Entry(DTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonSolutionExists(id))
                {
                    return "無對應資料，無法修改";
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

        //[HttpPost("Filter")]
        //public async Task<IEnumerable<SalonViewModel>> FilterSalonSolutionDTO([FromBody] SalonViewModel SalonDTO)
        //{
        //    return _context.SalonSolution.Where(
        //        emp => emp.SalonSolutionId == SalonDTO.SalonSolutionId ||
        //               emp.SalonSolutionName.Contains(SalonDTO.SalonSolutionName) ||
        //               emp.SalonSolutionDiscription.Contains(SalonDTO.SalonSolutionDiscription) ||
        //               emp.SalonSolutionPrice == SalonDTO.SalonSolutionPrice)
        //        .Select(emp => new SalonViewModel
        //        {
        //            SalonSolutionId = emp.SalonSolutionId,
        //            SalonSolutionName = emp.SalonSolutionName,
        //            SalonSolutionDiscription = emp.SalonSolutionDiscription,
        //            SalonSolutionPrice = emp.SalonSolutionPrice
        //        });
        //}

        //[HttpPost]
        //public async Task<string> PostPetSalon(SalonViewModel SalonDTO)
        //{

        //    SalonSolution emp = new SalonSolution
        //    {
        //        SalonSolutionId = SalonDTO.SalonSolutionId,
        //        SalonSolutionName = SalonDTO.SalonSolutionName,
        //        SalonSolutionDiscription = SalonDTO.SalonSolutionDiscription,
        //        SalonSolutionPrice = SalonDTO.SalonSolutionPrice,
        //    };
        //    _context.SalonSolution.Add(emp);
        //    await _context.SaveChangesAsync();

        //    return $"員工編號:{emp.SalonSolutionId}";
        //}

        [HttpPost]
        public async Task<string> CteateSalonSolution (SalonViewModel SalonSolutionDTO)
        {
            SalonSolution DTO = new SalonSolution
            {
                SalonSolutionId = SalonSolutionDTO.SalonSolutionId,
                SalonSolutionName= SalonSolutionDTO.SalonSolutionName,
                SalonSolutionDiscription= SalonSolutionDTO.SalonSolutionDiscription,
                SalonSolutionPrice=SalonSolutionDTO.SalonSolutionPrice
            };
            _context.SalonSolution.Add(DTO);
            await _context.SaveChangesAsync();
            return $"服務類型:{DTO.SalonSolutionName}";
        }

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
