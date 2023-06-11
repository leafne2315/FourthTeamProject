using FourthTeamProject.Areas.Admin.ViewModels;
using FourthTeamProject.Models.ViewModel;
using FourthTeamProject.PetHeavenModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourthTeamProject.Areas.Admin.Controllers.API
{
    [Route("api/SalonSolutionSalonEnterprise/[action]")]
    [ApiController]
    public class SalonSolutionSalonAPIController : ControllerBase
    {

        private readonly PetHeavenDbContext _context;
        public SalonSolutionSalonAPIController(PetHeavenDbContext context)
        {
            _context = context;
        }

        public IActionResult GetSalonSolutionSalon()
        {
            var temp = _context.SalonSolutionSalon.Include(x => x.Salon)
                .Include(x => x.SalonSolution)
                .Select(option => new SalonSolutionSalonViewModel
                {
                    SalonId = option.SalonId,
                    SalonSolutionId = option.SalonSolutionId,
                    SalonName = option.Salon.SalonName,
                    SalonSolutionName = option.SalonSolution.SalonSolutionName,

                });



            //var salonData = _context.SalonSolutionSalon
            //    .GroupBy(ss => ss.SalonSolutionId)
            //    .Select(group => new 
            //    {
            //        SalonSolutionId = group.Key,
            //        SalonSolutionName = group.FirstOrDefault().SalonSolution.SalonSolutionName,
            //        SalonIds = group.Select(ss => new
            //        {
            //            SalonId = ss.SalonId,
            //            SalonName = ss.Salon.SalonName
            //        })
            //    })
            //    .ToList();

            return Ok(temp);
        }
    }
}
