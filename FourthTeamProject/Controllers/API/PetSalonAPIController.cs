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
        
    //    public async Task<IEnumerable<SalonViewModel>> GetDogSalon()
    //    {
    //        var temp = _context.Salon.Include(x => x.SalonCatagory).Where(x => x.SalonCatagoryName == "狗狗")
    //            .Select(joinresult => new SalonViewModel
    //            {
				//	SalonCatagoryName = joinresult.SalonCatagoryName,
				//	SalonName = joinresult.Salon.SalonName,
				//	SalonImagePath = joinresult.Salon.SalonImagePath,
				//	SalonId = joinresult.Salon.SalonId,
				//});
    //    }


        public async Task<IEnumerable<SalonViewModel>> GetPetSalon()
        {
            var temp = _context.Salon.Include(x => x.SalonCatagory)
                //.Join(_context.SalonSolution, Salon => Salon.SalonId, SalonSolution => SalonSolution.SalonSolutionId, (Salon, SalonSolution)
                //=> new { Salon = Salon, SalonSolution = SalonSolution })
                .Select(joinresult => new SalonViewModel
                {
                    SalonCatagoryName = joinresult.SalonCatagory.SalonCatagoryName,
                    //SalonSolutionId = joinresult.SalonSolution.SalonSolutionId,
                    //SalonSolutionName = joinresult.SalonSolution.SalonSolutionName,
                    //SalonSolutionDiscription = joinresult.SalonSolution.SalonSolutionDiscription,
                    //SalonSolutionPrice = joinresult.SalonSolution.SalonSolutionPrice,
                    SalonName = joinresult.SalonName,
                    SalonImagePath = joinresult.SalonImagePath,
                    SalonId = joinresult.SalonId,
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




        //[HttpPut("{id}")]
        //public async Task<string> PutPetSalon(int id, [FromBody] SalonViewModel SalonDTO)
        //{
        //    if (id != SalonDTO.SalonSolutionId)
        //    {
        //        return "無此商品編號";
        //    }
        //    SalonSolution emp = await _context.SalonSolution.FindAsync(id);
        //    emp.SalonSolutionId = SalonDTO.SalonSolutionId;
        //    emp.SalonSolutionName = SalonDTO.SalonSolutionName;
        //    emp.SalonSolutionDiscription = SalonDTO.SalonSolutionDiscription;
        //    emp.SalonSolutionPrice = SalonDTO.SalonSolutionPrice;
        //    _context.Entry(emp).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SalonSolutionExists(id))
        //        {
        //            return "修改失敗";
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return "修改成功";
        //}

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
    }
}
