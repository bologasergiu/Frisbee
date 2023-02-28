using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.EmailSender.Common;
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
        Task<bool> ConfirmAccount(string email, string userToken);
        void SendEmail(EmailTemplateType templateType, string email, ConfirmEmailModel model);
        Task<string> GenerateRegistrationToken (string email);
    }
}
