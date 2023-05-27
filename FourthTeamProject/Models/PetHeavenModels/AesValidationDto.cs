namespace FourthTeamProject.Models.PetHeavenModels
{
    public class AesValidationDto
    {
        public AesValidationDto(string CMemberAccount, DateTime ExpiredDate)
        {
            this.CMemberAccount = CMemberAccount;
            this.ExpiredDate = ExpiredDate;
        }
        public string CMemberAccount { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
