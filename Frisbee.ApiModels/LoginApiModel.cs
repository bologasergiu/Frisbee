using System.ComponentModel.DataAnnotations;

namespace Frisbee.ApiModels
{
    public class LoginApiModel
    {
        [EmailAddress]
        public string Email { get; set; } 
        public string Password { get; set; }
    }
}
