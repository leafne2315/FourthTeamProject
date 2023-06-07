namespace FourthTeamProject.Models.ViewModel
{
	public class MemberCentreViewModel
	{		
		public int Id { get; set; }
		public string Account { get; internal set; }
		public string Email { get; internal set; }
		public string Name { get; internal set; }
		public string Address { get; internal set; }
		public string Password { get; internal set; }
		public DateTime? Birthday { get; internal set; }
		public string Phone { get; internal set; }
	}
}
