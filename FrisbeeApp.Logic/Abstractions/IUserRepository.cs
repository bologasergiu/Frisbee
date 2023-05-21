using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;
using Microsoft.AspNetCore.Http;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IUserRepository
    {
        Task<List<TeamMemberDTO>> ViewTeam(string teamName);
        Task<bool> UpdateUser(string email, User user);
        Task<bool> UpdateTeam(Guid Id, string team);
        Task<List<TimeOffRequest>> ViewFilteredRequests(string email, SearchCriteria searchCriterea);
        Task<bool> UpdateProfilePicture(string email, IFormFile picture);
        Task<TeamMemberDTO> GetUserDetails(string email);
        Task<bool> ConfirmEmail(string email);
    }
}
