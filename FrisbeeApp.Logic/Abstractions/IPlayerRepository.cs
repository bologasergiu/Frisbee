using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Common;

namespace FrisbeeApp.Logic.Abstractions
{
    public interface IPlayerRepository
    {
        Task<bool> AddTimeOffRequest(TimeOffRequest timeOffRequest, string email);
        Task<List<TimeOffRequest>> ViewAllTimeOffRequest(string email);
        Task<bool> CancelledTimeOffRequest(Guid Id);
        Task<List<string>> GetCochEmailList(string email);
    }
}
