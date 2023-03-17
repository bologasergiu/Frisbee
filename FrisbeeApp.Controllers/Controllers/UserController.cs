using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.EmailSender.Common;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace FrisbeeApp.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPlayerRepository _playerRepository;
        private readonly FrisbeeAppContext _context;


        public UserController(IMapper mapper, IAuthRepository repository, IUserRepository userRepository, IEmailService emailService, IPlayerRepository playerRepository, FrisbeeAppContext context)
        {
            _mapper = mapper;
            _authRepository = repository;
            _userRepository = userRepository;
            _emailService = emailService;
            _playerRepository = playerRepository;
            _context = context;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register([FromQuery] RegisterApiModel registerApiModel)
        {
            var registerUser = _mapper.Map<User>(registerApiModel);
            var registerResult = await _authRepository.Register(registerUser, registerApiModel.Password, registerApiModel.Role);
            var token = await _authRepository.GenerateRegistrationToken(registerUser.Email);
            var link = Url.Action("ConfirmEmail", "Email", new { userEmail = HttpUtility.UrlEncode(registerUser.Email), userToken = HttpUtility.UrlEncode(token) }, protocol: Request.Scheme);

            _emailService.SendEmail(EmailTemplateType.ConfirmAccountPlayer, new List<string> { registerUser.Email }, new EmailInfo
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Link = link
            });

            var role = await _authRepository.GetRole(registerUser.Email);
            string coachEmail = await _playerRepository.GetCoachEmail(registerUser.Email);
            var coachUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == coachEmail);
            if (role == ChosenRole.Player.ToString())
            {
                _emailService.SendEmail(EmailTemplateType.ApproveAccount, new List<string> { coachEmail }, new EmailInfo
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    Responsable = coachUser.FirstName,
                    UserType = ChosenRole.Player.ToString(),
                });

            } else
            {
                _emailService.SendEmail(EmailTemplateType.ApproveAccount, new List<string> { "bologasergiu22@gmail.com" }, new EmailInfo
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    Responsable = "Sergiu",
                    UserType = ChosenRole.Coach.ToString(),
                }); ;
            }

            return registerResult;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<TokenDTO> Login(LoginApiModel loginApiModel)
        {
            var loginUser = _mapper.Map<LoginDTO>(loginApiModel);

            return await _authRepository.Login(loginUser);
        }

        [Authorize]
        [Route("logout")]
        [HttpPost]
        public async Task Logout()
        {
            await _authRepository.Logout();
        }

        [Authorize(Roles = "Coach, Admin")]
        [Route("approve-account")]
        [HttpPut]
        public async Task<bool> ApproveAccount(Guid id)
        {
            return await _authRepository.ApproveAccount(id, User.Identity.Name);
        }

        [Authorize]
        [Route("view-team")]
        [HttpGet]
        public async Task<List<TeamMemberDTO>> ViewTeam(string teamName)
        {
            return await _userRepository.ViewTeam(teamName);
        }

        [Authorize]
        [Route("update-user")]
        [HttpPut]
        public async Task<bool> UpdateUser(Guid Id, UpdateUserApiModel updateUserApiModel)
        {
            var user = _mapper.Map<User>(updateUserApiModel);
            return await _userRepository.UpdateUser(Id, user);
        }

        [Authorize(Roles = "Admin, Coach")]
        [Route("update-team")]
        [HttpPut]
        public async Task<bool> UpdateTeam(Guid Id, string team)
        {
            return await _userRepository.UpdateTeam(Id, team);
        }

        [Authorize]
        [Route("update-profile-picture")]
        [HttpPut]
        public async Task<bool> UpdateProfilePicture(string email, IFormFile picture)
        {
            return await _userRepository.UpdateProfilePicture(email, picture);
        }
    };
}
