using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frisbee.ApiModels
{
    public class LoginApiModel : IValidatableObject
    {
        public LoginApiModel() { }
        [EmailAddress]
        public String Email { get; set; } 
        public String Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           yield return new ValidationResult(errorMessage: "Username or password are incorrect. Please try again !");
        }
    }
}
