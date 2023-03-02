using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Web;

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
        public async Task<bool> ConfirmEmail(string userEmail, string userToken)
        {
            var email = HttpUtility.UrlDecode(userEmail);
            var token = HttpUtility.UrlDecode(userToken);
            return await _authRepository.ConfirmAccount(email, token);
        }
    }
}
