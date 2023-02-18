using AutoMapper;
using Frisbee.ApiModels;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using FrisbeeApp.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Logic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FrisbeeAppContext _context;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public UserRepository(FrisbeeAppContext context, IAuthRepository authRepository, IMapper mapper)
        {
            _context = context;
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<List<TeamMemberDTO>> ViewTeam(string teamName)
        {
            var existingTeam = await _context.Teams.FirstOrDefaultAsync(x => x.TeamName == teamName) ?? throw new EntityNotFoundException("Team not found!");
            var teamMembers = await _context.Users.Where(x => x.Team == existingTeam.TeamName).ToListAsync();

            List<TeamMemberDTO> list= new List<TeamMemberDTO>();
            foreach (var teamMember in teamMembers) 
            {
                var role = await _authRepository.GetRole(teamMember.Email);
                if (role != null && role != ChosenRole.Coach.ToString())
                {
                    var member = _mapper.Map<TeamMemberDTO>(teamMember);
                    member.Role = role;
                    list.Add(member);
                }

            }
            return list;
        }

        public async Task<bool> UpdateUser(Guid Id, User user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new EntityNotFoundException("User not found"); 
            dbUser.FirstName = user.FirstName!=null ? user.FirstName : dbUser.FirstName;
            dbUser.LastName = user.LastName!=null ? user.LastName : dbUser.LastName;
            if(user.Email != null)
            {
                dbUser.UserName = user.Email;
                dbUser.NormalizedUserName = user.Email.ToUpper();
                dbUser.NormalizedEmail = user.Email.ToUpper();
                dbUser.Email = user.Email;
            }
            dbUser.BirthDate = user.BirthDate != DateTime.MinValue ? user.BirthDate : dbUser.BirthDate;
            dbUser.Gender = user.Gender != 0 ? user.Gender : dbUser.Gender;
            dbUser.PhoneNumber = user.PhoneNumber!= null ? user.PhoneNumber : dbUser.PhoneNumber;

             _context.Users.Update(dbUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTeam(Guid Id, string team)
        {
            var existingTeam = await _context.Teams.FirstOrDefaultAsync(x => x.TeamName == team) ?? throw new EntityNotFoundException("Team not found!");
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id) ?? throw new EntityNotFoundException("User ID not found"); 
            dbUser.Team= existingTeam.TeamName;

            _context.Users.Update(dbUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<TimeOffRequest>> ViewFilteredRequests(string email, SearchCriteria searchCriteria)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (dbUser == null)
                return null;
            var role = await _authRepository.GetRole(email);
            if(role == "Coach")
            {
                var timeOffRequestsList =  _context.TimeOffRequests.Where(x => x.TeamName == dbUser.Team && x.Status == RequestStatus.Pending);
                var filteredList = FilterRequestsByCriteria(timeOffRequestsList, searchCriteria).Result;
                return filteredList;
            }
            else if(role == "Player")
            {
                var timeOffRequestsList =  _context.TimeOffRequests.Where(x => x.UserEmail == email);
                var filteredList = FilterRequestsByCriteria(timeOffRequestsList, searchCriteria).Result;
                return filteredList;
            }
            return null;
        }

        private async Task<List<TimeOffRequest>> FilterRequestsByCriteria(IQueryable<TimeOffRequest> timeoffRequests, SearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                return await timeoffRequests.ToListAsync();
            }

            if (searchCriteria.Status != null)
            {
                timeoffRequests = timeoffRequests.Where(b => b.Status == searchCriteria.Status);
            }

            if (searchCriteria.PlayerName != null)
            {
                timeoffRequests = timeoffRequests.Where(b => b.UserName.ToLower().Contains(searchCriteria.PlayerName.ToLower()));
            }

            if (searchCriteria.RequestType != null)
            {
                timeoffRequests = timeoffRequests.Where(b => b.RequestType == searchCriteria.RequestType);
            }

            return await timeoffRequests.ToListAsync();
        }
    }
}
