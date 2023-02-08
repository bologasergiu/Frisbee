namespace FrisbeeApp.Logic.DtoModels
{
    public class TeamMemberDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Certified { get; set; }
        public string Role { get; set; }
    }
}
