using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/HotelServiceEnterprise/[action]")]
    [ApiController]
    public class HotelServiceAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public HotelServiceAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelServiceEnterpriseViewModel> GetHotelService()
        {

            var temp = _context.HotelService
                .Select(option => new HotelServiceEnterpriseViewModel
                {
                    HotelServiceName = option.HotelServiceName,
                    HotelServiceID = option.HotelServiceID,
                });
            return temp;
        }

        [HttpPut("{id}")]
        public async Task<string> PutHotelService(int id, [FromBody] HotelServiceEnterpriseViewModel hotelService)
        {
            if (id != hotelService.HotelServiceID)
            {
                return "房型服務編號錯誤";
            }
            HotelService DTO = await _context.HotelService.FindAsync(id);
            DTO.HotelServiceID = hotelService.HotelServiceID;
            DTO.HotelServiceName = hotelService.HotelServiceName;
            _context.Entry(DTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelServiceExists(id))
                {
                    return "房型服務編號不存在";
                }
                else
                {
                    throw;
                }
            }

            return "修改成功";
        }

        private bool HotelServiceExists(int id)
        {
            return (_context.HotelService?.Any(e => e.HotelServiceID == id)).GetValueOrDefault();

        }

        [HttpDelete("{HotelServiceId}")]
        public async Task<string> DeleteHotelService(int HotelServiceId)
        {
            var HotelService = await _context.HotelService.FindAsync(HotelServiceId);
            if (HotelService == null)
            {
                return "無此房型服務，不可刪除，請洽談工程師處理!!";
            }

            _context.HotelService.Remove(HotelService);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "此筆房型服務已存在房型中，不可刪除!!";
            }

            return "房型服務刪除成功!!";
        }

        [HttpPost]
        public async Task<string> CreateHotelService( [FromBody]HotelServiceEnterpriseViewModel HotelServiceDTO)
        {


            HotelService DTO = new HotelService
            {
                //HotelServiceID = HotelServiceDTO.HotelServiceID,
                HotelServiceName = HotelServiceDTO.HotelServiceName,
            };
            _context.HotelService.Add(DTO);
            await _context.SaveChangesAsync();

            return "房型服務新增成功";
        }

    }
}
