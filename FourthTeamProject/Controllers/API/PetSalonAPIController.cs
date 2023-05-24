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
        public async Task<IEnumerable<SalonViewModel>> GetPetSalon()
        {
            var temp = _context.Salon
                .Join(_context.SalonSolution,Salon => Salon.SalonId
                , SalonSolution=>SalonSolution.SalonSolutionId
                ,(Salon,SalonSolution) => new {Salon=Salon,SalonSolution=SalonSolution})
                .Select(joinresult => new SalonViewModel
            {
                SalonCatagoryName=joinresult.Salon.SalonCatagory.SalonCatagoryName,
                SalonSolutionName=joinresult.SalonSolution.SalonSolutionName,
                SalonSolutionDiscription=joinresult.SalonSolution.SalonSolutionDiscription,
                SalonSolutionPrice=joinresult.SalonSolution.SalonSolutionPrice,
                SalonName=joinresult.Salon.SalonName,
                SalonImagePath=joinresult.Salon.SalonImagePath,


            });
            return temp;
        }

        //[HttpPut]
        //public async Task<string> PutPetSalon(int id, [FromBody] SalonViewModel SalonDTO)
        //{
        //}

    }
}
