using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractisations
{
    public interface IAuthRepository
    {
        Task<bool> Register(User user, string password, ChosenRole role);
        Task<TokenDTO> Login(LoginDTO loginUser);
        Task Logout();
        Task<string> GetRole(string email);
        Task<bool> ApproveAccount(Guid id, string email);
    }
}
