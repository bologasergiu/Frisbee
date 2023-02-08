using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IUserRepository
    {
        Task<List<TeamMemberDTO>> ViewTeam(string teamName);
        Task<bool> UpdateUser(Guid Id, User user);
        Task<bool> UpdateTeam(Guid Id, string team);
    }
}
