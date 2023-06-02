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

        public IEnumerable<SalonViewModel> GetSalonSolution()
        {
            var temp = _context.SalonSolution.Select(option => new SalonViewModel
                {
                    SalonSolutionId=option.SalonSolutionId,
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
            return  temp;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UploadImage(int id, IFormFile SalonImagePath)
        {
            byte[] image;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await SalonImagePath.CopyToAsync(memoryStream);
                image = memoryStream.ToArray();
            }

            Salon salon = await _context.Salon.FindAsync(id);

            salon.SalonImagePath = image;
            await _context.SaveChangesAsync();
            return Ok("可");
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
