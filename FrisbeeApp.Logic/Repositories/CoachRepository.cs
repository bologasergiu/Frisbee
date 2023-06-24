using AutoMapper;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.IO;
using System.Collections;

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
            var timeoffRequests = await _context.TimeOffRequests.Where(x => x.TeamName == existingUser.Team).ToListAsync();

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

        public async Task<List<string>> GetTeamEmailList(string email)
        {
            var coach = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (coach == null || _context == null)
            {
                return new List<string>();
            }
            var coachTeam = coach.Team;
            var result = _context.Users.Where(x=> x.Team == coachTeam).Select(x=>x.Email).ToList();

            return result;
        }

        public async Task<string> GetTimeOffRequestEmailAddress(Guid Id)
        {
            var dbRequest = await _context.TimeOffRequests.FirstOrDefaultAsync(x => x.Id == Id);
            return dbRequest.UserEmail;
        }

        public async Task<List<Training>> GetTrainings()
        {
            var trainings = await _context.Trainings.ToListAsync();
            return trainings;
        }

        public async Task<List<TeamModelDTO>> GetAllTeams()
        {
            var teamsList = await _context.Teams.ToListAsync();
            return teamsList.Select(team => new TeamModelDTO
            {
                TeamName = team.TeamName,
                Id = team.Id,
                NumberOfMembers = getNumberOfMembers(team.TeamName)
            }).ToList();
        }

        public async Task<List<Training>> GetTrainingsPerTeam(string teamName)
        {
            var trainings = await _context.Trainings.Where(x => x.Team == teamName).ToListAsync();
            return trainings;
        }

        private int getNumberOfMembers(string teamName)
        {
            List<User> users = new List<User>();
            users = _context.Users.Where(x => x.Team == teamName).ToList();
            return users.Count;
        }

        
    }
}
