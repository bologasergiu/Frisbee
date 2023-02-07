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
    }
}
