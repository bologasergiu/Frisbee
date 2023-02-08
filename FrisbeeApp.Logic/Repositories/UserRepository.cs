using AutoMapper;
using FrisbeeApp.Context;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractions;
using FrisbeeApp.Logic.Abstractisations;
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
                if (role != null)
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
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Email = user.Email;
            dbUser.BirthDate = user.BirthDate;
            dbUser.Gender = user.Gender;
            dbUser.PhoneNumber = user.PhoneNumber;

             _context.Users.Update(dbUser);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTeam(Guid Id, string team)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            dbUser.Team= team;

            _context.Users.Update(dbUser);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
