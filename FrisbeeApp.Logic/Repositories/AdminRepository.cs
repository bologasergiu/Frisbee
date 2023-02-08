using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Logic.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FrisbeeAppContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthRepository _authenticationRepository;

        public AdminRepository(FrisbeeAppContext context, UserManager<User> userManager, IAuthRepository authenticationRepository)
        {
            _context = context;
            _userManager = userManager; 
            _authenticationRepository = authenticationRepository;   
        }
        public async Task<bool> CreateTeam(string teamName)
        {
            Team team = new Team
            {
                Id= Guid.NewGuid(),
                TeamName=teamName 
            };
            await _context.Teams.AddAsync(team);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> DeleteUser(Guid Id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            var role = await _authenticationRepository.GetRole(dbUser.Email);
            var removedRole= await _userManager.RemoveFromRoleAsync(dbUser, role);
            var removedUser = await _userManager.DeleteAsync(dbUser);

            return removedRole.Succeeded && removedUser.Succeeded;
        }

        
    }
}
