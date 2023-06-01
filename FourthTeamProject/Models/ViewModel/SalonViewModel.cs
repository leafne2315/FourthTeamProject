using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FourthTeamProject.Models.ViewModel
{
    public class SalonViewModel
    {
        public int SalonSolutionSalonId { get; set; }
        public int SalonId { get; set; }
        public int SalonSolutionId { get; set; }
        public string? SalonCatagoryName { get; set; }
        public string? SalonSolutionName { get; set; }
        public string? SalonSolutionDiscription { get; set; }
        public int SalonSolutionPrice { get; set; }
        public string? SalonName { get; set; }

        public byte[]? SalonImagePath { get; set; }



        public string Src { get; set; }
    }
}
