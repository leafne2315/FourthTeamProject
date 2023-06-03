namespace FourthTeamProject.Models.ViewModel
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Account { get; internal set; }
        public string Email { get; internal set; }
        public string Name { get; internal set; }
        public string Address { get; internal set; }
		public string Password { get; internal set; }
	}
}
