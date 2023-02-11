using FrisbeeApp.DatabaseModels.Models;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IPlayerRepository
    {
        Task<bool> AddTimeOffRequest(TimeOffRequest timeOffRequest, string email);
        Task<List<TimeOffRequest>> ViewAllTimeOffRequest(string email);
    }
}
