using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Controllers.API
{
    [Route("api/SalonClient/[action]")]
    [ApiController]
    public class SalonClientAPIController : ControllerBase
    {
        private readonly PetHeavenDbContext _context;
        public SalonClientAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SalonViewModel> GetSalonClient()
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
    }
}
