
using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class ChangePasswordApiModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
