using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;

namespace FrisbeeApp.Logic.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FrisbeeAppContext _context;

        public AdminRepository(FrisbeeAppContext context)
        {
            _context = context;
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
    }
}
