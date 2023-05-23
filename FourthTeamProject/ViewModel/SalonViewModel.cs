using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FourthTeamProject.ViewModel
{
    public class SalonViewModel
    {
        [Display(Name = "寵物美容編號")]
        public int CSalonId { get; set; }
        [Display(Name = "寵物美容項目")]
        public string CSalonName { get; set; }
        [Display(Name = "寵物美容內容")]
        public string CSalonContet { get; set; }
    }
}
