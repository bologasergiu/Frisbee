using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _repository;

        public UserController(IMapper mapper, IAuthRepository repository) 
        {
            _mapper = mapper;
            _repository = repository;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterApiModel registerApiModel)
        {
            var registerUser = _mapper.Map<User>(registerApiModel);

            return await _repository.Register(registerUser, registerApiModel.Password, registerApiModel.Role);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<TokenDTO> Login(LoginApiModel loginApiModel)
        {
            var loginUser = _mapper.Map<LoginDTO>(loginApiModel);

            return await _repository.Login(loginUser);
        }

        [Authorize]
        [Route("logout")]
        [HttpPost]
        public async Task Logout()
        {
            await _repository.Logout();
        }

        [Authorize(Roles ="Coach, Admin")]
        [Route("approve-account")]
        [HttpPut]
        public async Task<bool> ApproveAccount(Guid id)
        {
            return await _repository.ApproveAccount(id, User.Identity.Name);
        }
    };
}
