using FrisbeeApp.Context;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Logic.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly FrisbeeAppContext _context;
        private readonly IAuthRepository _repository;

        public CoachRepository(FrisbeeAppContext context, IAuthRepository repository)
        {
            _context= context;  
            _repository= repository;    
        }
        public async Task<bool> ApproveAccount(Guid id)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id) ?? throw new EntityNotFoundException("UserId does not exist.");
            var role = await _repository.GetRole(dbUser.Email);
            if (role == ChosenRole.Player.ToString())
            {
                dbUser.AccountApproved = true;
                _context.Users.Update(dbUser);
            }
           
            return await _context.SaveChangesAsync() == 1;
        }
    }
}
