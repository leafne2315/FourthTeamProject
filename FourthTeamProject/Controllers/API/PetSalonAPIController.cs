using FourthTeamProject.Models;
using FourthTeamProject.Models.ViewModel;
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
        public async Task<IEnumerable<SalonViewModel>> GetPetSalon()
        {
            var temp =  _context.TSalonType.Select(option => new SalonViewModel
            {
                CSalonId = option.CSalonId,
                CSalonName = option.CSalonName,
                CSalonContet = option.CSalonContet
            });
            return temp;
        }

        //[HttpPut]
        //public async Task<string> PutPetSalon(int id, [FromBody] SalonViewModel SalonDTO)
        //{
        //}
            
    }
}
