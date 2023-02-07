namespace FrisbeeApp.Logic.Abstractions
{
    public interface ICoachRepository
    {
        Task<bool> ApproveAccount(Guid id);
    }
}
