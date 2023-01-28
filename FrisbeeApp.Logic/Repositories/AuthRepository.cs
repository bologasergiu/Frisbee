using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractisations;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace FrisbeeApp.Logic.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;

        public AuthRepository(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }
        public async Task<bool> Register(User user, string password, ChosenRole role)
        {
            var isUserRegistered = (await _userManager.CreateAsync(user, password)).Succeeded;
            var isUserAssignedRole = (await _userManager.AddToRoleAsync(user, role.ToString())).Succeeded;

            return isUserRegistered && isUserAssignedRole;
        }
    }
}
