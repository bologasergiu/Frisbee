using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
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

        public async Task<bool> AddTraining(Training training, string email)
        {
            training.Id = Guid.NewGuid();
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new EntityNotFoundException("Coach not found!");
            training.CoachName = dbUser.LastName + " " + dbUser.FirstName;
            training.Team = dbUser.Team;

            await _context.Trainings.AddAsync(training);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }
    }
}
