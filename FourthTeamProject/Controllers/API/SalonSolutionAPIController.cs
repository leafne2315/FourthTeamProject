using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("/api/SalonSolutionEnterprise/[action]")]
    [ApiController]
    public class SalonSolutionAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public SalonSolutionAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }
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

        [HttpPut("{id}")]
        public async Task<string> PutSalonSolution(int id, [FromBody] SalonViewModel SalonSolution)
        {
            if (id != SalonSolution.SalonSolutionId)
            {
                return "美容服務編號錯誤";
            }
            SalonSolution DTO = await _context.SalonSolution.FindAsync(id);
            DTO.SalonSolutionId = SalonSolution.SalonSolutionId;
            DTO.SalonSolutionName = SalonSolution.SalonSolutionName;
            DTO.SalonSolutionDiscription = SalonSolution.SalonSolutionDiscription;
            DTO.SalonSolutionPrice = SalonSolution.SalonSolutionPrice;
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

    }
}
