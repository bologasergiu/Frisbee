using AutoMapper;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FrisbeeApp.Logic.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly FrisbeeAppContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthRepository _authenticationRepository;
        private readonly IMapper _mapper;

        public AdminRepository(FrisbeeAppContext context, UserManager<User> userManager,IAuthRepository authenticationRepository, IMapper mapper)
        {
            _context = context;
            _userManager = userManager; 

            _authenticationRepository = authenticationRepository;  
            _mapper = mapper;
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

        public async Task<bool> DeleteTeam(string name)
        {
            var dbTeam = await _context.Teams.FirstOrDefaultAsync(x=>x.TeamName == name);
            List<User> users = new List<User>();
            users = await _context.Users.Where(x => x.Team == name).ToListAsync();

            if (dbTeam != null)
            {
                foreach (var user in users)
                    {
                    var role = await _authenticationRepository.GetRole(user.Email);
                    if(role != ChosenRole.Admin.ToString()) {
                       _context.Users.Remove(user);
                    }
                    
                }
                _context.Teams.Remove(dbTeam);
                await _context.SaveChangesAsync();
            } 
            return false;
        }

        public async Task<bool> DeleteUser(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            var role = await _authenticationRepository.GetRole(dbUser.Email);
            if(role == ChosenRole.Admin.ToString())
            {
                return false;
            }
            var removedRole= await _userManager.RemoveFromRoleAsync(dbUser, role);
            var removedUser = await _userManager.DeleteAsync(dbUser);

            return removedRole.Succeeded && removedUser.Succeeded;
        }

        public async Task<List<TeamModelDTO>> GetAllTeams()
        {
            var teams = await _context.Teams.ToListAsync();

            return teams.Select(team => new TeamModelDTO
            {
                TeamName = team.TeamName,
                Id = team.Id,
                NumberOfMembers = getNumberOfMembers(team.TeamName)
            }).ToList();
        }

        public async Task<List<TeamMemberDTO>> GetAllUsers()
        {
            List<User> users = new List<User>();
            users = await _context.Users.ToListAsync();
            var mappedUsers = _mapper.Map<List<TeamMemberDTO>>(users);
            List<TeamMemberDTO> result = new List<TeamMemberDTO>();

            foreach (var mappedUser in mappedUsers)
            {            
                var userRole = await _authenticationRepository.GetRole(mappedUser.Email);
                if(userRole == ChosenRole.Admin.ToString())
                {
                    mappedUser.Role = ChosenRole.Admin;
                }else if(userRole == ChosenRole.Coach.ToString())
                {
                    mappedUser.Role = ChosenRole.Coach;
                }
                else if(userRole == ChosenRole.Player.ToString())
                {
                    mappedUser.Role = ChosenRole.Player;
                }
                result.Add(mappedUser);
            }
            return result;
        }

        public async Task<TeamModelDTO> GetTeamModel(string teamName)
        {
            TeamModelDTO teamModelDTO = new TeamModelDTO();
            var dbTeam = await _context.Teams.FirstOrDefaultAsync(x=>x.TeamName == teamName);
            teamModelDTO.TeamName = teamName;
            teamModelDTO.NumberOfMembers = getNumberOfMembers(teamName);
            teamModelDTO.Id = dbTeam.Id;

            return teamModelDTO;
        }

        private int getNumberOfMembers(string teamName)
        {
            List<User> users = new List<User>();
            users = _context.Users.Where(x=>x.Team ==  teamName).ToList();
            return users.Count;
        }

    }
}
