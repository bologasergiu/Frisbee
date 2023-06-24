using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web;
using static FrisbeeApp.Logic.Repositories.AuthRepository;

namespace FrisbeeApp.Controllers.Controllers
{
    [Route("Email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailServiceRepository;
        private readonly IAuthRepository _authRepository;
        public EmailController(IEmailService emailServiceRepository, IAuthRepository authRepository)
        {

            _emailServiceRepository = emailServiceRepository;
            _authRepository = authRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmEmail/{userEmail}/{userToken}")]
        public async Task<IActionResult> ConfirmEmail(string userEmail, string userToken)
        {
            var email = HttpUtility.UrlDecode(userEmail);
            var token = HttpUtility.UrlDecode(userToken);
            var confirmed  =  await _authRepository.ConfirmAccount(email, token);
            if(confirmed) 
            {             
                return Redirect($"http://localhost:4200/login");
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmNewPassword/{userEmail}/{userToken}")]
        public IActionResult ConfirmNewPassword(string userEmail, string userToken)
        {
            var email = HttpUtility.UrlDecode(userEmail);
            var token = HttpUtility.UrlDecode(userToken);
            var redirectUrl = $"http://localhost:4200/change-password?email={email}&token={token}";
            return Redirect(redirectUrl);
        }
    }
}
