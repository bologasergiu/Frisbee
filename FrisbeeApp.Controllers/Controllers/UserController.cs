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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
        private readonly UserManager<User> _userManager;


        public UserController(IMapper mapper, IAuthRepository repository, IUserRepository userRepository, IEmailService emailService, IPlayerRepository playerRepository, FrisbeeAppContext context, UserManager<User> userManager)
        {
            _mapper = mapper;
            _authRepository = repository;
            _userRepository = userRepository;
            _emailService = emailService;
            _playerRepository = playerRepository;
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<bool> Register(RegisterApiModel registerApiModel)
        {
            var registerUser = _mapper.Map<User>(registerApiModel);
            var registerResult = await _authRepository.Register(registerUser, registerApiModel.Password, registerApiModel.Role);
            var token = await _authRepository.GenerateRegistrationToken(registerUser.Email);
            var link = Url.Action("ConfirmEmail", "Email", new { userEmail = HttpUtility.UrlEncode(registerUser.Email), userToken = HttpUtility.UrlEncode(token), returnUrl = "http://localhost:4200/login" }, protocol: Request.Scheme);

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
        [Route("approve-account/{id}")]
        [HttpPut]
        public async Task<bool> ApproveAccount(Guid id)
        {
            return await _authRepository.ApproveAccount(id, User.Identity.Name);
        }

        [Authorize]
        [Route("view-team/{teamName}")]
        [HttpGet]
        public async Task<List<TeamMemberDTO>> ViewTeam(string teamName)
        {
            return await _userRepository.ViewTeam(teamName);
        }
            
        [Authorize]
        [Route("update-user/{email}")]
        [HttpPut]
        public async Task<bool> UpdateUser(string email, UpdateUserApiModel updateUserApiModel)
        {
            var user = _mapper.Map<User>(updateUserApiModel);
            return await _userRepository.UpdateUser(email, user);
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

        [Authorize]
        [Route("user-info/{email}")]
        [HttpGet]
        public async Task<TeamMemberDTO> GetUserDetails()
        {
            return await _userRepository.GetUserDetails(User.Identity.Name);
        }

        [Authorize]
        [Route("confirmEmail/{email}")]
        [HttpPut]
        public async Task<bool> ConfirmEmail(string email)
        {
            return await _userRepository.ConfirmEmail(email);
        }
        [AllowAnonymous]
        [Route("change-password-request/{email}")]
        [HttpPut]
        public async Task<bool> ChangePasswordRequest(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return false;
            }

            var token = await _authRepository.GenerateConfirmNewPasswordToken(email);
            var returnUrl = $"http://localhost:4200/change-password?userEmail={HttpUtility.UrlEncode(user.Email)}&userToken={HttpUtility.UrlEncode(token)}";
            var link = Url.Action("ConfirmNewPassword", "Email", new { userEmail = HttpUtility.UrlEncode(user.Email), userToken = HttpUtility.UrlEncode(token), returnUrl }, protocol: Request.Scheme);

            _emailService.SendEmail(EmailTemplateType.PasswordChanged, new List<string> { email }, new EmailInfo
            {
               Link = link
            }); ;
            return true;

            
        }

        [AllowAnonymous]
        [Route("change-password")]
        [HttpPut]
        public async Task<bool> ChangePassword(ChangePasswordApiModel changePasswordApiModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == changePasswordApiModel.Email);
            if (user == null)
            {
                return false;
            }
            var token = await _authRepository.GenerateConfirmNewPasswordToken(changePasswordApiModel.Email);
            var result = await _userManager.ResetPasswordAsync(user, token, changePasswordApiModel.Password);

            return true;


        }
    };
}
       