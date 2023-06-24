using Frisbee.ApiModels;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface ICoachRepository
    {
        Task<bool> AddTraining(Training training, string name);
        Task<List<TimeOffRequest>> ViewAllTimeOffRequestsPerTeam(string teamName);
        Task<bool> ChangeTimeoffRequestStatus(Guid id, RequestStatus status, string email);
        Task<List<string>> GetTeamEmailList(string email);
        Task<string> GetTimeOffRequestEmailAddress(Guid Id);
        Task<List<Training>> GetTrainings();
        Task<List<TeamModelDTO>> GetAllTeams();
        Task<List<Training>> GetTrainingsPerTeam(string teamName);
    }
}
