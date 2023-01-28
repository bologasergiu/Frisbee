﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(ConfirmPassword.CompareTo(Password) != 0)
            {
                yield return new ValidationResult(errorMessage: "Passwords do not match.", memberNames: new[] {"Password"});
            }
        }
    }

}