namespace FrisbeeApp.Logic.DtoModels
{
    public class TeamMemberDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Certified { get; set; }
        public bool AccountApproved { get; set; }
        public ChosenRole Role { get; set; }
        public Gender Gender { get; set; }
        
    }
}
