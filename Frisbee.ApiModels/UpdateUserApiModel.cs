using FrisbeeApp.DatabaseModels.Models;
using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class UpdateUserApiModel : IValidatableObject
    {
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Firstname can only contain letters.")]
        public string? FirstName { get; set; }
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Lastname can only contain letters.")]
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        [EmailAddress]
        public string? Email{ get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber{ get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BirthDate > DateTime.Now)
            {
                yield return new ValidationResult(errorMessage: "Wrong Birthdate", memberNames: new[] { "BirthDate" });
            }
            if (Gender != 0)
            {
                if (!Enum.IsDefined(typeof(Gender), Gender))
                {
                    yield return new ValidationResult(errorMessage: "Invalid Gender.", memberNames: new[] { "Gender" });
                }
            }
        }
    }
}
