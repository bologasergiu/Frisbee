using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Common;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IPlayerRepository
    {
        Task<bool> AddTimeOffRequest(TimeOffRequest timeOffRequest, string email);
        Task<List<TimeOffRequest>> ViewAllTimeOffRequest(string email);
        Task<bool> DeleteTimeOffRequest(Guid Id);
        Task<string> GetCoachEmail(string email);
        Task<List<Training>> GetTrainings(string email);
    }
}
