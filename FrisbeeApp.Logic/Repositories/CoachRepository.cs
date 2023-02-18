using AutoMapper;
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

        public CoachRepository(FrisbeeAppContext context)
        {
            _context = context;
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

        public async Task<List<TimeOffRequest>> ViewAllTimeOffRequestsPerTeam(string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email== email) ?? throw new EntityNotFoundException("Team ID not found!");
            var timeoffRequests = await _context.TimeOffRequests.Where(x => x.TeamName == existingUser.Team && x.Status==RequestStatus.Pending).ToListAsync();

            return timeoffRequests;
        }
        public async Task<bool> ChangeTimeoffRequestStatus(Guid Id, RequestStatus status, string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x=>x.Email== email); 
            var timeOffRequest = await _context.TimeOffRequests.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new EntityNotFoundException("Timeoff request does not exist!");
            if (dbUser.Team == timeOffRequest.TeamName)
            {
                if (timeOffRequest.Status == RequestStatus.Cancelled)
                {
                    throw new RequestAlreadyCancelled("Request is already cancelled and can't be edited!");
                }
                if (timeOffRequest.Status == RequestStatus.Pending)
                {
                    switch (status)
                    {
                        case RequestStatus.Cancelled:
                            timeOffRequest.Status = RequestStatus.Cancelled;
                            break;
                        case RequestStatus.Rejected:
                            timeOffRequest.Status = RequestStatus.Rejected;
                            break;
                        case RequestStatus.Approved:
                            timeOffRequest.Status = RequestStatus.Approved;
                            break;
                        default:
                            throw new EntityNotFoundException("Status doesn't exist");
                    }
                }
                else
                {
                    throw new Exception("Can't modify request with other status than pending.");
                }

                await _context.SaveChangesAsync();
                return true;
            }
            throw new Exception("Can't modify requests for players from other teams.");
        }
    }
}
