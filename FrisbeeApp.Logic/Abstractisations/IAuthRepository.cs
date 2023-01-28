using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.DtoModels;

namespace FrisbeeApp.Logic.Abstractisations
{
    public interface IAuthRepository
    {
        Task<bool> Register(User user, string password, ChosenRole role);
        //Task<TokenDto> Login(LoginDto loginUser);
        //Task Logout();
    }
}
