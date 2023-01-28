using Frisbee.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public UserController() { }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterApiModel registerApiModel)
        {
            return true;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<bool> Login(LoginApiModel loginApiModel)
        {
            return true;    
        }
    };
}
