using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Abstractions;
using FrisbeeApp.EmailSender.Common;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace FrisbeeApp.Logic.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly FrisbeeAppContext _context;
        private readonly IEmailService _emailService;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, ITokenService tokenService, FrisbeeAppContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        public async Task<TokenDTO> Login(LoginDTO loginUser)
        {
            var registeredUser = await _userManager.FindByEmailAsync(loginUser.Email);
            if (registeredUser == null)
            {
                return null;
            }
            var loginResult = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (loginResult.Succeeded)
            {
                 string role = GetRole(loginUser.Email).Result;
                 string generatedToken = _tokenService.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), loginUser, role);
                 if (generatedToken != null)
                 {
                     TokenDTO tokenDto = new() { Token = generatedToken };
                     return tokenDto;
                 }
                 else
                     return null;
            }
            else
                return null;
        }

        public async Task<bool> Register(User user, string password, ChosenRole role)
        {
            var existingTeam = await _context.Teams.FirstOrDefaultAsync(x=>x.TeamName==user.Team) ?? throw new EntityNotFoundException("Team not found!");
            var isUserRegistered = (await _userManager.CreateAsync(user, password)).Succeeded;
            var isUserAssignedRole = (await _userManager.AddToRoleAsync(user, role.ToString())).Succeeded;

            var result = isUserRegistered && isUserAssignedRole;
            
            return result;
        }

        public async Task<string> GetRole(string email)
        {
            string result = "";
            var userId = await _context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();
            if (userId != Guid.NewGuid())
            {
                var userRoleId = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).FirstOrDefaultAsync();
                if (userRoleId != Guid.NewGuid())
                {
                    if (userRoleId == Constants.PlayerRoleID)
                        result = ChosenRole.Player.ToString();
                    else if (userRoleId == Constants.CoachRoleID)
                        result = ChosenRole.Coach.ToString();
                    else if (userRoleId == Constants.AdminRoleID)
                        result = ChosenRole.Admin.ToString();
                }
                else
                    throw new EntityNotFoundException($"The UserRole for {email} was not found!");
            }
            else
                throw new EntityNotFoundException($"The user with the email {email} was not found!");

            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<bool> ApproveAccount(Guid id, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new EntityNotFoundException("UserId does not exist.");
            var dbRole = await GetRole(dbUser.Email);
            var currentUserRole = await GetRole(email);
            if (currentUserRole == ChosenRole.Coach.ToString())
            {
                if (dbRole == ChosenRole.Player.ToString())
                {
                    dbUser.AccountApproved = true;
                    _context.Users.Update(dbUser);
                }
            }
            else if(currentUserRole == ChosenRole.Admin.ToString())
            {
                if (dbRole == ChosenRole.Coach.ToString())
                {
                    dbUser.AccountApproved = true;
                    _context.Users.Update(dbUser);
                }
            }

            return await _context.SaveChangesAsync() == 1;
        }

        public async Task<string> GenerateRegistrationToken(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            string token = null;
            if(dbUser != null)
            {
                token = await _userManager.GenerateEmailConfirmationTokenAsync(dbUser);
            }
            return token;
        }
    }
}
