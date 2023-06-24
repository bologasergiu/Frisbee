
using FrisbeeApp.DatabaseModels.Models;
using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class ChangePasswordApiModel : IValidatableObject
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ConfirmPassword.CompareTo(Password) != 0)
            {
                yield return new ValidationResult(errorMessage: "Passwords do not match.", memberNames: new[] { "Password" });
            }
        }
    }
}
