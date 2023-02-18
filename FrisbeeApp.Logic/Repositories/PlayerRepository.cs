using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace FrisbeeApp.Logic.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly FrisbeeAppContext _context;

        public PlayerRepository(FrisbeeAppContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTimeOffRequest(TimeOffRequest timeOffRequest, string email)
        {
            timeOffRequest.Id = Guid.NewGuid();
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email) ?? throw new EntityNotFoundException("User does not exist.");
            timeOffRequest.UserEmail = email;
            timeOffRequest.UserName = dbUser.FirstName + " " + dbUser.LastName;
            timeOffRequest.Status = RequestStatus.Pending;
            timeOffRequest.TeamName = dbUser.Team;
            await _context.TimeOffRequests.AddAsync(timeOffRequest);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<bool> CancelledTimeOffRequest(Guid Id)
        {
            var existingTimeOffRequest = await _context.TimeOffRequests.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new EntityNotFoundException("Timeoff request does not exist!");
            if (existingTimeOffRequest.Status == RequestStatus.Cancelled)
            {
                throw new RequestAlreadyCancelled("Request is already cancelled and can not be edited!");
            }

            existingTimeOffRequest.Status = RequestStatus.Cancelled;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }

        public async Task<List<TimeOffRequest>> ViewAllTimeOffRequest(string email)
        {
            var existingTimeOffRequest = await _context.TimeOffRequests.FirstOrDefaultAsync(x => x.UserEmail == email) ?? throw new EntityNotFoundException("Team not found!");
            var timeOffRequests = await _context.TimeOffRequests.Where(x=>x.UserEmail== email).ToListAsync();

            return timeOffRequests;
        }
    }
}
