using Microsoft.AspNetCore.Identity;

namespace FrisbeeApp.DatabaseModels.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Team { get; set; }
        public bool Certified { get; set; }
        public bool AccountApproved { get; set; }

    }
}
