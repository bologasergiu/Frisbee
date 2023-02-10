using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class RegisterApiModel : IValidatableObject
    {
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "FirstName can only contain letters.")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "LastName can only contain letters.")]
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Team { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#=+';:_,.?!@$%^&*-]).{10,}$",
            ErrorMessage = "Password should contain at least 10 characters, at least one uppercase letter," +
            "one lowercase letter, one secial character and at leat one digit.")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public  ChosenRole Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(ConfirmPassword.CompareTo(Password) != 0)
            {
                yield return new ValidationResult(errorMessage: "Passwords do not match.", memberNames: new[] {"Password"});
            }
            if (Role != null)
            {
                if (!Enum.IsDefined(typeof(ChosenRole), Role) || Role == ChosenRole.Admin)
                {
                    yield return new ValidationResult(errorMessage: "Invalid Role.", memberNames: new[] { "Role" });
                }
            }
            if (BirthDate > DateTime.Now)
            {
                yield return new ValidationResult(errorMessage: "Wrong Birthdate", memberNames: new[] { "BirthDate" });
            }
        }
    }

}