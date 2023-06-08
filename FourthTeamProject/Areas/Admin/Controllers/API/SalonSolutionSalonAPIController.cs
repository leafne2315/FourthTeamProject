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

        //public IEnumerable<SalonSolutionDataViewModel> GetSalonSolutionSalon()
        //{

        //    var temp = _context.SalonSolution.Include(x => x.SalonSolutionSalon)
        //        .ThenInclude(x=>x.Salon)
        //        .ThenInclude(x=>x.SalonCatagory)
        //        .Select(option => new SalonSolutionDataViewModel
        //        {
        //            SalonSolutionName=option.SalonSolutionName,
        //            Salons = option.SalonSolutionSalon.Select(sss => new SalonDataViewModel
        //            {
        //                SalonName = sss.Salon.SalonName,
        //                SalonCatagoryName = sss.Salon.SalonCatagory.SalonCatagoryName
        //            }).ToList()
        //        });
        //    return temp;
        //}
    }
}
